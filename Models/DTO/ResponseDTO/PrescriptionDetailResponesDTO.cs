// Models/DTO/ResponseDTO/PrescriptionDetailResponseDTO.cs
namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class PrescriptionDetailResponseDTO
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
        public string Usage { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string? UpdateBy { get; set; }
    }
}
