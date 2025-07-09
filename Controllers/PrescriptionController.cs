using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;          // <-- cần để xài FirstOrDefaultAsync
using System.Security.Claims;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Prescription;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Phải đăng nhập
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly ApplicationDBContext _context;

        public PrescriptionController(IPrescriptionService prescriptionService, ApplicationDBContext context)
        {
            _prescriptionService = prescriptionService;
            _context = context;
        }

        private int GetUserIdFromClaims()
        {
            var claim = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        // DÙNG đúng DbSet<User_Role> với tên User_Roles
        private async Task<bool> IsDoctor(int userId)
        {
            // lấy bản ghi user-role
            var ur = await _context.User_Roles
                                   .FirstOrDefaultAsync(x => x.UserId == userId);
            if (ur == null)
                return false;

            // kiểm tra Role.Name == "Doctor"
            var role = await _context.Roles
                                     .FirstOrDefaultAsync(r => r.Id == ur.RoleId);
            return role != null && role.Name == "Doctor";
        }

        private string GetUserNameFromClaims()
        {
            // Lấy ClaimTypes.Name (hoặc ClaimTypes.NameIdentifier) tuỳ bạn đã ghi vào token như thế nào
            return User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                   ?? "system";
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _prescriptionService.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _prescriptionService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // Lấy đơn thuốc của bác sĩ theo DoctorId (dành cho Admin hoặc bác sĩ)
        [HttpGet("get-by-doctor/{doctorId}")]
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var list = await _prescriptionService.GetByDoctorIdAsync(doctorId);
            return Ok(list);
        }

        // Lấy đơn thuốc của bệnh nhân theo PatientId
        [HttpGet("get-by-patient/{patientId}")]
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var list = await _prescriptionService.GetByPatientIdAsync(patientId);
            return Ok(list);
        }

        // Lấy đơn thuốc của bác sĩ hiện tại hoặc bệnh nhân dựa trên UserId từ JWT token
        [HttpGet("my-prescriptions")]
        [Authorize]
        public async Task<IActionResult> GetMine()
        {
            // Lấy UserId từ JWT token
            var userId = GetUserIdFromClaims();
            if (userId == 0) return Unauthorized("User chưa đăng nhập.");

            // Lấy role của người dùng
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(role)) return Unauthorized("Role không hợp lệ.");

            var list = await _prescriptionService.GetMineAsync(userId, role);
            return Ok(list);
        }

        [HttpPost("add-prescription")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create([FromBody] PrescriptionRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized("User chưa đăng nhập.");

            if (!await IsDoctor(userId))
                return Forbid("Chỉ bác sĩ mới có quyền tạo đơn thuốc.");

            request.UserId = userId;
            var created = await _prescriptionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("update-prescription/{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Update(int id, [FromBody] PrescriptionRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized("User chưa đăng nhập.");

            if (!await IsDoctor(userId))
                return Forbid("Chỉ bác sĩ mới có quyền cập nhật đơn thuốc.");

            request.UserId = userId;
            var updated = await _prescriptionService.UpdateAsync(id, request);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        [HttpDelete("delete-prescription/{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized("User chưa đăng nhập.");

            if (!await IsDoctor(userId))
                return Forbid("Chỉ bác sĩ mới có quyền xóa đơn thuốc.");

            var ok = await _prescriptionService.DeleteAsync(id);
            if (!ok)
                return NotFound();
            return NoContent();
        }

        [HttpPut("update-status/{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] PrescriptionUpdateStatusResquest req)
        {
            // xác thực token, lấy tên user ra để ghi vào UpdateBy
            var userName = GetUserNameFromClaims();

            var updated = await _prescriptionService.UpdateStatusAsync(id, req.Status, userName);
            if (updated == null)
                return NotFound($"Không tìm thấy Prescription với ID = {id}");
            return Ok(updated);
        }
    }
}
