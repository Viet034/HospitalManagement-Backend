namespace SWP391_SE1914_ManageHospital.Models.Entities
{
    public class LaboratoryStaff
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public DateTime CreatedAt { get; set; }
    }


}
