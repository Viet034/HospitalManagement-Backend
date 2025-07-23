using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Department;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Doctor;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using System.Collections.Generic;

namespace SWP391_SE1914_ManageHospital.Mapper;

public interface IDoctorMapper
{
    // request => Entity(DTO)
    Doctor CreateToEntity(DoctorCreate create);
    Doctor UpdateToEntity(DoctorUpdate update);
    Doctor DeleteToEntity(DoctorDelete delete);

    // Entity(DTO) => Response
    DoctorResponseDTO EntityToResponse(Doctor entity);
    IEnumerable<DoctorResponseDTO> ListEntityToResponse(IEnumerable<Doctor> entities);
}