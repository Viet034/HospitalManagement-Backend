using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.Entities;

public class Medicine_Inventory 
{
    public int Id;
    public int Quantity { get; set; }
    public int BatchNumber { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime ImportDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public MedicineInventoryStatus Status { get; set; }
    public int MedicineId { get; set; }
    public virtual Medicine Medicine { get; set; }
    
}
