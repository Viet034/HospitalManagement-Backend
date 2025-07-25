using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.Entities;

public class InvoiceDetail : BaseEntity
{
    public InvoiceDetailStatus Status { get; set; }
    public decimal? Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public int InvoiceId { get; set; }
    public int? PrescriptionsId { get; set; }
    public int? ServiceId { get; set; }
    public virtual Invoice Invoice { get; set; }
    public virtual Prescription Prescription { get; set; }
    public virtual Servicess? Service { get; set; }
}
