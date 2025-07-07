// Services/PrescriptionService.cs
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Prescription;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Mapper;
using Microsoft.EntityFrameworkCore;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly ApplicationDBContext _context;
        private readonly IPrescriptionMapper _mapper;

        public PrescriptionService(ApplicationDBContext context, IPrescriptionMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrescriptionResponseDTO>> GetAllAsync()
        {
            var list = await _context.Prescriptions.ToListAsync();
            return list.Select(p => _mapper.MapToResponse(p));
        }

        public async Task<IEnumerable<PrescriptionResponseDTO>> GetByDoctorIdAsync(int doctorId)
        {
            var prescriptions = await _context.Prescriptions
                .Where(p => p.DoctorId == doctorId)
                .ToListAsync();  // Lấy tất cả đơn thuốc của bác sĩ

            // Ánh xạ từ entities sang DTO (PrescriptionResponseDTO)
            return prescriptions.Select(p => _mapper.MapToResponse(p));
        }

        // Lấy đơn thuốc của bác sĩ hiện tại từ UserId
        public async Task<IEnumerable<PrescriptionResponseDTO>> GetByUserIdAsync(int userId)
        {
            // Tìm DoctorId từ UserId
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
            {
                throw new Exception("Không tìm thấy bác sĩ.");
            }

            return await GetByDoctorIdAsync(doctor.Id); // Dùng phương thức trên để lấy đơn thuốc theo DoctorId
        }

        public async Task<PrescriptionResponseDTO> GetByIdAsync(int id)
        {
            var p = await _context.Prescriptions.FindAsync(id);
            return p == null ? null : _mapper.MapToResponse(p);
        }

        // Lấy tất cả đơn thuốc của bác sĩ theo DoctorId
        

        public async Task<PrescriptionResponseDTO> CreateAsync(PrescriptionRequest request)
        {
            // Lấy bác sĩ qua UserId
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == request.UserId);
            if (doctor == null)
                throw new Exception("Không tìm thấy bác sĩ.");

            var entity = new Prescription
            {
                Note = request.Note,
                Status = (PrescriptionStatus)request.Status,
                PatientId = request.PatientId,
                DoctorId = doctor.Id,
                Name = request.Name,
                Code = request.Code,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateBy = doctor.Name,
                UpdateBy = doctor.Name
            };

            _context.Prescriptions.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.MapToResponse(entity);
        }

        public async Task<PrescriptionResponseDTO> UpdateAsync(int id, PrescriptionRequest request)
        {
            var entity = await _context.Prescriptions.FindAsync(id);
            if (entity == null) return null;

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == request.UserId);
            if (doctor == null)
                throw new Exception("Không tìm thấy bác sĩ.");

            entity.Note = request.Note;
            entity.Status = (PrescriptionStatus)request.Status;
            entity.PatientId = request.PatientId;
            entity.Name = request.Name;
            entity.Code = request.Code;
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdateBy = doctor.Name;

            await _context.SaveChangesAsync();
            return _mapper.MapToResponse(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Prescriptions.FindAsync(id);
            if (entity == null) return false;
            _context.Prescriptions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
