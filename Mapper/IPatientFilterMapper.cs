using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Mapper;

public interface IPatientFilterMapper
{
    PatientFilterResponse EntityToResponse(Patient entity);
    IEnumerable<PatientFilterResponse> ListEntityToResponse(IEnumerable<Patient> entities);
}