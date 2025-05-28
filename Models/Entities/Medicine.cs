using System.ComponentModel;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.Entities;

public class Medicine : BaseEntity
{
    public MedicineStatus Status { get; set; }
    public string Description { get; set; }
    public string Unit { get; set; }
    public string Dosage { get; set; }
    public int MedicineCategoryId { get; set; }
    public virtual MedicineCategory MedicineCategory { get; set; }
    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
    public virtual ICollection<Medicine_Inventory> Medicine_Inventories { get; set; } = new List<Medicine_Inventory>();
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    
}
