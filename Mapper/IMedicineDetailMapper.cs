using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface IMedicineDetailMapper
    {
        MedicineDetailResponseDTO MapToDTO(MedicineDetail entity);  // Chuyển đổi từ entity sang DTO
        MedicineDetail MapToEntity(MedicineDetailRequest request);  // Chuyển đổi từ DTO sang entity
        void MapToExistingEntity(MedicineDetailRequest request, MedicineDetail entity);  // Cập nhật entity từ DTO
    }
}
