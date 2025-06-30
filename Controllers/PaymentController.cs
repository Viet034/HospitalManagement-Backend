using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Ultility;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;

        public PaymentController(IPaymentService paymentService, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _configuration = configuration;
        }

        // ------------------ CRUD ------------------

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _paymentService.CreatePaymentAsync(payment);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != payment.Id)
                return BadRequest("Id không khớp");

            var existing = await _paymentService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _paymentService.UpdatePaymentAsync(payment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound();

            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
        }

        // ------------------ VNPay ------------------

        [HttpGet("create-vnpay-url")]
        public IActionResult CreateVNPayUrl(decimal amount)
        {
            var vnp_TmnCode = _configuration["VNPay:TmnCode"];
            var vnp_HashSecret = _configuration["VNPay:HashSecret"];
            var vnp_Url = _configuration["VNPay:BaseUrl"];
            var vnp_ReturnUrl = _configuration["VNPay:ReturnUrl"];

            var vnpay = new VNPayLibrary();
            string txnRef = DateTime.Now.Ticks.ToString();

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((int)(amount * 100)).ToString());
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", txnRef);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán viện phí");
            vnpay.AddRequestData("vnp_OrderType", "billpayment");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            vnpay.AddRequestData("vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1");
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return Ok(new { paymentUrl });
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VNPayReturn()
        {
            var vnp_TxnRef = Request.Query["vnp_TxnRef"].ToString();
            var vnp_ResponseCode = Request.Query["vnp_ResponseCode"].ToString();
            var vnp_TransactionStatus = Request.Query["vnp_TransactionStatus"].ToString();
            var vnp_AmountRaw = Request.Query["vnp_Amount"].ToString();

            // Kiểm tra các tham số cần thiết
            if (string.IsNullOrEmpty(vnp_TxnRef) || string.IsNullOrEmpty(vnp_ResponseCode) ||
                string.IsNullOrEmpty(vnp_TransactionStatus) || string.IsNullOrEmpty(vnp_AmountRaw))
            {
                return BadRequest("Thiếu tham số từ VNPay callback.");
            }

            // Parse số tiền
            if (!decimal.TryParse(vnp_AmountRaw, out decimal amountVND))
            {
                return BadRequest("vnp_Amount không hợp lệ.");
            }

            decimal amount = amountVND / 100; // VNPay trả về số tiền x 100

            // Kiểm tra kết quả thanh toán thành công
            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
            {
                var payment = new Payment
                {
                    TransactionId = vnp_TxnRef,
                    Amount = amount,
                    PaymentDate = DateTime.Now,
                    Status = "Paid",
                    PaymentMethod = "VNPay",
                    Payer = "VNPayUser" // hoặc đọc từ query nếu có
                };

                await _paymentService.CreatePaymentAsync(payment);

                return Ok(new { message = "Thanh toán thành công", txnRef = vnp_TxnRef });
            }
            else
            {
                return BadRequest("Thanh toán không thành công từ VNPay.");
            }
        }
    }
}
