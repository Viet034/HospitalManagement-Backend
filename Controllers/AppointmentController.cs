using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Service;
using System.Net;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IClinicService _clinicService;
    private readonly IDoctorService _doctorService;
    private readonly IServiceService _serviceService;
    private readonly ApplicationDBContext _context;

    public AppointmentController(IClinicService clinicService, IDoctorService doctorService, IServiceService serviceService, ApplicationDBContext context)
    {
        _clinicService = clinicService;
        _doctorService = doctorService;
        _serviceService = serviceService;
        _context = context;
    }

    /// <summary>
    /// Lấy danh sách phòng khám đang hoạt động (status = 0) để đặt lịch hẹn
    /// </summary>
    /// <returns>Danh sách phòng khám có thể đặt lịch</returns>
    [HttpGet("clinics")]
    [ProducesResponseType(typeof(IEnumerable<ClinicResponseDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetActiveClinics()
    {
        try
        {
            var clinics = await _clinicService.GetActiveClinicsForAppointmentAsync();
            return Ok(new
            {
                success = true,
                message = "Lấy danh sách phòng khám thành công",
                data = clinics
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                data = (object)null
            });
        }
    }

    /// <summary>
    /// Lấy danh sách bác sĩ đang hoạt động trong phòng khám để đặt lịch hẹn
    /// </summary>
    /// <param name="clinicId">ID phòng khám</param>
    /// <returns>Danh sách bác sĩ có thể đặt lịch</returns>
    [HttpGet("doctors/{clinicId}")]
    [ProducesResponseType(typeof(IEnumerable<DoctorResponseDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetDoctorsByClinic(int clinicId)
    {
        try
        {
            var doctors = await _doctorService.GetDoctorsByClinicIdAsync(clinicId);
            return Ok(new
            {
                success = true,
                message = "Lấy danh sách bác sĩ thành công",
                data = doctors
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                data = (object)null
            });
        }
    }

    /// <summary>
    /// Tìm kiếm phòng khám theo tên cho đặt lịch hẹn
    /// </summary>
    /// <param name="name">Tên phòng khám cần tìm</param>
    /// <returns>Danh sách phòng khám phù hợp</returns>
    [HttpGet("clinics/search")]
    [ProducesResponseType(typeof(IEnumerable<ClinicResponseDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SearchClinics([FromQuery] string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Tên phòng khám không được để trống",
                    data = (object)null
                });
            }

            var clinics = await _clinicService.SearchClinicByKeyAsync(name);
            return Ok(new
            {
                success = true,
                message = "Tìm kiếm phòng khám thành công",
                data = clinics
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                data = (object)null
            });
        }
    }

    /// <summary>
    /// Lấy thông tin chi tiết phòng khám theo ID
    /// </summary>
    /// <param name="id">ID phòng khám</param>
    /// <returns>Thông tin chi tiết phòng khám</returns>
    [HttpGet("clinics/{id}")]
    [ProducesResponseType(typeof(ClinicResponseDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetClinicById(int id)
    {
        try
        {
            var clinic = await _clinicService.FindClinicByIdAsync(id);
            return Ok(new
            {
                success = true,
                message = "Lấy thông tin phòng khám thành công",
                data = clinic
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                data = (object)null
            });
        }
    }

    [HttpGet("services-by-doctor/{doctorId:int}")]
    public async Task<IActionResult> GetServicesByDoctorId(int doctorId)
    {
        try
        {
            var departmentId = await _doctorService.GetDepartmentIdByDoctorIdAsync(doctorId);

            if (departmentId == null)
            {
                return Ok(new
                {
                    success = true,
                    message = "Bác sĩ không thuộc khoa nào, không có dịch vụ.",
                    data = new List<ServiceResponseDTO>()
                });
            }

            var services = await _serviceService.GetServicesByDepartmentAsync(departmentId.Value);

            return Ok(new
            {
                success = true,
                message = "Lấy danh sách dịch vụ thành công",
                data = services
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                data = (object)null
            });
        }
    }

    [HttpGet("available-shifts")]
    public async Task<IActionResult> GetAvailableShifts([FromQuery] int doctorId, [FromQuery] DateTime date)
    {
        // Định nghĩa thời gian ca sáng và chiều
        var morningStart = new TimeSpan(7, 30, 0);
        var morningEnd = new TimeSpan(12, 0, 0);
        var afternoonStart = new TimeSpan(13, 0, 0);
        var afternoonEnd = new TimeSpan(17, 30, 0);

        // Lấy thứ trong tuần từ ngày được chọn (1=Monday, 2=Tuesday, ..., 7=Sunday)
        // Trong C#, DayOfWeek.Monday = 1, nhưng cần chuyển đổi vì database dùng 1=Monday
        int dayOfWeek = (int)date.DayOfWeek;
        // Chuyển đổi: Sunday(0) -> 7, Monday(1) -> 1, Tuesday(2) -> 2, ..., Saturday(6) -> 6
        if (dayOfWeek == 0) dayOfWeek = 7; // Sunday

        // Kiểm tra bác sĩ có làm ca sáng không
        var morningShift = await _context.Doctor_Shifts.FirstOrDefaultAsync(s => 
            s.DoctorId == doctorId && 
            s.DayOfWeek == dayOfWeek && 
            s.ShiftType.ToLower() == "morning" &&
            s.IsActive);

        var afternoonShift = await _context.Doctor_Shifts.FirstOrDefaultAsync(s => 
            s.DoctorId == doctorId && 
            s.DayOfWeek == dayOfWeek && 
            s.ShiftType.ToLower() == "afternoon" &&
            s.IsActive);

        // Đếm số lượng appointment đã đặt cho từng ca
        int morningCount = await _context.Appointments
            .Where(a => a.AppointmentDate.Date == date.Date
                && a.StartTime.TimeOfDay == morningStart
                && a.EndTime.TimeOfDay == morningEnd
                && a.Doctor_Appointments.Any(da => da.DoctorId == doctorId))
            .CountAsync();

        int afternoonCount = await _context.Appointments
            .Where(a => a.AppointmentDate.Date == date.Date
                && a.StartTime.TimeOfDay == afternoonStart
                && a.EndTime.TimeOfDay == afternoonEnd
                && a.Doctor_Appointments.Any(da => da.DoctorId == doctorId))
            .CountAsync();

        return Ok(new
        {
            morning = new
            {
                available = morningShift != null && morningCount < 10,
                count = morningCount,
                doctorWorks = morningShift != null
            },
            afternoon = new
            {
                available = afternoonShift != null && afternoonCount < 10,
                count = afternoonCount,
                doctorWorks = afternoonShift != null
            }
        });
    }

    [Authorize]
    [HttpGet("user-info")]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
        if (user == null) return NotFound();
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user.Id);
        return Ok(new
        {
            success = true,
            data = new
            {
                id = user.Id,
                email = user.Email,
                name = patient?.Name,
                phone = patient?.Phone,
                gender = patient?.Gender
            }
        });
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAppointment([FromBody] SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment.AppointmentCreateRequest request)
    {
        // Lấy userId từ token
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
        if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

        // Lấy patient theo userId
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patient == null)
            return BadRequest(new { success = false, message = "Không tìm thấy thông tin bệnh nhân!" });

        // Cập nhật thông tin bệnh nhân nếu có thay đổi
        var info = request.PatientInfo;
        bool isChanged = false;
        if (info != null)
        {
            if (patient.Name != info.Name) { patient.Name = info.Name; isChanged = true; }
            if (patient.Phone != info.Phone) { patient.Phone = info.Phone; isChanged = true; }
            if (patient.Gender.ToString() != info.Gender) { if (Enum.TryParse(info.Gender, out Gender g)) { patient.Gender = g; isChanged = true; } }
            if (patient.Dob != info.Dob) { patient.Dob = info.Dob; isChanged = true; }
            if (patient.CCCD != info.CCCD) { patient.CCCD = info.CCCD; isChanged = true; }
            if (patient.Address != info.Address) { patient.Address = info.Address; isChanged = true; }
            if (patient.InsuranceNumber != info.InsuranceNumber) { patient.InsuranceNumber = info.InsuranceNumber; isChanged = true; }
            if (patient.Allergies != info.Allergies) { patient.Allergies = info.Allergies; isChanged = true; }
            if (patient.BloodType != info.BloodType) { patient.BloodType = info.BloodType; isChanged = true; }
            if (isChanged) _context.Patients.Update(patient);
        }

        // Xác định thời gian bắt đầu/kết thúc theo ca
        TimeSpan start, end;
        if (request.Shift.ToLower() == "morning")
        {
            start = new TimeSpan(7, 30, 0);
            end = new TimeSpan(12, 0, 0);
        }
        else if (request.Shift.ToLower() == "afternoon")
        {
            start = new TimeSpan(13, 0, 0);
            end = new TimeSpan(17, 30, 0);
        }
        else
        {
            return BadRequest(new { success = false, message = "Ca làm việc không hợp lệ!" });
        }

        // Tạo mới appointment
        var appointment = new SWP391_SE1914_ManageHospital.Models.Entities.Appointment
        {
            AppointmentDate = request.AppointmentDate.Date,
            StartTime = request.AppointmentDate.Date + start,
            EndTime = request.AppointmentDate.Date + end,
            Status = SWP391_SE1914_ManageHospital.Ultility.Status.AppointmentStatus.Scheduled,
            Note = request.Note,
            isSend = false,
            PatientId = patient.Id,
            ClinicId = request.ClinicId,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            CreateBy = patient.Name,
            UpdateBy = patient.Name
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(); // Để lấy Id appointment

        // Liên kết với doctor
        var doctorAppointment = new SWP391_SE1914_ManageHospital.Models.Entities.Doctor_Appointment
        {
            DoctorId = request.DoctorId,
            AppointmentId = appointment.Id,
            Status = SWP391_SE1914_ManageHospital.Ultility.Status.DoctorAppointmentStatus.Assigned
        };
        _context.Doctor_Appointments.Add(doctorAppointment);

        // TODO: Nếu có liên kết với service, hãy tạo bảng Appointment_Service nếu có, hoặc lưu serviceId vào appointment nếu thiết kế cho phép
        // (Ở đây chỉ lưu thông tin serviceId vào note nếu không có bảng liên kết)
        appointment.Note = (appointment.Note ?? "") + $"\nServiceId: {request.ServiceId}";

        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Đặt lịch thành công!", data = new { appointmentId = appointment.Id } });
    }
} 