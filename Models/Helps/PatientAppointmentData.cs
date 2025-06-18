using System;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Models.Helps
{
    /// hỗ trợ truyền dữ liệu giữa service và mapper
    public class PatientAppointmentData
    {
        public Patient Patient { get; set; }
        public DateTime ExaminationTime { get; set; }
        public string AppointmentStatus { get; set; }

        public PatientAppointmentData()
        {
            this.Patient = null;
            this.ExaminationTime = DateTime.MinValue;
            this.AppointmentStatus = string.Empty;
        }
    }
}