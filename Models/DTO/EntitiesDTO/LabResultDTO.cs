namespace SWP391_SE1914_ManageHospital.DTOs
{
    public class LabResultDto
    {
        public int LabRequestId { get; set; }
        public string Conclusion { get; set; }
        public IFormFile Attachment { get; set; } // Chỉ 1 file ảnh hoặc PDF
    }
}