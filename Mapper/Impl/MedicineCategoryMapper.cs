using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Mapper;

namespace SWP391_SE1914_ManageHospital.Service
{
    public class MedicineCategoryMapper : IMedicineCategoryMapper
    {
        public MedicineCategoryResponseDTO MapToDTO(MedicineCategory m)
        {
            return new MedicineCategoryResponseDTO
            {
                Id = m.Id,
                Name = m.Name,
                
                Description = m.Description,
                ImageURL = m.ImageURL,
                Status = m.Status,
                
            };
        }
    }
}
