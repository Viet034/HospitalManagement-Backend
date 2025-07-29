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

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class PatientFilterService : IPatientFilterService
    {
        private readonly ApplicationDBContext _context;
        private readonly IPatientFilterMapper _mapper;
        private readonly ILogger<PatientFilterService> _logger;

        public PatientFilterService(ApplicationDBContext context, IPatientFilterMapper mapper, ILogger<PatientFilterService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<PatientFilterResponse>> GetTodayScheduleByDoctorAsync(int doctorId)
        {
            if (doctorId <= 0)
                throw new ArgumentException("ID bác sĩ không hợp lệ", nameof(doctorId));

            var today = DateTime.UtcNow.AddHours(7).Date;

            var doctorAppointments = await _context.Doctor_Appointments
                .Where(da => da.DoctorId == doctorId)
                .Include(da => da.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(da => da.Doctor)
                .ToListAsync();

            var appointments = doctorAppointments
                .Where(da => da.Appointment.AppointmentDate.Date == today)
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
            if (filter == null)
                throw new ArgumentNullException(nameof(filter), "Bộ lọc không được để trống.");

            List<PatientFilterResponse> result;

            if (filter.DoctorId > 0)
            {
                var doctorAppointments = await _context.Doctor_Appointments
                    .Where(da => da.DoctorId == filter.DoctorId)
                    .Include(da => da.Appointment)
                        .ThenInclude(a => a.Patient)
                    .Include(da => da.Doctor)
                    .ToListAsync();

                var filtered = doctorAppointments.AsQueryable();

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
            var doctorAppointments = await _context.Doctor_Appointments
                .Include(da => da.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(da => da.Doctor)
                .OrderBy(da => da.Appointment.AppointmentDate)
                .ThenBy(da => da.Appointment.StartTime)
                .ToListAsync();

            var result = doctorAppointments.Select(da =>
                _mapper.EntityToResponse(
                    da.Appointment,
                    da.DoctorId,
                    da.Doctor.Name
                )
            ).ToList();

            return result;
        }
    }
}