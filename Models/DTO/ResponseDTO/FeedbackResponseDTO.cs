namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class FeedbackResponseDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int? DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }

        public FeedbackResponseDTO() { }
        public FeedbackResponseDTO(int id, string content, DateTime createDate, int? doctorId, string doctorName, int patientId, string patientName)
        {
            Id = id;
            Content = content;
            CreateDate = createDate;
            DoctorId = doctorId;
            DoctorName = doctorName;
            PatientId = patientId;
            PatientName = patientName;
        }
    }
}
