using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.Entities;

public class UserProfile : BaseEntity
{
    public string Phone { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
} 