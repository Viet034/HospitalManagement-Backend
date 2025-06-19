using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supply;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface ISupplyMapper
    {
        SupplyResponseDTO ToResponseDTO(Supply entity);
        IEnumerable<SupplyResponseDTO> ListEntityToResponse(IEnumerable<Supply> entities);
        Supply ToEntity(SupplyCreate dto);
        void UpdateEntity(Supply entity, SupplyUpdate dto);
    }
}

