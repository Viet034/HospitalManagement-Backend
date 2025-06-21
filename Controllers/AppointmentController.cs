using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Service;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Get all appointments
    /// </summary>
    /// <returns>List of appointments</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> GetAllAppointments()
    {
        var appointments = await _appointmentService.GetAllAppointmentsAsync();
        return Ok(appointments);
    }

    /// <summary>
    /// Get appointment by ID
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <returns>Appointment details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentResponseDTO>> GetAppointmentById(int id)
    {
        try
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Search appointments by name
    /// </summary>
    /// <param name="name">Name to search for</param>
    /// <returns>List of matching appointments</returns>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> SearchAppointmentsByName([FromQuery] string name)
    {
        var appointments = await _appointmentService.SearchAppointmentsByNameAsync(name);
        return Ok(appointments);
    }

    /// <summary>
    /// Get appointments by patient ID
    /// </summary>
    /// <param name="patientId">Patient ID</param>
    /// <returns>List of appointments for the patient</returns>
    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> GetAppointmentsByPatientId(int patientId)
    {
        try
        {
            var appointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId);
            return Ok(appointments);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Create a new appointment
    /// </summary>
    /// <param name="create">Appointment creation data</param>
    /// <returns>Created appointment</returns>
    [HttpPost]
    public async Task<ActionResult<AppointmentResponseDTO>> CreateAppointment([FromBody] AppointmentCreate create)
    {
        try
        {
            var appointment = await _appointmentService.CreateAppointmentAsync(create);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an appointment
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="update">Updated appointment data</param>
    /// <returns>Updated appointment</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<AppointmentResponseDTO>> UpdateAppointment(int id, [FromBody] AppointmentUpdate update)
    {
        try
        {
            var appointment = await _appointmentService.UpdateAppointmentAsync(id, update);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Change appointment status
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="status">New status</param>
    /// <returns>Updated appointment</returns>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<AppointmentResponseDTO>> ChangeAppointmentStatus(int id, [FromBody] AppointmentStatus status)
    {
        try
        {
            var appointment = await _appointmentService.ChangeAppointmentStatusAsync(id, status);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Delete an appointment
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <returns>Success indicator</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAppointment(int id)
    {
        try
        {
            var result = await _appointmentService.DeleteAppointmentAsync(id);
            return result ? NoContent() : BadRequest("Failed to delete appointment");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
} 