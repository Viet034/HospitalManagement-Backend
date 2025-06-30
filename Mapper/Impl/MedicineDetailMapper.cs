using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public class MedicineDetailMapper : IMedicineDetailMapper
    {
        // Chuyển đổi từ entity MedicineDetail sang MedicineDetailResponseDTO
        public MedicineDetailResponseDTO MapToDTO(MedicineDetail entity)
        {
            return new MedicineDetailResponseDTO
            {
                Id = entity.Id,
                MedicineId = entity.MedicineId,
                Ingredients = entity.Ingredients,
                ExpiryDate = entity.ExpiryDate,
                Manufacturer = entity.Manufacturer,
                Warning = entity.Warning,
                StorageInstructions = entity.StorageInstructions,
                Status = entity.Status,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
                CreateBy = entity.CreateBy,
                UpdateBy = entity.UpdateBy,
                Description = entity.Description,
                MedicineName = entity.Medicine?.Name // Nếu có liên kết với bảng Medicine, lấy tên thuốc
            };
        }

        // Chuyển đổi từ MedicineDetailRequest sang entity MedicineDetail
        public MedicineDetail MapToEntity(MedicineDetailRequest request)
        {
            return new MedicineDetail
            {
                MedicineId = request.MedicineId,
                Ingredients = request.Ingredients,
                ExpiryDate = request.ExpiryDate,
                Manufacturer = request.Manufacturer,
                Warning = request.Warning,
                StorageInstructions = request.StorageInstructions,
                Status = request.Status,
                CreateBy = request.CreateBy,
                UpdateBy = request.UpdateBy,
                Description = request.Description,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
        }

        // Cập nhật entity MedicineDetail từ MedicineDetailRequest
        public void MapToExistingEntity(MedicineDetailRequest request, MedicineDetail entity)
        {
            entity.MedicineId = request.MedicineId;
            entity.Ingredients = request.Ingredients;
            entity.ExpiryDate = request.ExpiryDate;
            entity.Manufacturer = request.Manufacturer;
            entity.Warning = request.Warning;
            entity.StorageInstructions = request.StorageInstructions;
            entity.Status = request.Status;
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdateBy = request.UpdateBy;
            entity.Description = request.Description;
        }
    }
}
