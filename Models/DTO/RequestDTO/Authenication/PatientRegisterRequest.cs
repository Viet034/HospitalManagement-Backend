using System.ComponentModel.DataAnnotations;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Authenication;

public class PatientRegisterRequest
{
    // USER
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    // PATIENT
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Code { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime Dob { get; set; }

    [Required]
    [RegularExpression(@"\d{12}")]
    public string CCCD { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }

    [Required]
    public string EmergencyContact { get; set; }

    [Required]
    public string Address { get; set; }
}
