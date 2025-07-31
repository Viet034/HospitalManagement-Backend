using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Service;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController_1 : ControllerBase
{
    private readonly IAppointmentService_1 _appointmentService;

    public AppointmentController_1(IAppointmentService_1 appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAppointments_1()
    {
        var result = await _appointmentService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetAppointmentById_1(int id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);
        if (appointment == null)
        {
            return NotFound(new { message = "Không tìm thấy cuộc hẹn." });
        }
        return Ok(appointment);
    }
    [HttpGet("calculate%")]
    public async Task<IActionResult> CalculateGrowthPercentage()
    {
        var result = await _appointmentService.CalculateGrowthPercentageAsync();
        return Ok(result);
    }

}
