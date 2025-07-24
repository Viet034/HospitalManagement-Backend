using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public class InvoiceResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string CreateBy { get; set; }
    public string? UpdateBy { get; set; }
    public decimal? InitialAmount { get; set; } // Giá trị ban đầu, cho phép null
    public decimal? DiscountAmount { get; set; } // Giá trị giảm giá, cho phép null
    public decimal TotalAmount { get; set; } // Giá trị cuối cùng
    public string? Notes { get; set; }
    public InvoiceStatus Status { get; set; }
    public int AppointmentId { get; set; }
    public int? InsuranceId { get; set; } // Cho phép null
    public int PatientId { get; set; }
}
