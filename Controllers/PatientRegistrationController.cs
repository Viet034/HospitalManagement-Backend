using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Patient;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Service;
using System.Security.Claims;

namespace SWP391_SE1914_ManageHospital.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientRegistrationController : ControllerBase
{
    private readonly IPatientRegistrationService _service;

    public PatientRegistrationController(IPatientRegistrationService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(PatientProfileDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterPatient([FromBody] PatientRegistrationDTO request)
    {
        try
        {
            // Check if request is from authenticated user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            PatientProfileDTO result;
            if (!string.IsNullOrEmpty(userId))
            {
                // Register existing user as patient
                result = await _service.RegisterExistingUserAsPatientAsync(request, userId);
            }
            else
            {
                // Register new patient with new account
                result = await _service.RegisterNewPatientAsync(request);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("check-email/{email}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckEmailExists(string email)
    {
        var exists = await _service.IsEmailRegisteredAsync(email);
        return Ok(exists);
    }

    [HttpGet("check-contact/{contact}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckContactExists(string contact)
    {
        var exists = await _service.IsContactRegisteredAsync(contact);
        return Ok(exists);
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(PatientProfileDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentPatientProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User not authenticated");
        }

        var profile = await _service.GetPatientProfileByUserIdAsync(userId);
        if (profile == null)
        {
            return NotFound("No patient profile found for current user");
        }

        return Ok(profile);
    }

    [Authorize]
    [HttpGet("has-profile")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckHasProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User not authenticated");
        }

        var hasProfile = await _service.HasPatientProfileAsync(userId);
        return Ok(hasProfile);
    }
} 