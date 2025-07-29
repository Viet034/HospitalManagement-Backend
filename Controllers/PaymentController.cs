using Microsoft.AspNetCore.Mvc;
using Models.DTO.RequestDTO.Payment;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    

    [HttpPost("make-payment")]
    public async Task<IActionResult> MakePayment([FromBody] PaymentCreate create)
    {
        try
        {
            var success = await _paymentService.MakePaymentAsync(create);
            if (success) return Ok("Thanh toán thành công");
            return BadRequest("Thanh toán thất bại");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi khi thanh toán: {ex.ToString()}");
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllPayments()
    {
        var result = await _paymentService.GetAllPaymentsAsync();
        return Ok(result);
    }

}