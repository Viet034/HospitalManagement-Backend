using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class DoctorViewAllPatientService : IDoctorViewAllPatientService
    {
        private readonly ApplicationDBContext _context;
        private readonly IDoctorViewAllPatientMapper _mapper;

        public DoctorViewAllPatientService(ApplicationDBContext context, IDoctorViewAllPatientMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<DoctorViewAllPatientResponse> GetAllPatientsByDoctorId(int doctorId)
        {
            if (doctorId <= 0)
                throw new ArgumentException("ID bác sĩ không hợp lệ", nameof(doctorId));

            var doctorExists = _context.Doctors.Any(d => d.Id == doctorId);
            if (!doctorExists)
                throw new ArgumentException($"Không tìm thấy bác sĩ với ID: {doctorId}", nameof(doctorId));

            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor_Appointments).ThenInclude(da => da.Doctor)
                .Where(a =>
                    a.Doctor_Appointments.Any(da => da.DoctorId == doctorId) &&
                    a.Status == SWP391_SE1914_ManageHospital.Ultility.Status.AppointmentStatus.Completed
                )
                .ToList();

            if (appointments.Count == 0)
                return Enumerable.Empty<DoctorViewAllPatientResponse>();

            return _mapper.ListEntityToDoctorViewAllPatientResponse(appointments);
        }
    }
}