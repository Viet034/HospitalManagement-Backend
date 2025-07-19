using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.ShiftRequest;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO.ShiftRequest;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.Mappers;
namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class ShiftRequestMapper : IShiftRequestMapper
    {
        public Shift_Request ToEntity(ShiftRequestRequestDTO dto)
        {
            return new Shift_Request
            {
                DoctorId = dto.DoctorId,
                ShiftId = dto.ShiftId,
                RequestType = dto.RequestType,
                Reason = dto.Reason,
                Status = ShiftRequestStatus.Pending,
                CreatedDate = DateTime.UtcNow
            };
        }

        public ShiftRequestResponseDTO ToResponseDTO(Shift_Request entity)
        {
            return new ShiftRequestResponseDTO
            {
                Id = entity.Id,
                DoctorId = entity.DoctorId,
                ShiftId = entity.ShiftId,
                RequestType = entity.RequestType,
                Reason = entity.Reason,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                ApprovedDate = entity.ApprovedDate,
                DoctorName = entity.Doctor?.Name,
                ShiftType = entity.Doctor_Shift?.ShiftType,
                ShiftDate = entity.Doctor_Shift?.ShiftDate
            };
        }

        public void UpdateEntity(Shift_Request entity, ShiftRequestRequestDTO dto)
        {
            entity.ShiftId = dto.ShiftId;
            entity.RequestType = dto.RequestType;
            entity.Reason = dto.Reason;
        }
    }
}
