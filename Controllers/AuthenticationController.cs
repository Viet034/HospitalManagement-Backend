using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Authenication;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Authentication;
using SWP391_SE1914_ManageHospital.Service;
using System.Security.Claims;

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
    [HttpPost("change-password")]
    [Authorize] // Vẫn yêu cầu đăng nhập
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO request)
    {
        try
        {
            // Lấy thông tin từ token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userTypeClaim = User.FindFirst("UserType")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) ||
                !int.TryParse(userIdClaim, out int userId) ||
                !Enum.TryParse<UserType>(userTypeClaim, out UserType userType))
            {
                return Unauthorized("Invalid token");
            }

            // Gọi service với đầy đủ thông tin
            var result = await _service.ChangePasswordAsync(
                userId,
                userType,
                request.OldPassword,
                request.NewPassword
            );

            return Ok("Đổi mật khẩu thành công");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ChangePassword");
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO request)
    {
        try
        {
            await _service.ForgotPasswordAsync(request.Email, request.UserType);
            return Ok("Hướng dẫn đặt lại mật khẩu đã được gửi đến email của bạn");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ForgotPassword");
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
    {
        try
        {
            await _service.ResetPasswordAsync(request.Token, request.NewPassword, request.UserType);
            return Ok("Đặt lại mật khẩu thành công");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ResetPassword");
            return BadRequest(ex.Message);
        }
    }
}
