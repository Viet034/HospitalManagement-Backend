using System;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class MedicalRecordDetailResponse
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; }
        public string TestResults { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int? PrescriptionId { get; set; }
        public int? DiseaseId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string DiseaseName { get; set; }

        public MedicalRecordDetailResponse(int id, string diagnosis, string testResults, string? notes, string status, int appointmentId,
                                        int patientId, int doctorId, int? prescriptionId, int? diseaseId, string name, string code,
                                        DateTime createDate, DateTime? updateDate, string createBy, string? updateBy, string doctorName,
                                        string patientName, string diseaseName)
        {
            Id = id;
            Diagnosis = diagnosis;
            TestResults = testResults;
            Notes = notes;
            Status = status;
            AppointmentId = appointmentId;
            PatientId = patientId;
            DoctorId = doctorId;
            PrescriptionId = prescriptionId;
            DiseaseId = diseaseId;
            Name = name;
            Code = code;
            CreateDate = createDate;
            UpdateDate = updateDate;
            CreateBy = createBy;
            UpdateBy = updateBy;
            DoctorName = doctorName;
            PatientName = patientName;
            DiseaseName = diseaseName;
        }
    }
}