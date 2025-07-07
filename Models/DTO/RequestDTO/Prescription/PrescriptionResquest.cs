using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Prescription
{
    public class PrescriptionRequest
    {
        public string Note { get; set; }
        public PrescriptionStatus Status { get; set; }
        public int PatientId { get; set; }
        public int UserId { get; set; }  // Lấy UserId từ JWT token
        public string Name { get; set; }
        public string Code { get; set; }
    }


}
