namespace Models.DTO.ResponseDTO
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }
        public int PatientId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsConfirmed { get; set; }
        public string? Note { get; set; }
    }
}
