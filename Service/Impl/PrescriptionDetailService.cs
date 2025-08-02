// Service/Impl/PrescriptionDetailService.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PrescriptionDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class PrescriptionDetailService : IPrescriptionDetailService
    {
        private readonly ApplicationDBContext _context;
        private readonly IPrescriptionDetailMapper _mapper;

        public PrescriptionDetailService(ApplicationDBContext context, IPrescriptionDetailMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PrescriptionDetailResponseDTO>> GetAllAsync()
        {
            var list = await _context.PrescriptionDetails
                .AsNoTracking()
                .ToListAsync();
            return list.Select(_mapper.MapToResponse).ToList();
        }

        public async Task<PrescriptionDetailResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _context.PrescriptionDetails.FindAsync(id);
            return entity == null ? null : _mapper.MapToResponse(entity);
        }



        public async Task<List<PrescriptionDetailResponseDTO>> GetByPrescriptionIdAsync(int prescriptionId)
        {
            // 1. Lấy danh sách detail kèm Medicine
            var details = await _context.PrescriptionDetails
                .Where(d => d.PrescriptionId == prescriptionId)
                .Include(d => d.Medicine)        // để có d.Medicine.Name, d.Medicine.Code
                .AsNoTracking()
                .ToListAsync();

            // 2. Tập hợp medicineId duy nhất
            var meds = details.Select(d => d.MedicineId).Distinct().ToList();

            // 3. Build map medicineId -> unitPrice (lấy lô InStock sớm nhất)
            var priceMap = new Dictionary<int, decimal>();
            foreach (var mid in meds)
            {
                var price = await _context.Medicine_Inventories
                    .Where(mi => mi.MedicineId == mid
                              && mi.Quantity > 0
                              && mi.Status == (int)MedicineInventoryStatus.InStock)
                    .OrderBy(mi => mi.ExpiryDate)
                    .Select(mi => mi.UnitPrice)
                    .FirstOrDefaultAsync();
                priceMap[mid] = price;
            }

            // 4. Map ra DTO
            var result = details.Select(d =>
            {
                var up = priceMap.TryGetValue(d.MedicineId, out var p) ? p : 0m;
                return new PrescriptionDetailResponseDTO
                {
                    Id = d.Id,
                    PrescriptionId = d.PrescriptionId,
                    MedicineId = d.MedicineId,
                    MedicineName = d.Medicine.Name,
                    Quantity = d.Quantity,
                    Usage = d.Usage,
                    Status = d.Status.ToString(),
                    CreateDate = d.CreateDate,
                    UpdateDate = d.UpdateDate!.Value,
                    CreateBy = d.CreateBy,
                    UpdateBy = d.UpdateBy,
                    UnitPrice = up,
                    Total = up * d.Quantity
                };
            }).ToList();

            return result;
        }



        public async Task<PrescriptionDetailResponseDTO> CreateAsync(PrescriptionDetailRequest req, int userId)
        {
            // Kiểm tra số lượng thuốc
            if (req.Quantity <= 0)
                throw new InvalidOperationException("Số lượng thuốc phải lớn hơn 0.");

            // Lấy Prescription
            var prescription = await _context.Prescriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == req.PrescriptionId);

            if (prescription == null)
                throw new InvalidOperationException("Không tìm thấy đơn thuốc.");

            // Kiểm tra trạng thái đơn thuốc
            if (prescription.Status != PrescriptionStatus.New)
                throw new InvalidOperationException("Chỉ có thể thêm chi tiết cho đơn thuốc mới.");

            // Tìm Medicine
            var medicine = await _context.Medicines
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Name == req.MedicineName);

            if (medicine == null)
                throw new InvalidOperationException($"Không tìm thấy thuốc {req.MedicineName}");

            // ----------- KIỂM TRA ĐÃ CÓ CHI TIẾT THUỐC NÀY CHƯA -----------
            var existedDetail = await _context.PrescriptionDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(pd => pd.PrescriptionId == req.PrescriptionId && pd.MedicineId == medicine.Id);

            if (existedDetail != null)
                throw new InvalidOperationException($"Thuốc '{medicine.Name}' đã có trong đơn thuốc này. Vui lòng chọn thuốc khác hoặc sửa số lượng ở chi tiết đã có.");

            // Kiểm tra kho thuốc
            var inventories = await _context.Medicine_Inventories
                .Where(mi => mi.MedicineId == medicine.Id
                          && mi.Quantity > 0
                          && mi.Status == 0) // In stock
                .OrderBy(mi => mi.ExpiryDate)
                .ToListAsync();

            var totalAvailable = inventories.Sum(mi => mi.Quantity);
            if (totalAvailable < req.Quantity)
                throw new InvalidOperationException($"Không đủ thuốc trong kho. Cần {req.Quantity}, còn {totalAvailable}.");

            // Tìm bác sĩ để gán CreateBy/UpdateBy
            var doctor = await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
                throw new InvalidOperationException("Không tìm thấy bác sĩ.");

            // Tạo PrescriptionDetail
            var detail = new PrescriptionDetail
            {
                PrescriptionId = req.PrescriptionId,
                MedicineId = medicine.Id,
                Quantity = req.Quantity,
                Usage = req.Usage,
                Status = (PrescriptionDetailStatus)req.Status,
                Name = medicine.Name,
                Code = medicine.Code,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateBy = doctor.Name!,
                UpdateBy = doctor.Name!
            };

            _context.PrescriptionDetails.Add(detail);
            await _context.SaveChangesAsync();

            // Cập nhật lại Amount cho Prescription sau khi thêm chi tiết
            await UpdatePrescriptionAmount(req.PrescriptionId);

            return _mapper.MapToResponse(detail);
        }






        public async Task<bool> DeleteAsync(int id)
        {
            var detail = await _context.PrescriptionDetails.FindAsync(id);
            if (detail == null) return false;
            _context.PrescriptionDetails.Remove(detail);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<PrescriptionDetailResponseDTO>> GetByUserAsync(int userId, string role)
        {
            // 1. Lấy toàn bộ detail kèm Prescription → Doctor/Patient
            IQueryable<PrescriptionDetail> query = _context.PrescriptionDetails
                .Include(d => d.Prescription)
                    .ThenInclude(p => p.Doctor)
                .Include(d => d.Prescription)
                    .ThenInclude(p => p.Patient)
                .Include(d => d.Medicine);  // có thể include Medicine để lấy tên

            if (role.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
                query = query.Where(d => d.Prescription.Doctor.UserId == userId);
            else if (role.Equals("Patient", StringComparison.OrdinalIgnoreCase))
                query = query.Where(d => d.Prescription.Patient.UserId == userId);

            var list = await query.AsNoTracking().ToListAsync();

            // 2. Tập hợp medicineId duy nhất để không query lặp
            var meds = list.Select(d => d.MedicineId).Distinct().ToList();

            // 3. Lấy price FIFO cho mỗi medicineId
            var priceMap = new Dictionary<int, decimal>();
            foreach (var mid in meds)
            {
                var price = await _context.Medicine_Inventories
                    .Where(mi => mi.MedicineId == mid
                              && mi.Quantity > 0
                              && mi.Status == (int)MedicineInventoryStatus.InStock /*0 nếu bạn dùng int*/)
                    .OrderBy(mi => mi.ExpiryDate)
                    .Select(mi => mi.UnitPrice)
                    .FirstOrDefaultAsync();

                priceMap[mid] = price;
            }

            // 4. Map ra DTO
            var result = list.Select(d =>
            {
                var up = priceMap.GetValueOrDefault(d.MedicineId, 0m);
                return new PrescriptionDetailResponseDTO
                {
                    Id = d.Id,
                    PrescriptionId = d.PrescriptionId,
                    MedicineId = d.MedicineId,
                    MedicineName = d.Medicine.Name,
                    Quantity = d.Quantity,
                    Usage = d.Usage,
                    Status = d.Status.ToString(),
                    CreateDate = d.CreateDate,
                    UpdateDate = d.UpdateDate!.Value,
                    CreateBy = d.CreateBy,
                    UpdateBy = d.UpdateBy,
                    UnitPrice = up,
                    Total = up * d.Quantity
                };
            });

            return result;
        }


        public async Task UpdatePrescriptionAmount(int prescriptionId)
        {
            // Lấy tất cả chi tiết của đơn thuốc
            var prescriptionDetails = await _context.PrescriptionDetails
                .Where(pd => pd.PrescriptionId == prescriptionId)
                .Include(pd => pd.Medicine) // Đảm bảo lấy thông tin về Medicine
                .ToListAsync();

            // Tính tổng giá trị của đơn thuốc (Tổng = đơn giá * số lượng)
            decimal totalAmount = prescriptionDetails.Sum(pd => pd.Medicine.UnitPrice * pd.Quantity);

            // Cập nhật Amount cho Prescription
            var prescription = await _context.Prescriptions.FindAsync(prescriptionId);
            if (prescription != null)
            {
                prescription.Amount = totalAmount;
                await _context.SaveChangesAsync(); // Lưu lại tổng giá trị
            }
        }



        public async Task<PrescriptionDetailResponseDTO> UpdateAsync(int id, PrescriptionDetailRequest req, int userId)
        {
            // 1. Tìm detail cần sửa
            var detail = await _context.PrescriptionDetails.Include(d => d.Medicine).FirstOrDefaultAsync(d => d.Id == id);
            if (detail == null)
                throw new Exception("Không tìm thấy chi tiết đơn thuốc.");

            // 2. Kiểm tra trạng thái đơn thuốc (chỉ cho phép sửa khi đơn còn trạng thái 'New')
            var prescription = await _context.Prescriptions.FirstOrDefaultAsync(p => p.Id == detail.PrescriptionId);
            if (prescription == null)
                throw new Exception("Không tìm thấy đơn thuốc.");
            if (prescription.Status != PrescriptionStatus.New)
                throw new Exception("Chỉ có thể sửa chi tiết cho đơn thuốc mới.");

            // 3. Kiểm tra bác sĩ
            var doctor = await _context.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
                throw new Exception("Không tìm thấy bác sĩ.");

            // 4. Tìm Medicine mới theo tên (cho phép đổi thuốc)
            var medicine = await _context.Medicines.AsNoTracking().FirstOrDefaultAsync(m => m.Name == req.MedicineName);
            if (medicine == null)
                throw new Exception($"Không tìm thấy thuốc {req.MedicineName}");

            // 5. Kiểm tra kho thuốc (trừ đi chính số lượng cũ trong detail để tính lại khả dụng)
            var inventories = await _context.Medicine_Inventories
                .Where(mi => mi.MedicineId == medicine.Id
                          && mi.Quantity > 0
                          && mi.Status == (int)MedicineInventoryStatus.InStock)
                .OrderBy(mi => mi.ExpiryDate)
                .ToListAsync();

            // Tính tổng khả dụng: cộng lại số lượng cũ vì sắp sửa update
            int totalAvailable = inventories.Sum(mi => mi.Quantity);

            if (totalAvailable < req.Quantity)
                throw new Exception($"Không đủ thuốc trong kho. Cần {req.Quantity}, còn {totalAvailable}.");

            // Kiểm tra nếu lấy hết kho
            bool isDepleted = (totalAvailable == req.Quantity);

            // 6. Cập nhật lại detail
            detail.MedicineId = medicine.Id;
            detail.Name = medicine.Name;
            detail.Code = medicine.Code;
            detail.Quantity = req.Quantity;
            detail.Usage = req.Usage;
            detail.Status = (PrescriptionDetailStatus)req.Status;
            detail.UpdateDate = DateTime.UtcNow;
            detail.UpdateBy = doctor.Name;

            await _context.SaveChangesAsync();

            // 7. Cập nhật lại Amount cho Prescription
            await UpdatePrescriptionAmount(detail.PrescriptionId);

            // 8. Nếu lấy hết thuốc thì trả về message đặc biệt
            if (isDepleted)
                throw new Exception("Bạn đã lấy hết số thuốc trong kho cho loại này.");

            // 9. Trả về DTO response như thường
            return _mapper.MapToResponse(detail);
        }



    }



}
