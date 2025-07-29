using System;
namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter
{
    public class PatientFilter
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan StartTime { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public PatientFilter()
        {
        }
        public PatientFilter(int appointmentId, int doctorId, int patientId, string patientName, DateTime appointmentDate, TimeSpan startTime)
        {
            AppointmentId = appointmentId;
            DoctorId = doctorId;
            PatientId = patientId;
            PatientName = patientName;
            AppointmentDate = appointmentDate;
            StartTime = startTime;
        }
    }
}