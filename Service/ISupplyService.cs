using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supply;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Services.Interface
{
    public interface ISupplyService
    {
        Task<List<SupplyResponseDTO>> GetAllAsync();
        Task<SupplyResponseDTO?> GetByIdAsync(int id);
        Task<SupplyResponseDTO> CreateAsync(SupplyCreate dto);
        Task<bool> UpdateAsync(SupplyUpdate dto);
        Task<bool> DeleteAsync(int id);
        Task<List<SupplyResponseDTO>> SearchAsync(SupplySearch searchDto);
    }
}
