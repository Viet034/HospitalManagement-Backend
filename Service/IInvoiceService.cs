using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;

namespace SWP391_SE1914_ManageHospital.Service;

public interface IInvoiceService
{
    public Task<List<InvoiceDTO>> GetPaymentsByPatientIdAsync(int patientId);

}
