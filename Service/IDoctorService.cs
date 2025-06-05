using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service;

public interface IDoctorService
{
    Task<IEnumerable<DoctorResponseDTO>> GetActiveDoctorsByClinicAsync(int clinicId);
    Task<IEnumerable<DoctorScheduleResponseDTO>> GetDoctorScheduleInClinicAsync(int doctorId, int clinicId, DateTime? fromDate = null, DateTime? toDate = null);
} 