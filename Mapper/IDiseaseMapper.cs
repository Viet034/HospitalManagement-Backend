using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Disease;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface IDiseaseMapper
    {
        DiseaseResponse MapToResponse(DiseaseDTO entity);
        Disease MapCreateRequestToEntity(DiseaseCreateRequest request);
        void MapUpdateRequestToEntity(Disease entity, DiseaseUpdateRequest request);
    }
}