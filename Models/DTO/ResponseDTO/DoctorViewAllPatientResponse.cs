using System;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class DoctorViewAllPatientResponse
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = default!;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public string Status { get; set; } = default!;
        public DateTime Dob { get; set; }
        public string Address { get; set; } = default!;

    }
}