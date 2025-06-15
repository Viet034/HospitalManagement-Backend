using System.ComponentModel.DataAnnotations;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine
{
    public class MedicineCreate
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Dosage { get; set; }
        public int UnitId { get; set; }
        public int MedicineCategoryId { get; set; }

        [EnumDataType(typeof(MedicineStatus))]
        public MedicineStatus Status { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string? UpdateBy { get; set; }
    }
}
