namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineDetail
{
    public class MedicineDetailRequest
    {
        public int MedicineId { get; set; }
        public string Ingredients { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Manufacturer { get; set; }
        public string Warning { get; set; }
        public string StorageInstructions { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string Description { get; set; }
    }
}
