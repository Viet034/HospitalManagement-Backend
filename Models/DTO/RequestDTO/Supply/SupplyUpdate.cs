using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supply
{
    public class SupplyUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public SupplyStatus Status { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public int AppointmentId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public SupplyUpdate() { }

        public SupplyUpdate(int id, string name, string code, SupplyStatus status, string description,
                            string unit, int appointmentId, DateTime? updateDate, string? updateBy)
        {
            Id = id;
            Name = name;
            Code = code;
            Status = status;
            Description = description;
            Unit = unit;
            AppointmentId = appointmentId;
            UpdateDate = updateDate;
            UpdateBy = updateBy;
        }
    }
}
