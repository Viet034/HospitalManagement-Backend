using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.Entities;

public class Invoice : BaseEntity
{
    public decimal InitialAmount { get; set; } //Price ban dau
    public decimal DiscountAmount { get; set; } //Price duoc giam
    public decimal TotalAmount { get; set; } //Price cuoi cung
    public string? Notes { get; set; }
    public InvoiceStatus Status { get; set; }
    public int AppointmentId { get; set; }
    public int InsuranceId { get; set; }
    public int PatientId { get; set; }
    public virtual Appointment Appointment { get; set; } = null!;
    public virtual Patient Patient  { get; set; } = null!;
    public virtual Insurance Insurance { get; set; } = null!;
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    public virtual ICollection<Payment_Invoice> Payment_Invoices { get; set; } = new List<Payment_Invoice>();
}
