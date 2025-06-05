using SWP391_SE1914_ManageHospital.Models.Entities;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _paymentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _paymentRepository.GetAllAsync();
    }

    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync();
        return payment;
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        // giả sử bạn thêm UpdateAsync ở repository
        _paymentRepository.UpdateAsync(payment);
        await _paymentRepository.SaveChangesAsync();
    }

    public async Task DeletePaymentAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment != null)
        {
            _paymentRepository.DeleteAsync(payment);
            await _paymentRepository.SaveChangesAsync();
        }
    }
}
