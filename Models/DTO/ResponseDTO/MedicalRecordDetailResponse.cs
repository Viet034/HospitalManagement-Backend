namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class MedicalRecordDetailResponse
    {
        // Lấy đầy đủ thông tin chi tiết
        public int Id { get; set; }
        public string Diagnosis { get; set; } = default!;
        public string TestResults { get; set; } = default!;
        public string? Notes { get; set; }
        public string Status { get; set; } = default!;
        public DateTime CreateDate { get; set; }
        public string DoctorName { get; set; } = default!;
        public string PatientName { get; set; } = default!;
        public string DiseaseName { get; set; } = default!;
        public int AppointmentId { get; set; }
        public int PrescriptionId { get; set; }

        public MedicalRecordDetailResponse()
        {
        }

        public MedicalRecordDetailResponse(
            int id,
            string diagnosis,
            string testResults,
            string? notes,
            string status,
            DateTime createDate,
            string doctorName,
            string patientName,
            string diseaseName,
            int appointmentId,
            int prescriptionId)
        {
            Id = id;
            Diagnosis = diagnosis;
            TestResults = testResults;
            Notes = notes;
            Status = status;
            CreateDate = createDate;
            DoctorName = doctorName;
            PatientName = patientName;
            DiseaseName = diseaseName;
            AppointmentId = appointmentId;
            PrescriptionId = prescriptionId;
        }
    }
}
