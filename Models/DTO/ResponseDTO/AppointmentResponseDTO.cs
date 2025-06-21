using System.ComponentModel.DataAnnotations;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public class AppointmentResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    [EnumDataType(typeof(AppointmentStatus))]
    public AppointmentStatus Status { get; set; }
    public string? Note { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string CreateBy { get; set; }
    public string? UpdateBy { get; set; }
    public int PatientId { get; set; }
    public int ClinicId { get; set; }
    public int ReceptionId { get; set; }
    
    // Thêm thông tin bệnh nhân để hiện thị trong UI
    public string? PatientName { get; set; }
    public string? PatientEmail { get; set; }
    public string? PatientImage { get; set; }
    public string? Type { get; set; } // New Patient hoặc Old Patient
} 