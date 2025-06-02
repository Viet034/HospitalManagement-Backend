using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO
{
    public interface IDoctorMapper
    {
        DoctorResponseDTO MapToDTO(Doctor doctor);
        Doctor MapToEntity(DoctorCreate doctorDTO);
        Doctor MapToEntity(DoctorUpdate doctorDTO);
    }
}