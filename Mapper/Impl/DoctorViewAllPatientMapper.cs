using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using System.Collections.Generic;
using System.Linq;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class DoctorViewAllPatientMapper : IDoctorViewAllPatientMapper
    {
        public DoctorViewAllPatientResponse EntityToDoctorViewAllPatientResponse(Appointment entity)
        {
            var doctorAppointment = entity.Doctor_Appointments?.FirstOrDefault();
            var doctor = doctorAppointment?.Doctor;
            var patient = entity.Patient;

            return new DoctorViewAllPatientResponse
            {
                DoctorId = doctor?.Id ?? 0,
                DoctorName = doctor?.Name ?? "",
                PatientId = patient?.Id ?? 0,
                PatientName = patient?.Name ?? "",
                Phone = patient?.Phone ?? "",
                AppointmentDate = entity.AppointmentDate,
                StartTime = entity.StartTime,
                Status = entity.Status.ToString(),
                Dob = patient?.Dob ?? default,
                Address = patient?.Address ?? ""
            };
        }

        public IEnumerable<DoctorViewAllPatientResponse> ListEntityToDoctorViewAllPatientResponse(IEnumerable<Appointment> entities)
        {
            return entities.Select(EntityToDoctorViewAllPatientResponse);
        }
    }
}