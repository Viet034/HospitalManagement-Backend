using System;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class PatientFilterResponse
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string AppointmentStatus { get; set; }
        public string? Note { get; set; }

        public PatientFilterResponse()
        {
            AppointmentId = 0;
            DoctorId = 0;
            DoctorName = string.Empty;
            PatientId = 0;
            PatientName = string.Empty;
            AppointmentDate = DateTime.MinValue;
            StartTime = TimeSpan.Zero;
            EndTime = null;
            AppointmentStatus = string.Empty;
            Note = null;
        }

        public PatientFilterResponse(int appointmentId, int doctorId, int patientId, string doctorName, string patientName, DateTime appointmentDate, TimeSpan startTime, TimeSpan? endTime, string appointmentStatus, string? note)
        {
            AppointmentId = appointmentId;
            DoctorId = doctorId;
            DoctorName = doctorName;
            PatientId = patientId;
            PatientName = patientName;
            AppointmentDate = appointmentDate;
            StartTime = startTime;
            EndTime = endTime;
            AppointmentStatus = appointmentStatus;
            Note = note;
        }
    }
}