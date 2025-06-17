using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IMedicineCategoryService
    {
        Task<List<MedicineCategoryResponseDTO>> GetAllAsync();
        Task<MedicineCategoryResponseDTO?> GetByIdAsync(int id);
        Task<MedicineCategoryResponseDTO> CreateAsync(MedicineCategoryCreate request);
        Task<bool> UpdateAsync(int id, MedicineCategoryCreate request);
        Task<bool> DeleteAsync(int id);
    }
}
