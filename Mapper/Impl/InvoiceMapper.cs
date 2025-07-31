using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class InvoiceMapper : IInvoiceMapper
    {
        // Phương thức chuyển từ Entity sang DTO
        public InvoiceResponseDTO EntityToResponse(Invoice entity)
        {
            return new InvoiceResponseDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
                CreateBy = entity.CreateBy,
                UpdateBy = entity.UpdateBy,
                InitialAmount = entity.InitialAmount,
                DiscountAmount = entity.DiscountAmount,
                TotalAmount = entity.TotalAmount,
                Notes = entity.Notes,
                Status = entity.Status,
                AppointmentId = entity.AppointmentId,
                AppointmentName = entity.Appointment?.Name ?? "",      // Lấy tên lịch hẹn
                InsuranceId = entity.InsuranceId,
                InsuranceName = entity.Insurance?.Name ?? "",          // Lấy tên bảo hiểm
                PatientId = entity.PatientId,
                PatientName = entity.Patient?.Name ?? ""               // Lấy tên bệnh nhân
            };
        }


        // Phương thức chuyển danh sách Entity sang danh sách DTO
        public IEnumerable<InvoiceResponseDTO> ListEntityToResponse(IEnumerable<Invoice> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }
    }
}
