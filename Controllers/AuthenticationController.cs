using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Authenication;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthService service, ILogger<AuthenticationController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("register/patient")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterPatient([FromBody] PatientRegisterRequest request)
    {
        try
        {
            var response = await _service.RegisterPatientAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during patient registration");
            return BadRequest(ex.ToString());
        }
    }
}
