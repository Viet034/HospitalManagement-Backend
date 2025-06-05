using SWP391_SE1914_ManageHospital.Models;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public interface IMedicineService
{
    Task<List<MedicineResponseDTO>> GetAllAsync();
    Task<MedicineResponseDTO?> GetByIdAsync(int id);
    Task<MedicineResponseDTO> CreateAsync(MedicineCreate request);
    Task<bool> UpdateAsync(int id, MedicineCreate request);
    Task<bool> DeleteAsync(int id);
}
