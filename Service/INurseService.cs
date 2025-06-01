namespace SWP391_SE1914_ManageHospital.Service
{
    public interface INurseService
    {
        Task<NurseDTO> GetNurseByIdAsync(int id);
        Task<IEnumerable<NurseDTO>> GetAllNursesAsync();
        Task<NurseDTO> CreateNurseAsync(NurseDTO nurseDto);
        Task<NurseDTO> UpdateNurseAsync(int id, NurseDTO nurseDto);
        Task<bool> DeleteNurseAsync(int id);
    }
}