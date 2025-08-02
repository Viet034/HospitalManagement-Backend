using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using System.Net;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

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

    [HttpGet("get-app-by-paid/{paid}")]
    [ProducesResponseType(typeof(IEnumerable<Appointment>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAppointmentsByPaidStatus(int paid)
    {
        try
        {
            var response = await _appointmentService.GetByPatientIdAsync(paid);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("change-status/{id}")]
    [ProducesResponseType(typeof(IEnumerable<Appointment>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SoftDeleteApp(int id, AppointmentStatus newStatus)
    {
        try
        {
            var response = await _appointmentService.SoftDelete(id, newStatus);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-app-admin")]
    [ProducesResponseType(typeof(IEnumerable<Appointment>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAppointmentsForAdmin()
    {
        try
        {
            var response = await _appointmentService.GetAllForAdminAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
