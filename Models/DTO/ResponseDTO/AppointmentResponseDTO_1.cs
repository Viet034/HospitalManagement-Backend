using static SWP391_SE1914_ManageHospital.Ultility.Status;

public class AppointmentResponseDTO_1
{
    public int Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Note { get; set; }
    public bool isSend { get; set; }

    public int PatientId { get; set; }
    public string PatientName { get; set; }

    public int ClinicId { get; set; }
    public string ClinicName { get; set; }

    public int? ReceptionId { get; set; }
    public string? ReceptionName { get; set; }

    public int? ServiceId { get; set; }
    public string? ServiceName { get; set; }

    public string Code { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string CreateBy { get; set; }
    public string? UpdateBy { get; set; }
}
