using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper;
 
public interface IDoctorScheduleMapper
{
    DoctorScheduleResponseDTO MapToResponse(Doctor_Appointment doctorAppointment);
    IEnumerable<DoctorScheduleResponseDTO> MapToResponseList(IEnumerable<Doctor_Appointment> doctorAppointments);
} 