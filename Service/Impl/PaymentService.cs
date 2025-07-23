
using Microsoft.EntityFrameworkCore;
using Models.DTO.RequestDTO.Payment;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.Entities;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDBContext _context;
    private readonly IPaymentMapper _mapper;

    public PaymentService(ApplicationDBContext context, IPaymentMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> MakePaymentAsync(PaymentCreate create)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Lấy thông tin Invoice và liên kết liên quan
            var invoice = await _context.Invoices
                .Include(i => i.Payment_Invoices)
                .Include(i => i.Appointment)
                .FirstOrDefaultAsync(i => i.Id == create.InvoiceId);

            if (invoice == null)
                throw new Exception("Invoice not found");

            // 2. Map DTO -> Entity (nên dùng mapper nếu có)
            var payment = _mapper.CreateToEntity(create);

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // 3. Tạo bản ghi Payment_Invoice
            var paymentInvoice = new Payment_Invoice
            {
                InvoiceId = invoice.Id,
                PaymentId = payment.Id,
                AmountPaid = create.Amount
            };

            _context.Payment_Invoices.Add(paymentInvoice);

            // 4. Tính tổng tiền đã thanh toán (bao gồm cả payment vừa thêm)
            decimal totalPaid = invoice.Payment_Invoices.Sum(pi => pi.AmountPaid) + create.Amount;

            if (totalPaid >= invoice.TotalAmount)
            {
                invoice.Status = InvoiceStatus.Paid;
                invoice.Appointment.Status = AppointmentStatus.Completed;
            }
            else if (totalPaid > 0)
            {
                invoice.Status = InvoiceStatus.PartiallyPaid;
            }

            // 5. Cập nhật thông tin sửa đổi
            invoice.UpdateDate = DateTime.UtcNow.AddHours(7);
            invoice.UpdateBy = create.Payer;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}