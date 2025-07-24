
namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public class PaymentResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string CreateBy { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string Payer { get; set; }
    public string? Notes { get; set; }

    public PaymentResponseDTO()
    {
    }

    public PaymentResponseDTO(int id, string name, string code, DateTime createDate, DateTime? updateDate, string createBy, string? updateBy, DateTime paymentDate, decimal amount, string paymentMethod, string payer, string? notes)
    {
        Id = id;
        Name = name;
        Code = code;
        CreateDate = createDate;
        UpdateDate = updateDate;
        CreateBy = createBy;
        UpdateBy = updateBy;
        PaymentDate = paymentDate;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Payer = payer;
        Notes = notes;
    }
}