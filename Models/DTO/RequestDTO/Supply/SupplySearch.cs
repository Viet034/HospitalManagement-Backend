using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supply
{
    public class SupplySearch
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public SupplyStatus? Status { get; set; }
        public DateTime? FromCreateDate { get; set; }
        public DateTime? ToCreateDate { get; set; }
    }
}
