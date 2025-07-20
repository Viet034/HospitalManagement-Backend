using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_SE1914_ManageHospital.Models.Entities
{
    public enum ShiftRequestType
    {
        ChangeShift = 0,
        DayOff = 1
    }

    public enum ShiftRequestStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public class Shift_Request
    {
        [Key]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int ShiftId { get; set; }
        public ShiftRequestType RequestType { get; set; }
        [Required]
        public string Reason { get; set; } = null!;
        public ShiftRequestStatus Status { get; set; } = ShiftRequestStatus.Pending;
        public DateTime CreatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public virtual Doctor Doctor { get; set; } = null!;
        public virtual Doctor_Shift Doctor_Shift { get; set; } = null!;
    }
}