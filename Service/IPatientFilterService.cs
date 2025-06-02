using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public interface IPatientFilterService
{
    Task<IEnumerable<PatientFilterResponse>> GetPatientsByDoctorAsync(PatientFilter filter);
}