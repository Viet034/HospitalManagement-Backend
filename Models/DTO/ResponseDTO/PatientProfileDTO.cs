using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public class PatientProfileDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Code { get; set; }
    public string Email { get; set; }
    public string Contact { get; set; }
    public Gender Gender { get; set; }
    public PatientStatus Status { get; set; }
    public DateTime CreateDate { get; set; }
    public bool HasAccount { get; set; }
} 