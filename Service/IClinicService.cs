using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Clinic;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Ultility;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service;

public interface IClinicService
{
    public Task<IEnumerable<ClinicResponse>> GetAllClinicAsync();
    public Task<IEnumerable<ClinicResponse>> SearchClinicByKeyAsync(string key);
    public Task<ClinicResponse> UpdateClinicAsync(int id, ClinicUpdate update);
    public Task<ClinicResponse> CreateClinicAsync(ClinicCreate create);
    public Task<ClinicResponse> SoftDeleteClinicColorAsync(int id, ClinicStatus newStatus);
    public Task<bool> HardDeleteClinicAsync(int id);
    public Task<ClinicResponse> FindClinicByIdAsync(int id);
    public Task<string> CheckUniqueCodeAsync();
}
