using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public class DoctorScheduleResponseDTO
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; }
    public int ClinicId { get; set; }
    public string ClinicName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public DoctorAppointmentStatus DoctorAppointmentStatus { get; set; }
} 