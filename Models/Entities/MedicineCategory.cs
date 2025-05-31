using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.Entities;

public class MedicineCategory : BaseEntity
{
    
    public string Description { get; set; }
    public MedicineCategoryStatus Status { get; set; }
    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
