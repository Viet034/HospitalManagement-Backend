using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Ultility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDBContext _context;

        public InvoiceService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateInvoiceCodeAsync()
        {
            string newCode;
            bool exists;
            do
            {
                newCode = $"INV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..4].ToUpper()}";
                exists = await _context.Invoices.AnyAsync(x => x.Code == newCode);
            } while (exists);

            return newCode;
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.InvoiceDetails)
                .Include(i => i.Payment_Invoices)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            return await _context.Invoices
                .Include(i => i.Patient)
                .Include(i => i.Appointment)
                .OrderByDescending(i => i.CreateDate)
                .ToListAsync();
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            invoice.Code = await GenerateInvoiceCodeAsync();
            invoice.Status = InvoiceStatus.Unpaid;
            invoice.CreateDate = DateTime.Now;
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByPatientIdAsync(int patientId)
        {
            return await _context.Invoices
                .Where(i => i.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<bool> MarkInvoiceAsPaidAsync(int invoiceId)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null) return false;

            invoice.Status = InvoiceStatus.Paid;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Invoice>> SearchInvoicesAsync(string keyword)
        {
            return await _context.Invoices
                .Where(i => i.Code.Contains(keyword) || i.Notes.Contains(keyword))
                .ToListAsync();
        }
    }
}
