namespace SWP391_SE1914_ManageHospital.DTOs
{
	public class LabRequestDto
	{
		public int Id { get; set; } // Định nghĩa trường Id
		public int DoctorId { get; set; }
		public int PatientId { get; set; }
		public int AppointmentId { get; set; }
		public int MedicalRecordId { get; set; }
		public string TestType { get; set; }
		public string Description { get; set; }
		public DateTime RequestDate { get; set; }
		public string Status { get; set; }
		public string ResultText { get; set; }
		public string ResultFileUrl { get; set; }
		public DateTime ResultDate { get; set; }
	}
}