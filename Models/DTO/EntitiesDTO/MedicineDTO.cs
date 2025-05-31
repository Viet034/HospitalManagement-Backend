using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;

public class MedicineDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public MedicineStatus Status { get; set; }
    public string Description { get; set; }
    public string Unit { get; set; }
    public string Dosage { get; set; }
    public int MedicineCategoryId { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string CreateBy { get; set; }
    public string? UpdateBy { get; set; }
}
