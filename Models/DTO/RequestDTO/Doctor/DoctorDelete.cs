using System.ComponentModel.DataAnnotations;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO
{
    public class DoctorDelete
    {
        [Required]
        public int Id { get; set; }
    }
}