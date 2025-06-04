using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Patient;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service;

public interface IPatientRegistrationService
{
    Task<PatientProfileDTO> RegisterNewPatientAsync(PatientRegistrationDTO request);
    Task<PatientProfileDTO> RegisterExistingUserAsPatientAsync(PatientRegistrationDTO request, string userId);
    Task<bool> IsEmailRegisteredAsync(string email);
    Task<bool> IsContactRegisteredAsync(string contact);
    Task<PatientProfileDTO?> GetPatientProfileByUserIdAsync(string userId);
    Task<bool> HasPatientProfileAsync(string userId);
} 