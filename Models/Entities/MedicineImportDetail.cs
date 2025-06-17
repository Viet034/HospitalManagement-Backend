namespace SWP391_SE1914_ManageHospital.Models.Entities
{
    public class MedicineImportDetail : BaseEntity
    {
        public int ImportId { get; set; }
        public virtual MedicineImport Import { get; set; }

        public int MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }

        public string BatchNumber { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }


    }
}
