using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using static SWP391_SE1914_ManageHospital.Ultility.Status;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class InvoiceService : IInvoiceService
{
    private readonly ApplicationDBContext _context;

    public InvoiceService(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<string> CheckUniqueCodeAsync()
    {
        string newCode;
        bool isExist;

        do
        {
            newCode = GenerateCode.GenerateDepartmentCode();
            _context.ChangeTracker.Clear();
            isExist = await _context.Departments.AnyAsync(p => p.Code == newCode);
        }
        while (isExist);

        return newCode;
    }

    public async Task<bool> GenerateInvoiceDetailsAsync(int appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments
            .Include(a => a.Invoice)
            .Include(a => a.Medical_Record)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment == null || appointment.Invoice == null)
            return false;

        var invoice = appointment.Invoice;
        
        decimal totalAmount = 0;

        // Xử lý Tuền thuốc
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
                    var presDetail = new InvoiceDetail
                    {
                        InvoiceId = invoice.Id,
                        PrescriptionsId = prescription.Id,
                        ServiceId = null,
                        TotalAmount = prescription.Amount,
                        Notes = "Tiền thuốc",
                        Name = prescription.Name,
                        Code = "PRES-" + prescription.Id,
                        Status = InvoiceDetailStatus.Normal,
                        Discount = 0,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        UpdateBy = "System",
                        CreateBy = "System"
                    };
                        //Console.WriteLine("Adding InvoiceDetail for PrescriptionId: " + prescriptionId);

                        //if (invoice.InvoiceDetails == null)
                        //    invoice.InvoiceDetails = new List<InvoiceDetail>();

                        //invoice.InvoiceDetails.Add(presDetail);

                        //var entry = _context.Entry(presDetail);
                        //Console.WriteLine("Entity state after adding: " + entry.State); // Phải là Added
                        _context.InvoiceDetails.Add(presDetail);

                    }
                }
            //Console.WriteLine($"Prescription alreadyAdded: {alreadyAdded}");
        }

         //   Xử lý Service
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
                        var servDetail = new InvoiceDetail
                        {
                            InvoiceId = invoice.Id,
                            PrescriptionsId = null,
                            ServiceId = service.Id,
                            TotalAmount = service.Price,
                            Notes = "Tiền dịch vụ",
                            Name = service.Name,
                            Code = "SERV-" + service.Id,
                            Status = InvoiceDetailStatus.Normal,
                            CreateDate = DateTime.Now,
                            Discount = 0,
                            CreateBy = "System",
                            UpdateDate = DateTime.Now,
                            UpdateBy = "System"
                        };
                        //Console.WriteLine("Adding InvoiceDetail for ServiceId: " + serviceId);
                        _context.InvoiceDetails.Add(servDetail);
                    }
                }
                //Console.WriteLine($"Service alreadyAdded: {alreadyAdded}");
            }




            await _context.SaveChangesAsync();
            // Tổng hợp TotalAmount mới
            invoice.TotalAmount = await _context.InvoiceDetails
                .Where(d => d.InvoiceId == invoice.Id)
                .SumAsync(d => d.TotalAmount);

            invoice.InitialAmount = invoice.TotalAmount;
            await _context.SaveChangesAsync();
            //foreach (var entry in _context.ChangeTracker.Entries<InvoiceDetail>())
            //{
            //    Console.WriteLine($"EntityState: {entry.State} - InvoiceId: {entry.Entity.InvoiceId}, " +
            //                      $"PresId: {entry.Entity.PrescriptionsId}, ServiceId: {entry.Entity.ServiceId}");
            //}
            //var savedDetails = await _context.InvoiceDetails
            //    .Where(d => d.InvoiceId == invoice.Id)
            //    .ToListAsync();
            //Console.WriteLine($"Saved InvoiceDetails count: {savedDetails.Count}");
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi khi lưu vào database: " + ex.Message);
            
        }
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
