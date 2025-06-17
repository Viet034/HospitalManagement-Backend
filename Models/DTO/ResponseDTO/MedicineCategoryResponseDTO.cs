using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class MedicineCategoryResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        public string? ImageURL { get; set; }
        public MedicineCategoryStatus Status { get; set; }

        
    }
}
