using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.AppointmentVer2
{
    public class AppointmentCreate
    {
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Note { get; set; } = "Không có ghi chú";
        public int PatientId { get; set; }
        public int ClinicId { get; set; }
        public int ServiceId { get; set; }
        public string? Name { get; set; }
        public int DoctorId { get; set; }
        public string ShiftType { get; set; }
        
    }
}
