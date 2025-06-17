using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO
{
    public class MedicineCategoryCreate
    {
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public MedicineCategoryStatus Status { get; set; }
        public string CreateBy { get; set; } = string.Empty;
    }
}
