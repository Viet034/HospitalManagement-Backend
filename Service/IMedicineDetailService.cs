using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IMedicineDetailService
    {
        Task<List<MedicineDetailResponseDTO>> GetAllAsync();  // Lấy tất cả MedicineDetail
        Task<MedicineDetailResponseDTO?> GetByIdAsync(int id);  // Lấy thông tin MedicineDetail theo ID
        Task<MedicineDetailResponseDTO> CreateAsync(MedicineDetailRequest request);  // Tạo mới MedicineDetail
        Task<MedicineDetailResponseDTO?> UpdateAsync(int id, MedicineDetailRequest request);  // Cập nhật MedicineDetail theo ID
        Task<bool> DeleteAsync(int id);  // Xóa MedicineDetail theo ID
    }
}
