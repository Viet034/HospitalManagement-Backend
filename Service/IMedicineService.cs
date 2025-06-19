using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public interface IMedicineService
{
    Task<List<MedicineResponse>> GetAllAsync();
    Task<MedicineResponse?> GetByIdAsync(int id);
    Task<MedicineResponse> CreateAsync(MedicineRequest request);
    Task<MedicineResponse?> UpdateAsync(int id, MedicineRequest request);
    Task<bool> DeleteAsync(int id);
    Task<List<MedicineResponse>> GetByPrescribedAsync(int prescribed);

}
