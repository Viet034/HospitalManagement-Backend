using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Prescription;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Service;
using System.Threading.Tasks;
using SWP391_SE1914_ManageHospital.Data;
using Microsoft.EntityFrameworkCore;
using static SWP391_SE1914_ManageHospital.Ultility.Status;


namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly ApplicationDBContext _context;

        public PrescriptionController(IPrescriptionService prescriptionService, ApplicationDBContext context)
        {
            _prescriptionService = prescriptionService;
            _context = context;
        }

        // Lấy UserId từ Claims
        private int GetUserIdFromClaims()
        {
            var claim = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        // Kiểm tra người dùng có phải bác sĩ không
        private async Task<bool> IsDoctor(int userId)
        {
            var ur = await _context.User_Roles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (ur == null) return false;
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == ur.RoleId);
            return role != null && role.Name == "Doctor";
        }

        // Lấy tên người dùng từ JWT claims
        private string GetUserNameFromClaims()
        {
            return User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "system";
        }

        // Lấy tất cả đơn thuốc
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _prescriptionService.GetAllAsync();
            return Ok(list);
        }

        // Lấy đơn thuốc theo ID
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _prescriptionService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // Lấy đơn thuốc của bác sĩ theo DoctorId
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

        // Lấy đơn thuốc của người dùng hiện tại
        [HttpGet("my-prescriptions")]
        [Authorize]
        public async Task<IActionResult> GetMine()
        {
            // Lấy UserId từ JWT token
            var userId = GetUserIdFromClaims();
            if (userId == 0) return Unauthorized("User chưa đăng nhập.");

            // Lấy role của người dùng từ claims
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(role)) return Unauthorized("Role không hợp lệ.");

            // Lấy danh sách đơn thuốc của người dùng hiện tại
            var prescriptions = await _prescriptionService.GetMineAsync(userId, role);

            // Sắp xếp đơn thuốc theo ngày tạo (sắp xếp tăng dần, đơn thuốc mới sẽ ở cuối)
            prescriptions = prescriptions.OrderBy(p => p.CreateDate).ToList();

            return Ok(prescriptions); // Trả về danh sách đơn thuốc đã ánh xạ thông tin
        }




        // Thêm đơn thuốc mới
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
            // Sau khi tạo xong đơn thuốc, trả về đơn thuốc mới tạo
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        // Cập nhật đơn thuốc
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

        // Xóa đơn thuốc
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
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] PrescriptionUpdateStatusRequest request)
        {
            // Lấy thông tin đơn thuốc từ ID
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return NotFound($"Không tìm thấy đơn thuốc với ID = {id}");
            }

            // Kiểm tra nếu trạng thái hiện tại là Dispensed hoặc Cancelled, không cho phép thay đổi nữa
            if (prescription.Status == PrescriptionStatus.Dispensed || prescription.Status == PrescriptionStatus.Cancelled)
            {
                return BadRequest("Đơn thuốc này đã được cấp phát hoặc hủy, không thể thay đổi trạng thái.");
            }

            // Kiểm tra nếu trạng thái đang từ "New" chuyển sang "Dispensed" thì cần trừ thuốc khỏi kho
            if (prescription.Status == PrescriptionStatus.New && request.Status == PrescriptionStatus.Dispensed)
            {
                // Cập nhật trạng thái của đơn thuốc thành Dispensed
                prescription.Status = PrescriptionStatus.Dispensed;
                prescription.UpdateDate = DateTime.UtcNow;

                // Cập nhật tên bác sĩ vào UpdateBy
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy UserId từ Claims
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == int.Parse(userId));
                if (doctor != null)
                {
                    prescription.UpdateBy = doctor.Name;  // Cập nhật tên bác sĩ
                }
                else
                {
                    prescription.UpdateBy = "Bác sĩ không xác định";  // Nếu không tìm thấy bác sĩ
                }

                // Cập nhật trạng thái của tất cả PrescriptionDetail thành "Dispensed"
                var prescriptionDetails = await _context.PrescriptionDetails
                    .Where(pd => pd.PrescriptionId == id)
                    .ToListAsync();

                foreach (var pd in prescriptionDetails)
                {
                    pd.Status = PrescriptionDetailStatus.Dispensed;  // Cập nhật trạng thái của PrescriptionDetail
                }

                // Cập nhật kho thuốc chỉ khi trạng thái là Dispensed
                foreach (var pd in prescriptionDetails)
                {
                    var medicine = await _context.Medicine_Inventories
                        .FirstOrDefaultAsync(mi => mi.MedicineId == pd.MedicineId);

                    if (medicine == null || medicine.Quantity < pd.Quantity)
                    {
                        return BadRequest($"Không đủ thuốc trong kho cho thuốc: {pd.MedicineId}");
                    }

                    // Trừ số lượng thuốc trong kho
                    medicine.Quantity -= pd.Quantity;
                }
            }
            // Nếu trạng thái chuyển thành Cancelled thì không trừ thuốc khỏi kho, chỉ cập nhật trạng thái
            else if (request.Status == PrescriptionStatus.Cancelled)
            {
                prescription.Status = PrescriptionStatus.Cancelled;
                prescription.UpdateDate = DateTime.UtcNow;

                // Cập nhật tên bác sĩ vào UpdateBy
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy UserId từ Claims
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == int.Parse(userId));
                if (doctor != null)
                {
                    prescription.UpdateBy = doctor.Name;  // Cập nhật tên bác sĩ
                }
                else
                {
                    prescription.UpdateBy = "Bác sĩ không xác định";  // Nếu không tìm thấy bác sĩ
                }

                // Cập nhật trạng thái của tất cả PrescriptionDetail thành "Cancelled"
                var prescriptionDetails = await _context.PrescriptionDetails
                    .Where(pd => pd.PrescriptionId == id)
                    .ToListAsync();

                foreach (var pd in prescriptionDetails)
                {
                    pd.Status = PrescriptionDetailStatus.Cancelled;  // Cập nhật trạng thái của PrescriptionDetail
                }
            }
            else
            {
                return BadRequest("Trạng thái không hợp lệ cho phép thay đổi.");
            }

            // Lưu tất cả thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Trạng thái đơn thuốc đã được cập nhật thành công và kho thuốc đã được trừ." });
        }



    }
}
