using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentResponseDTO>> GetAllAppointmentsAsync();
    Task<IEnumerable<AppointmentResponseDTO>> GetAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentResponseDTO>> SearchAppointmentsByNameAsync(string name);
    Task<AppointmentResponseDTO> GetAppointmentByIdAsync(int id);
    Task<AppointmentResponseDTO> CreateAppointmentAsync(AppointmentCreate create);
    Task<AppointmentResponseDTO> UpdateAppointmentAsync(int id, AppointmentUpdate update);
    Task<AppointmentResponseDTO> ChangeAppointmentStatusAsync(int id, AppointmentStatus status);
    Task<bool> DeleteAppointmentAsync(int id);
    Task<string> GenerateUniqueCodeAsync();
} 