using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl;

public class DoctorScheduleMapper : IDoctorScheduleMapper
{
    public DoctorScheduleResponseDTO MapToResponse(Doctor_Appointment doctorAppointment)
    {
        return new DoctorScheduleResponseDTO
        {
            DoctorId = doctorAppointment.Doctor.Id,
            DoctorName = doctorAppointment.Doctor.Name,
            ClinicId = doctorAppointment.Appointment.Clinic.Id,
            ClinicName = doctorAppointment.Appointment.Clinic.Name,
            AppointmentDate = doctorAppointment.Appointment.AppointmentDate,
            StartTime = doctorAppointment.Appointment.StartTime,
            EndTime = doctorAppointment.Appointment.EndTime,
            Status = doctorAppointment.Appointment.Status,
            DoctorAppointmentStatus = doctorAppointment.Status
        };
    }

    public IEnumerable<DoctorScheduleResponseDTO> MapToResponseList(IEnumerable<Doctor_Appointment> doctorAppointments)
    {
        return doctorAppointments.Select(MapToResponse);
    }
} 