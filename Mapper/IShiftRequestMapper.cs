using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.ShiftRequest;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO.ShiftRequest;

namespace SWP391_SE1914_ManageHospital.Models.Mappers
{
    public interface IShiftRequestMapper
    {
        Shift_Request ToEntity(ShiftRequestRequestDTO dto);
        ShiftRequestResponseDTO ToResponseDTO(Shift_Request entity);
        void UpdateEntity(Shift_Request entity, ShiftRequestRequestDTO dto);
    }
}