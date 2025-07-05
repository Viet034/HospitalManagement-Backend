using System.ComponentModel.DataAnnotations;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.Entities;

public class Appointment : BaseEntity
{

    public DateTime AppointmentDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    [EnumDataType(typeof(AppointmentStatus))]
    public AppointmentStatus Status { get; set; }
    public string? Note { get; set; }
    public bool isSend { get; set; }
    public int PatientId { get; set; }
    public int ClinicId { get; set; }
    public int ReceptionId { get; set; }
    public virtual Patient Patient { get; set; } = null!;
    public virtual Clinic Clinic { get; set; } = null!;
    public virtual Invoice Invoice { get; set; } = null!;
    public virtual Reception Reception { get; set; } = null!;
    public virtual Medical_Record Medical_Record { get; set; } = null!;
    public virtual ICollection<Doctor_Appointment> Doctor_Appointments { get; set; } = new List<Doctor_Appointment>();
    public virtual ICollection<Nurse_Appointment> Nurse_Appointments { get; set; } = new List<Nurse_Appointment>();
    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    
}
