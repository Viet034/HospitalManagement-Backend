using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Mapper;
using System.Collections.Generic;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class PatientFilterMapper : IPatientFilterMapper
    {
        public PatientFilterResponse EntityToResponse(Appointment appointment, int doctorId = 0, string doctorName = "")
        {
            if (appointment == null)
                return new PatientFilterResponse();

            return new PatientFilterResponse
            {
                AppointmentId = appointment.Id,
                DoctorId = doctorId,
                DoctorName = doctorName,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient?.Name ?? string.Empty,
                AppointmentDate = appointment.AppointmentDate,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                AppointmentStatus = appointment.Status.ToString(),
                Note = appointment.Note
            };
        }

        public List<PatientFilterResponse> ListEntityToResponse(List<Appointment> appointments, int doctorId = 0)
        {
            var result = new List<PatientFilterResponse>();
            if (appointments == null || appointments.Count == 0)
                return result;

            foreach (var appointment in appointments)
            {
                result.Add(EntityToResponse(appointment, doctorId));
            }
            return result;
        }
    }
}