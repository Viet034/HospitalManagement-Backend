namespace SWP391_SE1914_ManageHospital.Models.Entities
{
    public class Doctor_Shift : BaseEntity
    {
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; } = null!;

        public int DayOfWeek { get; set; }  // 1=Monday, 2=Tuesday, 3=Wednesday, 4=Thursday, 5=Friday, 6=Saturday
        public string ShiftType { get; set; } = null!;  // "Morning" or "Afternoon"

        public TimeSpan StartTime { get; set; }              
        public TimeSpan EndTime { get; set; }               
        public bool IsActive { get; set; } = true;
              
        public string? Notes { get; set; }
    }
}
