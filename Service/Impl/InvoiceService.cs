using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using static SWP391_SE1914_ManageHospital.Ultility.Status;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class InvoiceService :   IInvoiceService
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

    
    

    public async Task<decimal> GetTotalRevenueByMonthAsync()
{
    try
    {
        // Lấy ngày bắt đầu tháng này và tháng sau
        var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1); // End of the current month

        // Tính tổng doanh thu trong tháng này
        var totalRevenueThisMonth = await _context.Invoices
            .Where(i => i.CreateDate >= currentMonthStart && i.CreateDate <= currentMonthEnd && (int)i.Status == 1)
            .SumAsync(i => i.TotalAmount);

        return totalRevenueThisMonth;
    }
    catch (Exception ex)
    {
        throw new Exception("Error calculating total revenue for the month: " + ex.Message);
    }
}

public async Task<decimal> GetTotalRevenueByYearAsync()
{
    try
    {
        // Lấy ngày bắt đầu năm này và năm sau
        var currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
        var currentYearEnd = new DateTime(DateTime.Now.Year, 12, 31); // End of the current year

        // Tính tổng doanh thu trong năm này
        var totalRevenueThisYear = await _context.Invoices
            .Where(i => i.CreateDate >= currentYearStart && i.CreateDate <= currentYearEnd && (int)i.Status == 1)
            .SumAsync(i => i.TotalAmount);

        return totalRevenueThisYear;
    }
    catch (Exception ex)
    {
        throw new Exception("Error calculating total revenue for the year: " + ex.Message);
    }
}

    // Tính tổng doanh thu
    public async Task<decimal> GetTotalRevenueAsync()
    {
        try
        {
            // Tính tổng doanh thu (TotalAmount) trong bảng Invoice chỉ với status = 1
            var totalRevenue = await _context.Invoices
                .Where(i => (int)i.Status == 1)
                .SumAsync(i => i.TotalAmount);

            return totalRevenue;
        }
        catch (Exception ex)
        {
            throw new Exception("Error calculating total revenue: " + ex.Message);
        }
    }

    // Lấy tất cả hóa đơn (Invoice)
    public async Task<List<InvoiceResponseDTO>> GetAllInvoicesAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Invoices
            .Include(i => i.Appointment)
            .Include(i => i.Insurance)
            .Include(i => i.Patient)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(i => i.CreateDate >= startDate.Value);

        if (endDate.HasValue)
        {
            var endOfDay = endDate.Value.Date.AddDays(1).AddTicks(-1); // Lấy hết ngày endDate
            query = query.Where(i => i.CreateDate <= endOfDay);
        }

        var invoices = await query.ToListAsync();

        // Mapping sang DTO
        var invoiceResponseDTOs = invoices.Select(i => new InvoiceResponseDTO
        {
            Id = i.Id,
            Name = i.Name,
            Code = i.Code,
            CreateDate = i.CreateDate,
            UpdateDate = i.UpdateDate,
            CreateBy = i.CreateBy,
            UpdateBy = i.UpdateBy,
            InitialAmount = i.InitialAmount,
            DiscountAmount = i.DiscountAmount,
            TotalAmount = i.TotalAmount,
            Notes = i.Notes,
            Status = i.Status,
            AppointmentId = i.AppointmentId,
            AppointmentName = i.Appointment != null ? i.Appointment.Name : "",
            InsuranceId = i.InsuranceId,
            InsuranceName = i.Insurance != null ? i.Insurance.Name : "",
            PatientId = i.PatientId,
            PatientName = i.Patient != null ? i.Patient.Name : ""
        }).ToList();

        return invoiceResponseDTOs;
    }


}
