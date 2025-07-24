using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class InvoiceService : IInvoiceService
{
    private readonly ApplicationDBContext _context;

    public InvoiceService(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<InvoiceDTO>> GetPaymentsByPatientIdAsync(int patientId)
    {
        var invoices = await _context.Invoices
        .Include(i => i.Appointment)
        .Include(i => i.Payment_Invoices)
            .ThenInclude(pi => pi.Payment)
        .Where(i => i.Appointment.PatientId == patientId && i.Payment_Invoices.Any())
        .Select(i => new InvoiceDTO
        {
            Id = i.Id,
            TotalAmount = i.TotalAmount,
            DiscountAmount = i.DiscountAmount,
            AppointmentId = i.AppointmentId,
            InsuranceId = i.InsuranceId,
            PatientId = i.PatientId,
            CreateDate = i.CreateDate,
            Status = i.Status,

            Payments = i.Payment_Invoices.Select(pi => new PaymentDTO
            {
                Id = pi.Payment.Id,
                Amount = pi.Payment.Amount,
                PaymentDate = pi.Payment.PaymentDate,
                PaymentMethod = pi.Payment.PaymentMethod,
                Payer = pi.Payment.Payer,
                Notes = pi.Payment.Notes
            }).ToList()
        })
        .ToListAsync();

        return invoices;
    }
}
