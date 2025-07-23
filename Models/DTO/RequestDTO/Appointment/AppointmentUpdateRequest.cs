using System;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment;

public class AppointmentUpdateRequest
{
    public DateTime? AppointmentDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
} 