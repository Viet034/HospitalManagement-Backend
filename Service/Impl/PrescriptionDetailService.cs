// Service/Impl/PrescriptionDetailService.cs
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PrescriptionDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var list = await _context.PrescriptionDetails
                .Where(d => d.PrescriptionId == prescriptionId)
                .AsNoTracking()
                .ToListAsync();
            return list.Select(_mapper.MapToResponse).ToList();
        }

        public async Task<PrescriptionDetailResponseDTO> CreateAsync(PrescriptionDetailRequest req)
        {
            // 1) Lấy thông tin bác sĩ từ UserId
            var doctor = await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == req.UserId);
            if (doctor == null)
                throw new Exception("Không tìm thấy bác sĩ.");

            // 2) Lấy thuốc gốc để copy Name/Code
            var med = await _context.Medicines
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == req.MedicineId);
            if (med == null)
                throw new Exception("Không tìm thấy thuốc.");

            // 3) Khởi tạo entity
            var detail = new PrescriptionDetail
            {
                PrescriptionId = req.PrescriptionId,
                MedicineId = req.MedicineId,
                Quantity = req.Quantity,
                Usage = req.Usage,
                Status = (SWP391_SE1914_ManageHospital.Ultility.Status.PrescriptionDetailStatus)req.Status,
                Name = med.Name,
                Code = med.Code,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateBy = doctor.Name!,
                UpdateBy = doctor.Name!
            };

            _context.PrescriptionDetails.Add(detail);
            await _context.SaveChangesAsync();
            return _mapper.MapToResponse(detail);
        }

        public async Task<PrescriptionDetailResponseDTO?> UpdateAsync(int id, PrescriptionDetailRequest req)
        {
            var detail = await _context.PrescriptionDetails.FindAsync(id);
            if (detail == null) return null;

            // Lấy bác sĩ từ UserId
            var doctor = await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == req.UserId);
            if (doctor == null)
                throw new Exception("Không tìm thấy bác sĩ.");

            // Ánh xạ các trường có thể thay đổi
            detail.Quantity = req.Quantity;
            detail.Usage = req.Usage;
            detail.Status = (SWP391_SE1914_ManageHospital.Ultility.Status.PrescriptionDetailStatus)req.Status;
            detail.UpdateDate = DateTime.UtcNow;
            detail.UpdateBy = doctor.Name!;

            // Cập nhật Name/Code nếu đổi MedicineId
            if (detail.MedicineId != req.MedicineId)
            {
                var med = await _context.Medicines
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == req.MedicineId);
                if (med != null)
                {
                    detail.MedicineId = req.MedicineId;
                    detail.Name = med.Name;
                    detail.Code = med.Code;
                }
            }

            await _context.SaveChangesAsync();
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
            IQueryable<PrescriptionDetail> query = _context.PrescriptionDetails
                .Include(d => d.Prescription)       // để join Prescription
                .ThenInclude(p => p.Doctor)
                .Include(d => d.Prescription)
                .ThenInclude(p => p.Patient);

            if (role.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
            {
                // Lọc theo Doctor.UserId
                query = query.Where(d => d.Prescription.Doctor.UserId == userId);
            }
            else if (role.Equals("Patient", StringComparison.OrdinalIgnoreCase))
            {
                // Lọc theo Patient.UserId
                query = query.Where(d => d.Prescription.Patient.UserId == userId);
            }
            else
            {
                // nếu là Admin hoặc các role khác: trả về tất cả
            }

            var list = await query.ToListAsync();
            return list.Select(d => _mapper.MapToResponse(d));
        }
    }
}
