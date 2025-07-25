using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using static SWP391_SE1914_ManageHospital.Ultility.Status;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class InvoiceService : IInvoiceService
{
    private readonly ApplicationDBContext _context;

    public InvoiceService(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<bool> GenerateInvoiceDetailsAsync(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Invoice)
            .Include(a => a.Medical_Record)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment == null || appointment.Invoice == null)
            return false;

        var invoice = appointment.Invoice;
        decimal totalAmount = 0;

        // Xử lý Prescription
        var prescriptionId = appointment.Medical_Record?.PrescriptionId;

        if (prescriptionId.HasValue)
        {
            bool alreadyAdded = await _context.InvoiceDetails
                .AnyAsync(d => d.InvoiceId == invoice.Id && d.PrescriptionsId == prescriptionId);

            if (!alreadyAdded)
            {
                var prescription = await _context.Prescriptions.FindAsync(prescriptionId);
                if (prescription != null)
                {
                    var detail = new InvoiceDetail
                    {
                        InvoiceId = invoice.Id,
                        PrescriptionsId = prescription.Id,
                        TotalAmount = prescription.Amount,
                        Status = InvoiceDetailStatus.Normal,
                        Notes = "Tiền thuốc"
                    };
                    _context.InvoiceDetails.Add(detail);
                    totalAmount += prescription.Amount;
                }
            }
        }

        // Xử lý Service
        var serviceId = appointment.ServiceId;

        if (serviceId.HasValue)
        {
            bool alreadyAdded = await _context.InvoiceDetails
                .AnyAsync(d => d.InvoiceId == invoice.Id && d.ServiceId == serviceId);

            if (!alreadyAdded)
            {
                var service = await _context.Services.FindAsync(serviceId);
                if (service != null)
                {
                    var detail = new InvoiceDetail
                    {
                        InvoiceId = invoice.Id,
                        ServiceId = service.Id,
                        TotalAmount = service.Price,
                        Status = InvoiceDetailStatus.Normal,
                        Notes = "Tiền dịch vụ khám"
                    };
                    _context.InvoiceDetails.Add(detail);
                    totalAmount += service.Price;
                }
            }
        }

        // Tổng hợp TotalAmount mới
        invoice.TotalAmount = await _context.InvoiceDetails
            .Where(d => d.InvoiceId == invoice.Id)
            .SumAsync(d => d.TotalAmount);

        invoice.InitialAmount = invoice.TotalAmount;
        invoice.UpdateDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }
    

    public async Task<List<InvoiceDTO>> GetPaymentsByPatientIdAsync(int userId)
    {
        
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
