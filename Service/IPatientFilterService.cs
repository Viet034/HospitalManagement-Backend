using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IPatientFilterService
    {
        Task<List<PatientFilterResponse>> GetTodayScheduleByDoctorAsync(int doctorId);
        Task<List<PatientFilterResponse>> FilterScheduleAsync(PatientFilter filter);
        Task<List<PatientFilterResponse>> GetAllSchedulesAsync();
    }
}