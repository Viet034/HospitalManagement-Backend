using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponseDTO>> GetAllDoctorsAsync();
        Task<DoctorResponseDTO> GetDoctorByIdAsync(int id);
        Task<DoctorResponseDTO> CreateDoctorAsync(DoctorCreate doctorDTO);
        Task<bool> UpdateDoctorAsync(int id, DoctorUpdate doctorDTO);
        Task<bool> DeleteDoctorAsync(DoctorDelete doctorDTO);
    }
}