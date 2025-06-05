using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper;
 
public interface IDoctorMapper
{
    DoctorResponseDTO EntityToResponse(Doctor entity);
    IEnumerable<DoctorResponseDTO> ListEntityToResponse(IEnumerable<Doctor> entities);
} 