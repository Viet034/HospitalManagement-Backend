namespace SWP391_SE1914_ManageHospital.Models.Entities
{
    public class Doctor_Shift : BaseEntity
    {
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; } = null!;

        public DateTime ShiftDate { get; set; }             
        public string ShiftType { get; set; } = null!;       

        public TimeSpan StartTime { get; set; }              
        public TimeSpan EndTime { get; set; }               
              
        public string? Notes { get; set; }
    }
}
