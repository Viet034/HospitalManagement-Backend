public class MedicalRecordAdminResponse
{
    public int Id { get; set; }
    public int Status { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string TestResults { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int PrescriptionId { get; set; }
    public int DiseaseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public string UpdateBy { get; set; } = string.Empty;

    // Thêm trường thông tin mở rộng từ liên kết (nếu cần)
    public string? DoctorName { get; set; }
    public string? PatientName { get; set; }
    public string? DiseaseName { get; set; }
    public string? AppointmentName { get; set; }
}
