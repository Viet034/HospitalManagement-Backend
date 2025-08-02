namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public class InvocieDetailFinalResponseDTO
{
    public int InvoiceId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? PatientName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }

    public string? DoctorName { get; set; }
    public string? Diagnosis { get; set; }
    public string? Notes { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<ServiceItemDTO> ServiceItems { get; set; } = new();
}
public class ServiceItemDTO
{
    public string? Type { get; set; } // "Service" hoặc "Prescription"
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}
