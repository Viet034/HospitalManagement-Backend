using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Service;

public interface IInvoiceService
{
    public Task<List<InvoiceDTO>> GetPaymentsByPatientIdAsync(int patientId);
    Task<bool> GenerateInvoiceDetailsAsync(int appointmentId);
   
    Task<decimal> GetTotalRevenueAsync(); // Lấy tất cả hóa đơn
     // Tính tỷ lệ phần trăm doanh thu tháng này so với tháng trước
    Task<decimal> GetTotalRevenueByMonthAsync();

    // Tính tỷ lệ phần trăm doanh thu năm này so với năm trước
    Task<decimal> GetTotalRevenueByYearAsync();

    Task<List<InvoiceResponseDTO>> GetAllInvoicesAsync(DateTime? startDate, DateTime? endDate);



}
