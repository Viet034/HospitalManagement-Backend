namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class DoctorPrescriptionTopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public float YearOfExperience { get; set; }
        public int PrescriptionCount { get; set; }
    }
}
