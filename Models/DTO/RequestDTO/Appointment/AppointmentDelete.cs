using System.ComponentModel.DataAnnotations;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment;

public class AppointmentDelete
{
    public string Name { get; set; }
    public string Code { get; set; }
    [EnumDataType(typeof(AppointmentStatus))]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Cancelled;
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; } = DateTime.Now;
    public string CreateBy { get; set; }
    public string UpdateBy { get; set; } = "System";
} 