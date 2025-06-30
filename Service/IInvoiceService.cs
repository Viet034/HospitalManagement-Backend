using SWP391_SE1914_ManageHospital.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IInvoiceService
    {
        Task<string> GenerateInvoiceCodeAsync();                         // Tạo mã hóa đơn
        Task<Invoice?> GetInvoiceByIdAsync(int id);                     // Lấy hóa đơn theo ID
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();               // Lấy tất cả hóa đơn
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);             // Tạo hóa đơn mới
        Task UpdateInvoiceAsync(Invoice invoice);                      // Cập nhật hóa đơn
        Task DeleteInvoiceAsync(int id);                               // Xóa hóa đơn
        Task<IEnumerable<Invoice>> GetInvoicesByPatientIdAsync(int patientId);  // Lấy hóa đơn theo bệnh nhân
        Task<bool> MarkInvoiceAsPaidAsync(int invoiceId);              // Đánh dấu hóa đơn là đã thanh toán
        Task<IEnumerable<Invoice>> SearchInvoicesAsync(string keyword); // Tìm hóa đơn theo từ khóa
    }
}
