﻿using SWP391_SE1914_ManageHospital.Data;
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



        private async Task<bool> IsDoctor(int userId)
        {
            var ur = await _context.User_Roles
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (ur == null) return false;

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == ur.RoleId);
            return role != null && role.Name == "Doctor";
        }


        public async Task<IEnumerable<PrescriptionResponseDTO>> GetByUserIdAsync(int userId)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
            {
                throw new Exception("Không tìm thấy bác sĩ.");
            }

            return await GetByDoctorIdAsync(doctor.Id); // Sử dụng phương thức đã có để lấy đơn thuốc theo DoctorId
        }

        public async Task<IEnumerable<PrescriptionResponseDTO>> GetAllAsync()
        {
            var list = await _context.Prescriptions.ToListAsync();
            return list.Select(p => _mapper.MapToResponse(p));
        }

        public async Task<PrescriptionResponseDTO> GetByIdAsync(int id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Doctor)  // Đảm bảo lấy thông tin bác sĩ
                .Include(p => p.Patient)  // Đảm bảo lấy thông tin bệnh nhân
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null) return null;

            var response = _mapper.MapToResponse(prescription);
            response.PatientName = prescription.Patient?.Name ?? "Unknown Patient";
            response.PatientCCCD = prescription.Patient?.CCCD ?? "Unknown CCCD";
            response.DoctorName = prescription.Doctor?.Name ?? "Unknown Doctor";

            return response;
        }




        public async Task<PrescriptionResponseDTO> CreateAsync(PrescriptionRequest request)
        {
            // Tìm bác sĩ từ UserId
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == request.UserId);
            if (doctor == null)
                throw new Exception("Không tìm thấy bác sĩ.");

            // Tìm bệnh nhân từ tên và CCCD
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Name == request.PatientName && p.CCCD == request.PatientCCCD);

            // Kiểm tra xem có bệnh nhân với tên và CCCD khớp hay không
            if (patient == null)
                throw new Exception("Không tìm thấy bệnh nhân với thông tin cung cấp.");

            // Tạo đơn thuốc mới
            var entity = new Prescription
            {
                Note = request.Note,
                Status = (PrescriptionStatus)request.Status,
                PatientId = patient.Id,  // Gán PatientId từ bệnh nhân
                DoctorId = doctor.Id,    // Gán DoctorId từ bác sĩ
                Name = request.Name,
                Code = request.Code,
                CreateDate = DateTime.UtcNow,  // Lấy thời gian hiện tại
                UpdateDate = DateTime.UtcNow,  // Lấy thời gian hiện tại
                CreateBy = doctor.Name,
                UpdateBy = doctor.Name
            };

            // Thêm đơn thuốc vào cơ sở dữ liệu
            _context.Prescriptions.Add(entity);
            await _context.SaveChangesAsync();

            // Ánh xạ entity Prescription thành DTO để trả về
            var response = _mapper.MapToResponse(entity);
            response.PatientName = patient.Name;
            response.PatientCCCD = patient.CCCD;
            response.DoctorName = doctor.Name;

            return response;  // Trả về DTO với thông tin đơn thuốc đã tạo
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

        public async Task<IEnumerable<PrescriptionResponseDTO>> GetByDoctorIdAsync(int doctorId)
        {
            var prescriptions = await _context.Prescriptions
                .Where(p => p.DoctorId == doctorId)
                .ToListAsync();
            return prescriptions.Select(p => _mapper.MapToResponse(p));
        }

        public async Task<IEnumerable<PrescriptionResponseDTO>> GetByPatientIdAsync(int patientId)
        {
            var prescriptions = await _context.Prescriptions
                .Where(p => p.PatientId == patientId)
                .ToListAsync();
            return prescriptions.Select(p => _mapper.MapToResponse(p));
        }

        public async Task<IEnumerable<PrescriptionResponseDTO>> GetMineAsync(int userId, string role)
        {
            IQueryable<Prescription> prescriptionsQuery;

            if (role == "Doctor")
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctor == null) throw new Exception("Không tìm thấy bác sĩ.");

                prescriptionsQuery = _context.Prescriptions
                    .Where(p => p.DoctorId == doctor.Id); // Lọc theo DoctorId
            }
            else if (role == "Patient")
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (patient == null) throw new Exception("Không tìm thấy bệnh nhân.");

                prescriptionsQuery = _context.Prescriptions
                    .Where(p => p.PatientId == patient.Id); // Lọc theo PatientId
            }
            else
            {
                throw new Exception("Role không hợp lệ");
            }

            // Thực hiện join để lấy thông tin bác sĩ và bệnh nhân cùng lúc
            var prescriptions = await prescriptionsQuery
                .Join(_context.Doctors, p => p.DoctorId, d => d.Id, (p, d) => new { p, d }) // Join Prescriptions với Doctors
                .Join(_context.Patients, pd => pd.p.PatientId, pt => pt.Id, (pd, pt) => new PrescriptionResponseDTO
                {
                    Id = pd.p.Id,
                    Note = pd.p.Note,
                    Status = (PrescriptionStatus)pd.p.Status,
                    PatientName = pt.Name,  // Lấy tên bệnh nhân từ pt
                    PatientCCCD = pt.CCCD, // Lấy CCCD từ pt
                    PatientId = pd.p.PatientId,
                    DoctorId = pd.p.DoctorId,
                    DoctorName = pt.Name,  // Lấy tên bác sĩ từ d
                    Name = pd.p.Name,
                    Code = pd.p.Code,
                    CreateDate = pd.p.CreateDate,
                    UpdateDate = pd.p.UpdateDate ?? DateTime.UtcNow,
                    CreateBy = pd.p.CreateBy ?? "system",
                    UpdateBy = pd.p.UpdateBy ?? "system"
                }).ToListAsync(); // Chuyển thành danh sách

            return prescriptions;
        }







        public async Task<PrescriptionResponseDTO> UpdateStatusAsync(int prescriptionId, PrescriptionStatus newStatus, string updatedBy)
        {
            var prescription = await _context.Prescriptions
                .FirstOrDefaultAsync(p => p.Id == prescriptionId);
            if (prescription == null)
                return null;

            prescription.Status = newStatus;
            prescription.UpdateDate = DateTime.UtcNow;
            prescription.UpdateBy = updatedBy;

            var details = await _context.PrescriptionDetails
                .Where(d => d.PrescriptionId == prescriptionId)
                .ToListAsync();

            foreach (var d in details)
            {
                d.Status = (PrescriptionDetailStatus)newStatus;
                d.UpdateDate = DateTime.UtcNow;
                d.UpdateBy = updatedBy;
            }

            await _context.SaveChangesAsync();

            return _mapper.MapToResponse(prescription);
        }
    }
}
