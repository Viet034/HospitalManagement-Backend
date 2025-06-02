namespace SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync();
        Task<DoctorDTO> GetDoctorByIdAsync(int id);
        Task<DoctorDTO> CreateDoctorAsync(DoctorDTO doctorDTO);
        Task<bool> UpdateDoctorAsync(int id, DoctorDTO doctorDTO);
        Task<bool> DeleteDoctorAsync(int id);
    }
}