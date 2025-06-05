using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Service;
using System.Net;

namespace SWP391_SE1914_ManageHospital.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _service;

    public DoctorController(IDoctorService service)
    {
        _service = service;
    }

    [HttpGet("get-active-by-clinic/{clinicId}")]
    [ProducesResponseType(typeof(IEnumerable<DoctorResponseDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<DoctorResponseDTO>>> GetActiveDoctorsByClinic(int clinicId)
    {
        try
        {
            var response = await _service.GetActiveDoctorsByClinicAsync(clinicId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-schedule/{doctorId}/{clinicId}")]
    [ProducesResponseType(typeof(IEnumerable<DoctorScheduleResponseDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<IEnumerable<DoctorScheduleResponseDTO>>> GetDoctorSchedule(
        int doctorId, 
        int clinicId, 
        [FromQuery] DateTime? fromDate = null, 
        [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var response = await _service.GetDoctorScheduleInClinicAsync(doctorId, clinicId, fromDate, toDate);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
} 