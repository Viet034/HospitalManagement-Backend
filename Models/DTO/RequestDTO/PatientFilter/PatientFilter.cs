namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter
{
    public class PatientFilter
    {
        public int DoctorId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? PatientName { get; set; }
    }
}
