namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class MedicalRecordHistoryDTO
    {
        public string AppointmentCode { get; set; }
        public string Diagnosis { get; set; }
        public string TestResults { get; set; }
        public string DoctorName { get; set; }
        public string ClinicName { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

    public class PrescriptionDTO
    {
        public string PrescriptionCode { get; set; }
        public DateTime CreateDate { get; set; }
        public string DoctorName { get; set; }
        public List<MedicineDetailDTO> Medicines { get; set; }
    }

    public class MedicineDetailDTO
    {
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public string Usage { get; set; }
    }
}
