using SWP391_SE1914_ManageHospital.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class DoctorResponseDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        public string CCCD { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? ImageURL { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        [Required]
        public float YearOfExperience { get; set; }

        [Required]
        public float WorkingHours { get; set; }

        [Required]
        [EnumDataType(typeof(DoctorStatus))]
        public DoctorStatus Status { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [Required]
        public string CreateBy { get; set; }

        public string? UpdateBy { get; set; }
    }
}