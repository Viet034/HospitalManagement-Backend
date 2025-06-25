namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.ImportMedicineEX
{
    public class ImportMedicineExcel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Dosage { get; set; }
        public string Ingredients { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string BatchNumber { get; set; }

        
        public string SupplierName { get; set; }
        public string UnitName { get; set; }     
        public string MedicineCategoryName { get; set; } 
        public string CreateBy { get; set; }
    }
}
