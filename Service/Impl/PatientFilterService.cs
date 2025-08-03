using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWP391_SE1914_ManageHospital.Service;
using Microsoft.Extensions.Logging;
using SWP391_SE1914_ManageHospital.Ultility;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class PatientFilterService : IPatientFilterService
    {
        private readonly ApplicationDBContext _context;
        private readonly IPatientFilterMapper _mapper;
        private readonly ILogger<PatientFilterService> _logger;

        public PatientFilterService(ApplicationDBContext context, IPatientFilterMapper mapper, ILogger<PatientFilterService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<PatientFilterResponse>> GetTodayScheduleByDoctorAsync(int doctorId)
        {
            // Validation: DoctorId phải hợp lệ
            if (doctorId <= 0)
                throw new ArgumentException("ID bác sĩ không hợp lệ", nameof(doctorId));

            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == doctorId);
            if (!doctorExists)
                throw new ArgumentException("Bác sĩ không tồn tại", nameof(doctorId));

            var today = DateTime.UtcNow.AddHours(7).Date;
            var validStatuses = new[] {
                Status.AppointmentStatus.Scheduled,
                Status.AppointmentStatus.Completed
            };

            var doctorAppointments = await _context.Doctor_Appointments
                .Where(da => da.DoctorId == doctorId)
                .Include(da => da.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(da => da.Doctor)
                .ToListAsync();

            var appointments = doctorAppointments
                .Where(da => da.Appointment.AppointmentDate.Date == today
                    && validStatuses.Contains(da.Appointment.Status))
                .OrderBy(da => da.Appointment.StartTime)
                .Select(da => _mapper.EntityToResponse(
                    da.Appointment,
                    da.DoctorId,
                    da.Doctor.Name
                ))
                .ToList();

            return appointments;
        }

        public async Task<List<PatientFilterResponse>> FilterScheduleAsync(PatientFilter filter)
        {
            // Validation: filter không được null
            if (filter == null)
                throw new ArgumentNullException(nameof(filter), "Bộ lọc không được để trống.");

            // Validation: kiểm tra ngày tháng
            if (filter.FromDate.HasValue && filter.ToDate.HasValue && filter.FromDate > filter.ToDate)
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");

            // Validation: kiểm tra DoctorId nếu có
            if (filter.DoctorId < 0)
                throw new ArgumentException("ID bác sĩ không hợp lệ", nameof(filter.DoctorId));
            if (filter.DoctorId > 0)
            {
                var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == filter.DoctorId);
                if (!doctorExists)
                    throw new ArgumentException("Bác sĩ không tồn tại", nameof(filter.DoctorId));
            }

            // Validation: kiểm tra PatientId nếu có
            if (filter.PatientId < 0)
                throw new ArgumentException("ID bệnh nhân không hợp lệ", nameof(filter.PatientId));
            if (filter.PatientId > 0)
            {
                var patientExists = await _context.Patients.AnyAsync(p => p.Id == filter.PatientId);
                if (!patientExists)
                    throw new ArgumentException("Bệnh nhân không tồn tại", nameof(filter.PatientId));
            }

            // Validation: kiểm tra AppointmentId nếu có
            if (filter.AppointmentId < 0)
                throw new ArgumentException("ID lịch hẹn không hợp lệ", nameof(filter.AppointmentId));
            if (filter.AppointmentId > 0)
            {
                var appointmentExists = await _context.Appointments.AnyAsync(a => a.Id == filter.AppointmentId);
                if (!appointmentExists)
                    throw new ArgumentException("Lịch hẹn không tồn tại", nameof(filter.AppointmentId));
            }

            List<PatientFilterResponse> result;
            var validStatuses = new[] {
                Status.AppointmentStatus.Scheduled,
                Status.AppointmentStatus.Completed
            };

            if (filter.DoctorId > 0)
            {
                var doctorAppointments = await _context.Doctor_Appointments
                    .Where(da => da.DoctorId == filter.DoctorId)
                    .Include(da => da.Appointment)
                        .ThenInclude(a => a.Patient)
                    .Include(da => da.Doctor)
                    .ToListAsync();

                var filtered = doctorAppointments
                    .Where(da => validStatuses.Contains(da.Appointment.Status))
                    .AsQueryable();

                if (filter.AppointmentId > 0)
                    filtered = filtered.Where(da => da.AppointmentId == filter.AppointmentId);

                if (filter.PatientId > 0)
                    filtered = filtered.Where(da => da.Appointment.PatientId == filter.PatientId);

                if (!string.IsNullOrWhiteSpace(filter.PatientName))
                    filtered = filtered.Where(da => da.Appointment.Patient != null && da.Appointment.Patient.Name.Contains(filter.PatientName));

                if (filter.FromDate.HasValue)
                    filtered = filtered.Where(da => da.Appointment.AppointmentDate.Date >= filter.FromDate.Value.Date);

                if (filter.ToDate.HasValue)
                    filtered = filtered.Where(da => da.Appointment.AppointmentDate.Date <= filter.ToDate.Value.Date);

                result = filtered
                    .OrderBy(da => da.Appointment.AppointmentDate)
                    .ThenBy(da => da.Appointment.StartTime)
                    .Select(da => _mapper.EntityToResponse(
                        da.Appointment,
                        da.DoctorId,
                        da.Doctor.Name
                    ))
                    .ToList();
            }
            else
            {
                var query = _context.Appointments
                    .Where(a => validStatuses.Contains(a.Status))
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor_Appointments)
                        .ThenInclude(da => da.Doctor)
                    .AsQueryable();

                if (filter.AppointmentId > 0)
                    query = query.Where(a => a.Id == filter.AppointmentId);

                if (filter.PatientId > 0)
                    query = query.Where(a => a.PatientId == filter.PatientId);

                if (!string.IsNullOrWhiteSpace(filter.PatientName))
                    query = query.Where(a => a.Patient != null && a.Patient.Name.Contains(filter.PatientName));

                if (filter.FromDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date >= filter.FromDate.Value.Date);

                if (filter.ToDate.HasValue)
                    query = query.Where(a => a.AppointmentDate.Date <= filter.ToDate.Value.Date);

                var appointments = await query
                    .OrderBy(a => a.AppointmentDate)
                    .ThenBy(a => a.StartTime)
                    .ToListAsync();

                result = appointments.Select(a =>
                {
                    var doctorAppointment = a.Doctor_Appointments.FirstOrDefault();
                    var doctorId = doctorAppointment?.DoctorId ?? 0;
                    var doctorName = doctorAppointment?.Doctor?.Name ?? doctorAppointment?.Doctor?.LicenseNumber ?? "Unknown";
                    return _mapper.EntityToResponse(a, doctorId, doctorName);
                }).ToList();
            }

            return result;
        }

        public async Task<List<PatientFilterResponse>> GetAllSchedulesAsync()
        {
            var validStatuses = new[] {
                Status.AppointmentStatus.Scheduled,
                Status.AppointmentStatus.Completed
            };

            var doctorAppointments = await _context.Doctor_Appointments
                .Include(da => da.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(da => da.Doctor)
                .ToListAsync();

            var result = doctorAppointments
                .Where(da => validStatuses.Contains(da.Appointment.Status))
                .OrderBy(da => da.Appointment.AppointmentDate)
                .ThenBy(da => da.Appointment.StartTime)
                .Select(da => _mapper.EntityToResponse(
                    da.Appointment,
                    da.DoctorId,
                    da.Doctor.Name
                ))
                .ToList();

            return result;
        }
    }
}