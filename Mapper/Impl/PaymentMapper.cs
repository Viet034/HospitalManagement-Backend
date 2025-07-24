using Microsoft.AspNetCore.Http.HttpResults;
using Models.DTO.RequestDTO.Payment;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Department;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl;

public class PaymentMapper : IPaymentMapper
{
    public Payment CreateToEntity(PaymentCreate create)
    {
        Payment paymennt = new Payment();
        paymennt.PaymentDate = create.PaymentDate;
        paymennt.Amount = create.Amount;
        paymennt.PaymentMethod = create.PaymentMethod;
        paymennt.Payer = create.Payer;
        paymennt.Notes = create.Notes;
        paymennt.Name = create.Name;
        paymennt.Code = create.Code;
        paymennt.CreateDate = DateTime.UtcNow.AddHours(7);
        paymennt.CreateBy = create.CreateBy;
        paymennt.UpdateDate = create.UpdateDate;
        paymennt.UpdateBy = create.UpdateBy;
        return paymennt;
    }

    public Department DeleteToEntity(DepartmentDelete delete)
    {
        throw new NotImplementedException();
    }

    public PaymentResponseDTO EntityToResponse(Payment entity)
    {
        PaymentResponseDTO response = new PaymentResponseDTO();
        response.Id = entity.Id;
        response.PaymentDate = entity.PaymentDate;
        response.Amount = entity.Amount;
        response.PaymentMethod = entity.PaymentMethod;
        response.Payer = entity.Payer;
        response.Notes = entity.Notes;
        response.Name = entity.Name;
        response.Code = entity.Code;
        response.CreateDate = DateTime.UtcNow.AddHours(7);
        response.CreateBy = entity.CreateBy;
        response.UpdateDate = entity.UpdateDate;
        response.UpdateBy = entity.UpdateBy;
        return response;
    }

    public IEnumerable<PaymentResponseDTO> ListEntityToResponse(IEnumerable<Payment> entities)
    {
        return entities.Select(x => EntityToResponse(x)).ToList();
    }

    public Department UpdateToEntity(DepartmentUpdate update)
    {
        throw new NotImplementedException();
    }
}