using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

public interface IAppointmentService_1
{
    Task<IEnumerable<AppointmentResponseDTO_1>> GetAllAsync();
    Task<AppointmentResponseDTO_1?> GetByIdAsync(int id);
    public Task<IEnumerable<AppointmentResponseDTO_1>> GetByPatientIdAsync(int patientId);
    public Task<IEnumerable<AppointmentResponseDTO_1>> GetAllForAdminAsync();
    public Task<AppointmentResponseDTO_1> SoftDelete(int id, AppointmentStatus newStatus);
    Task<double> CalculateGrowthPercentageAsync();

}
