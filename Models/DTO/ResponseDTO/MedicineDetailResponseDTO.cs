using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class MedicineDetailResponseDTO
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string Ingredients { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Warning { get; set; } = string.Empty;
        public string StorageInstructions { get; set; } = string.Empty;

        public int Status { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }

        public string Description { get; set; } = string.Empty;

        // Thông tin bổ sung nếu cần (tên của Medicine)
        public string MedicineName { get; set; } = string.Empty;
    }
}
