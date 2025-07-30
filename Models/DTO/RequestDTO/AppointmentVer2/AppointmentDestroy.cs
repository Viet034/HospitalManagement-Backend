using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.AppointmentVer2
{
    public class AppointmentDestroy
    {
        public DateTime AppointmentDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Note { get; set; }
        public int PatientId { get; set; }
        public int ClinicId { get; set; }
        public int ServiceId { get; set; }
        public int? ReceptionId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int DoctorId { get; set; }
        public bool isSend { get; set; }
    }
}
