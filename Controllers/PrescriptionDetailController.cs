// Controllers/PrescriptionDetailController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PrescriptionDetail;
using SWP391_SE1914_ManageHospital.Service;
using System.Threading.Tasks;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]    // Yêu cầu login
    public class PrescriptionDetailController : ControllerBase
    {
        private readonly IPrescriptionDetailService _service;
        public PrescriptionDetailController(IPrescriptionDetailService service)
        {
            _service = service;
        }

        private int GetUserIdFromClaims()
        {
            var claim = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        private string GetRoleFromClaims()
        {
            return User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                   ?? string.Empty;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        /// <summary>
        /// Lấy PrescriptionDetail của chính bác sĩ hoặc bệnh nhân đang login
        /// </summary>
        [HttpGet("my-details")]
        public async Task<IActionResult> GetMyDetails()
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0) return Unauthorized("User chưa đăng nhập.");

            var role = GetRoleFromClaims();
            if (string.IsNullOrEmpty(role)) return Forbid();

            var list = await _service.GetByUserAsync(userId, role);
            return Ok(list);
        }

        [HttpGet("get-by-prescription/{prescriptionId}")]
        public async Task<IActionResult> GetByPrescription(int prescriptionId)
        {
            var list = await _service.GetByPrescriptionIdAsync(prescriptionId);
            return Ok(list);
        }

        [HttpPost("add-detail")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Create([FromBody] PrescriptionDetailRequest req)
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0) return Unauthorized("User chưa đăng nhập.");

            req.UserId = userId;
            var dto = await _service.CreateAsync(req);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("update-detail/{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] PrescriptionDetailRequest req)
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0) return Unauthorized("User chưa đăng nhập.");

            req.UserId = userId;
            var dto = await _service.UpdateAsync(id, req);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpDelete("delete-detail/{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }


    }
}
