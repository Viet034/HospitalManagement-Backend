// Models/DTO/RequestDTO/PrescriptionDetailRequest.cs
namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PrescriptionDetail
{
    public class PrescriptionDetailRequest
    {
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
        public string Usage { get; set; } = string.Empty;
        public int Status { get; set; }

        public int UserId { get; set; }  // Lấy UserId từ JWT token
    }
}
