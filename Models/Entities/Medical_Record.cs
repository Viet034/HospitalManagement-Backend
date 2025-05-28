using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.Entities;

public class Medical_Record : BaseEntity
{
    public MedicalRecordStatus Status { get; set; }
    public string Diagnosis { get; set; }
    public string TestResults { get; set; }
    public string? Notes { get; set; }
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int PrescriptionId { get; set; }
    public int DiseaseId { get; set; }
    public virtual Patient Patient { get; set; } = null!;
    public virtual Doctor Doctor { get; set; } = null!;
    public virtual Prescription Prescription { get; set; } = null!;
    public virtual Disease Disease { get; set; } = null!;
    public virtual Appointment Appointment { get; set; } = null!;
}
