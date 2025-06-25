namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supplier
{
    public class SupplierCreate
    {
        public string Name { get; set; }

        public string Code { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string? UpdateBy { get; set; }

        public SupplierCreate() { }

        public SupplierCreate(string name, string phone, string email, string address, DateTime createDate, DateTime? updateDate, string createBy, string? updateBy)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Address = address;
            CreateDate = createDate;
            UpdateDate = updateDate;
            CreateBy = createBy;
            UpdateBy = updateBy;
        }
    }
}
