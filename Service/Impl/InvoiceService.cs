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

    public async Task<List<InvoiceDTO>> GetPaymentsByPatientIdAsync(int userId)
    {
        //var invoices = await _context.Invoices
        //.Include(i => i.Appointment)
        //    .ThenInclude(a => a.Patient) // để truy cập Patient.UserId
        //.Include(i => i.Payment_Invoices)
        //    .ThenInclude(pi => pi.Payment)
        //.Where(i => i.Appointment.Patient.UserId == userId)
        //.Select(i => new InvoiceDTO
        //{
        //    Id = i.Id,
        //    TotalAmount = i.TotalAmount,
        //    DiscountAmount = i.DiscountAmount,
        //    AppointmentId = i.AppointmentId,
        //    InsuranceId = i.InsuranceId,
        //    PatientId = i.PatientId,
        //    CreateDate = i.CreateDate,
        //    Status = i.Status,

        //    Payments = i.Payment_Invoices.Select(pi => new PaymentDTO
        //    {
        //        Id = pi.Payment.Id,
        //        Amount = pi.Payment.Amount,
        //        PaymentDate = pi.Payment.PaymentDate,
        //        PaymentMethod = pi.Payment.PaymentMethod,
        //        Payer = pi.Payment.Payer,
        //        Notes = pi.Payment.Notes
        //    }).ToList()
        //})
        //.ToListAsync();

        //return invoices;
        var invoices = await _context.Invoices
        .Include(i => i.Appointment)
            .ThenInclude(a => a.Patient)
        .Include(i => i.Payment_Invoices)
            .ThenInclude(pi => pi.Payment)
        .Where(i => i.Appointment.Patient.UserId == userId)
        .ToListAsync();

        var invoiceDTOs = new List<InvoiceDTO>();

        foreach (var invoice in invoices)
        {
            var hasFeedback = await _context.Feedbacks
                .AnyAsync(f => f.AppointmentId == invoice.AppointmentId);

            invoiceDTOs.Add(new InvoiceDTO
            {
                Id = invoice.Id,
                TotalAmount = invoice.TotalAmount,
                DiscountAmount = invoice.DiscountAmount,
                AppointmentId = invoice.AppointmentId,
                InsuranceId = invoice.InsuranceId,
                PatientId = invoice.PatientId,
                CreateDate = invoice.CreateDate,
                Status = invoice.Status,
                HasFeedback = hasFeedback,
                Payments = invoice.Payment_Invoices.Select(pi => new PaymentDTO
                {
                    Id = pi.Payment.Id,
                    Amount = pi.Payment.Amount,
                    PaymentDate = pi.Payment.PaymentDate,
                    PaymentMethod = pi.Payment.PaymentMethod,
                    Payer = pi.Payment.Payer,
                    Notes = pi.Payment.Notes
                }).ToList()
            });
        }

        return invoiceDTOs;
    }
}
