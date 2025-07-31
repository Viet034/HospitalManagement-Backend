using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

public interface IAppointmentService_1
{
    Task<IEnumerable<AppointmentResponseDTO_1>> GetAllAsync();
    Task<AppointmentResponseDTO_1?> GetByIdAsync(int id);
    Task<double> CalculateGrowthPercentageAsync();

}
