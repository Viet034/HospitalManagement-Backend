namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.ImportMedicineEX;

public class MedicineImportDetailRequest
{
    public string MedicineCode { get; set; } = string.Empty;
    public string MedicineName { get; set; } = string.Empty;
    public string UnitName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = "Không xác định";
    public string BatchNumber { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime ManufactureDate { get; set; }
    public DateTime ExpiryDate { get; set; }
}