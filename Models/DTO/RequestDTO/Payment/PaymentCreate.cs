namespace Models.DTO.RequestDTO.Payment;

public class PaymentCreate
{
    public int InvoiceId { get; set; }

    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Payer { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime PaymentDate { get; set; }

    public string Name { get; set; }
    public string Code { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string CreateBy { get; set; }
    public string? UpdateBy { get; set; }
}
