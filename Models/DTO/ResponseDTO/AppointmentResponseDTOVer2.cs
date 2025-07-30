using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class AppointmentResponseDTOVer2
    {
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }           
        public AppointmentStatus Status { get; set; }
        public string? Note { get; set; }
        public int PatientId { get; set; }
        public int ClinicId { get; set; }             
        public int ServiceId { get; set; }           
        public int? ReceptionId { get; set; }         
        public string? Name { get; set; }            
        public string? Code { get; set; }             
        public int DoctorId { get; set; }                    
    }
}
