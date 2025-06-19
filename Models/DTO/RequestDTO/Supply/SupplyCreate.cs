using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supply
{
    public class SupplyCreate
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public SupplyStatus Status { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public int AppointmentId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }

        public SupplyCreate() { }

        public SupplyCreate(string name, string code, SupplyStatus status, string description, string unit,
                            int appointmentId, DateTime createDate, string createBy)
        {
            Name = name;
            Code = code;
            Status = status;
            Description = description;
            Unit = unit;
            AppointmentId = appointmentId;
            CreateDate = createDate;
            CreateBy = createBy;
        }
    }
}
