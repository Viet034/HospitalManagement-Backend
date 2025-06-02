using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Clinic;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper;

public interface IClinicMapper
{
    // request => Entity(DTO)
    
    Clinic CreateToEntity(ClinicCreate create);
    Clinic UpdateToEntity(ClinicUpdate update);
    Clinic DeleteToEntity(ClinicDelete delete);

    // Entity(DTO) => Response
    ClinicResponse EntityToResponse(Clinic entity);
    IEnumerable<ClinicResponse> ListEntityToResponse(IEnumerable<Clinic> entities);
}
