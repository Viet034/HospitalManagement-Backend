using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Mapper;

public class PatientFilterMapper : IPatientFilterMapper
{
    public PatientFilterResponse EntityToResponse(Patient entity)
    {
        return new PatientFilterResponse
        {
            Id = entity.Id,
            Name = entity.Name
            // Map thêm trường nếu cần
        };
    }

    public IEnumerable<PatientFilterResponse> ListEntityToResponse(IEnumerable<Patient> entities)
    {
        return entities.Select(EntityToResponse);
    }
}