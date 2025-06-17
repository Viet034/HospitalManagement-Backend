public class CreatePrescriptionDTO
{
	public int PatientId { get; set; }
	public int DoctorId { get; set; }
	public string Diagnosis { get; set; }
	public List<PrescriptionItemDTO> Items { get; set; }
}

public class PrescriptionItemDTO
{
	public int MedicineId { get; set; }
	public string Usage { get; set; }
	public int Quantity { get; set; }
}
