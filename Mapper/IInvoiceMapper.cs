using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface IInvoiceMapper
    {
        // Chuyển từ Entity sang DTO
        InvoiceResponseDTO EntityToResponse(Invoice entity);

        // Chuyển danh sách từ Entity sang DTO
        IEnumerable<InvoiceResponseDTO> ListEntityToResponse(IEnumerable<Invoice> entities);
    }
}
