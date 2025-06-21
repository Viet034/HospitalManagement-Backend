using System.ComponentModel.DataAnnotations;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment;

public class AppointmentCreate
{
    public string Name { get; set; }
    public string? Code { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    [EnumDataType(typeof(AppointmentStatus))]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string? Note { get; set; }
    public int PatientId { get; set; }
    public int ClinicId { get; set; }
    public int ReceptionId { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public string CreateBy { get; set; } = "System";
} 