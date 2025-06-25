namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class MedicineResponse
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;

        public int UnitId { get; set; }

        public int UnitId { get; set; };
        public MedicineStatus Status { get; set; }

        public int MedicineCategoryId { get; set; }
        public string Prescribed { get; set; } = string.Empty;


        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
    }

}
