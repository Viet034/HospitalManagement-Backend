using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IMedicineCategoryService
    {
        Task<List<MedicineCategoryResponse>> GetAllAsync();
        Task<MedicineCategoryResponse?> GetByIdAsync(int id);
        Task<MedicineCategoryResponse> CreateAsync(MedicineCategoryRequest request);
        Task<MedicineCategoryResponse?> UpdateAsync(int id, MedicineCategoryRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
