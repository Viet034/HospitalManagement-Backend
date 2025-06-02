namespace SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO
{
    public interface IDoctorMapper
    {
        DoctorDTO MapToDTO(Doctor doctor);
        Doctor MapToEntity(DoctorDTO doctorDTO);
    }
}