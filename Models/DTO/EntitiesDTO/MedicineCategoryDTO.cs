using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;

public class MedicineCategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; }
    public MedicineCategoryStatus Status { get; set; }
    
}
