
using Models.DTO.RequestDTO.Payment;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Department;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper;

public interface IPaymentMapper
{
    // request => Entity(DTO)
    Payment CreateToEntity(PaymentCreate create);
    Department UpdateToEntity(DepartmentUpdate update);
    Department DeleteToEntity(DepartmentDelete delete);

    // Entity(DTO) => Response
    PaymentResponseDTO EntityToResponse(Payment entity);
    IEnumerable<PaymentResponseDTO> ListEntityToResponse(IEnumerable<Payment> entities);
}
