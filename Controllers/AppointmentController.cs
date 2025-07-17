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
    public async Task<IActionResult> GetActiveClinics([FromQuery] DateTime date)
    {
        try
        {
            var clinics = await _clinicService.GetActiveClinicsForAppointmentAsync(date);
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
    /// Lấy danh sách bác sĩ theo phòng khám cho đặt lịch hẹn
    /// </summary>
    /// <param name="clinicId">ID phòng khám</param>
    /// <param name="date">Ngày đặt lịch</param>
    /// <returns>Danh sách bác sĩ có thể đặt lịch</returns>
    [HttpGet("doctors/{clinicId}")]
    [ProducesResponseType(typeof(IEnumerable<DoctorResponseDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetDoctorsByClinic(int clinicId, [FromQuery] DateTime date)
    {
        try
        {
            var doctors = await _doctorService.GetDoctorsByClinicIdAsync(clinicId, date);
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
                && a.StartTime == morningStart
                && (a.EndTime.HasValue && a.EndTime.Value == morningEnd)
                && a.Doctor_Appointments.Any(da => da.DoctorId == doctorId))
            .CountAsync();

        int afternoonCount = await _context.Appointments
            .Where(a => a.AppointmentDate.Date == date.Date
                && a.StartTime == afternoonStart
                && (a.EndTime.HasValue && a.EndTime.Value == afternoonEnd)
                && a.Doctor_Appointments.Any(da => da.DoctorId == doctorId))
            .CountAsync();

        return Ok(new
        {
            morning = new
            {
                available = morningShift != null && morningCount < 5,
                count = morningCount,
                doctorWorks = morningShift != null
            },
            afternoon = new
            {
                available = afternoonShift != null && afternoonCount < 5,
                count = afternoonCount,
                doctorWorks = afternoonShift != null
            }
        });
    }

    [HttpGet("booked-time-slots")]
    public async Task<IActionResult> GetBookedTimeSlots([FromQuery] int doctorId, [FromQuery] string date)
    {
        // Debug: Log the raw input first
        Console.WriteLine($"=== GetBookedTimeSlots called ===");
        Console.WriteLine($"Raw input - DoctorId: {doctorId}, Date string: '{date}'");
        Console.WriteLine($"Date string length: {date?.Length}");
        if (!string.IsNullOrEmpty(date))
        {
            Console.WriteLine($"Date string bytes: {string.Join(", ", date.Select(c => (int)c))}");
        }

        try
        {
            // Check if date is null or empty
            if (string.IsNullOrEmpty(date))
            {
                Console.WriteLine("Date parameter is null or empty");
                return BadRequest(new
                {
                    success = false,
                    message = "Date parameter is required and cannot be empty",
                    data = (object)null
                });
            }

            // Try multiple date parsing approaches
            DateTime parsedDate = DateTime.MinValue;
            bool parseSuccess = false;

            // Try parsing with different formats
            string[] formats = { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "dd-MM-yyyy", "MM-dd-yyyy" };
            
            Console.WriteLine($"Trying to parse date: '{date}'");
            foreach (string format in formats)
            {
                Console.WriteLine($"Trying format: {format}");
                if (DateTime.TryParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    Console.WriteLine($"Successfully parsed with format: {format}");
                    parseSuccess = true;
                    break;
                }
            }

            // If specific formats failed, try general parsing
            if (!parseSuccess)
            {
                Console.WriteLine("Trying general DateTime.TryParse");
                if (DateTime.TryParse(date, out parsedDate))
                {
                    Console.WriteLine("Successfully parsed with general DateTime.TryParse");
                    parseSuccess = true;
                }
            }

            if (!parseSuccess)
            {
                Console.WriteLine($"Failed to parse date: '{date}'");
                return BadRequest(new
                {
                    success = false,
                    message = $"Invalid date format: '{date}'. Expected format: yyyy-MM-dd",
                    data = (object)null
                });
            }

            // Debug log
            Console.WriteLine($"DoctorId: {doctorId}, Date string: '{date}', Parsed date: {parsedDate:yyyy-MM-dd}");

            // Lấy tất cả các time slot đã được đặt cho bác sĩ trong ngày cụ thể
            Console.WriteLine($"Querying appointments for date: {parsedDate:yyyy-MM-dd}");
            
            // Debug: Kiểm tra tất cả appointments trong ngày
            var allAppointmentsInDay = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == parsedDate.Date)
                .ToListAsync();
            Console.WriteLine($"Total appointments on {parsedDate:yyyy-MM-dd}: {allAppointmentsInDay.Count}");
            
            foreach (var apt in allAppointmentsInDay)
            {
                Console.WriteLine($"Appointment ID: {apt.Id}, Date: {apt.AppointmentDate:yyyy-MM-dd}, StartTime: {apt.StartTime}");
            }
            
            // Debug: Kiểm tra Doctor_Appointments
            var doctorAppointments = await _context.Doctor_Appointments
                .Where(da => da.DoctorId == doctorId)
                .ToListAsync();
            Console.WriteLine($"Total doctor appointments for doctor {doctorId}: {doctorAppointments.Count}");
            
            foreach (var da in doctorAppointments)
            {
                Console.WriteLine($"Doctor_Appointment - DoctorId: {da.DoctorId}, AppointmentId: {da.AppointmentId}");
            }
            
            var bookedSlots = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == parsedDate.Date
                    && a.Doctor_Appointments.Any(da => da.DoctorId == doctorId))
                .Select(a => new
                {
                    startTime = a.StartTime,
                    endTime = a.EndTime
                })
                .ToListAsync();

            Console.WriteLine($"Found {bookedSlots.Count} booked slots");

            // Tạo danh sách các time slot đã được đặt với format HH:mm
            var bookedTimeSlots = new List<string>();
            foreach (var slot in bookedSlots)
            {
                try
                {
                    // Format TimeSpan thành HH:mm
                    var timeString = $"{slot.startTime.Hours:D2}:{slot.startTime.Minutes:D2}";
                    bookedTimeSlots.Add(timeString);
                    Console.WriteLine($"Added time slot: {timeString}");
                }
                catch (Exception timeEx)
                {
                    Console.WriteLine($"Error formatting time slot: {slot.startTime}, Error: {timeEx.Message}");
                }
            }

            // Debug log
            Console.WriteLine($"Booked slots: {string.Join(", ", bookedTimeSlots)}");
            Console.WriteLine($"=== GetBookedTimeSlots completed successfully ===");

            return Ok(new
            {
                success = true,
                message = "Lấy danh sách slot đã đặt thành công",
                data = bookedTimeSlots
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"=== ERROR in GetBookedTimeSlots ===");
            Console.WriteLine($"Error message: {ex.Message}");
            Console.WriteLine($"Error type: {ex.GetType().Name}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            
            // Return a more specific error message
            string errorMessage = ex.Message;
            if (ex.Message.Contains("Input string was not in a correct format"))
            {
                errorMessage = $"Lỗi định dạng ngày tháng: '{date}'. Vui lòng kiểm tra lại định dạng yyyy-MM-dd";
            }
            
            return BadRequest(new
            {
                success = false,
                message = errorMessage,
                data = (object)null
            });
        }
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
        var userData = new
        {
            id = user.Id,
            email = user.Email,
            name = patient?.Name,
            phone = patient?.Phone,
            gender = patient?.Gender,
            dob = patient?.Dob.ToString("yyyy-MM-dd"),
            cccd = patient?.CCCD,
            address = patient?.Address
        };

        return Ok(new
        {
            success = true,
            data = userData
        });
    }

    /// <summary>
    /// Tạo lịch hẹn mới
    /// </summary>
    /// <param name="request">Thông tin lịch hẹn</param>
    /// <returns>Thông tin lịch hẹn đã tạo</returns>
    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateAppointment([FromBody] SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment.AppointmentCreateRequest request)
    {
        try
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Dữ liệu yêu cầu không hợp lệ!" });
            if (request.AppointmentDate.Date < DateTime.Now.Date)
                return BadRequest(new { success = false, message = "Không thể đặt lịch cho ngày trong quá khứ!" });

            // Lấy userId từ token
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) 
                return Unauthorized(new { success = false, message = "Vui lòng đăng nhập để đặt lịch!" });
            if (!int.TryParse(userIdStr, out int userId)) 
                return Unauthorized(new { success = false, message = "Token không hợp lệ!" });

            // Lấy patient theo userId
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);

            // --- Thay đổi logic kiểm tra và thêm người thân ---
            string cccd = request.PatientInfo?.CCCD?.Trim();
            string phone = request.PatientInfo?.Phone?.Trim();
            // Parse ngày sinh từ string với nhiều format
            DateTime dob = DateTime.MinValue;
            string dobStr = null;
            bool dobParseSuccess = false;
            
            // Debug logging
            Console.WriteLine($"=== DEBUG DOB PARSING ===");
            Console.WriteLine($"PatientInfo.Dob type: {(request.PatientInfo?.Dob != null ? request.PatientInfo.Dob.GetType().Name : "null")}");
            Console.WriteLine($"PatientInfo.Dob value: {request.PatientInfo?.Dob}");
            
            if (request.PatientInfo != null)
            {
                object dobObj = request.PatientInfo.Dob;
                Console.WriteLine($"dobObj type: {dobObj?.GetType().Name}");
                Console.WriteLine($"dobObj value: {dobObj}");
                
                if (dobObj is DateTime dt)
                {
                    dobStr = dt.ToString("yyyy-MM-dd");
                    Console.WriteLine($"Converted DateTime to string: {dobStr}");
                }
                else if (dobObj is string dobString)
                {
                    dobStr = dobString;
                    Console.WriteLine($"Using string directly: {dobStr}");
                }
                else
                {
                    dobStr = null;
                    Console.WriteLine($"dobObj is null or unknown type");
                }
                
                if (!string.IsNullOrEmpty(dobStr))
                {
                    string[] formats = { "yyyy-MM-dd'T'HH:mm:ss", "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" };
                    Console.WriteLine($"Trying to parse: '{dobStr}' with formats: {string.Join(", ", formats)}");
                    dobParseSuccess = DateTime.TryParseExact(dobStr, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dob);
                    Console.WriteLine($"Parse success: {dobParseSuccess}, Result: {dob:yyyy-MM-dd}");
                }
                else
                {
                    Console.WriteLine("dobStr is null or empty");
                }
            }
            else
            {
                Console.WriteLine("PatientInfo is null");
            }
            
            if (!dobParseSuccess || dob == DateTime.MinValue)
            {
                Console.WriteLine($"=== DOB PARSING FAILED ===");
                Console.WriteLine($"dobParseSuccess: {dobParseSuccess}");
                Console.WriteLine($"dob: {dob:yyyy-MM-dd}");
                Console.WriteLine($"dob == DateTime.MinValue: {dob == DateTime.MinValue}");
                return BadRequest(new { success = false, message = "Ngày sinh không hợp lệ! Vui lòng nhập đúng định dạng yyyy-MM-dd hoặc dd/MM/yyyy." });
            }
            
            Console.WriteLine($"=== DOB PARSING SUCCESS ===");
            Console.WriteLine($"Final dob: {dob:yyyy-MM-dd}");
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.CCCD == cccd || p.Phone == phone);
            if (existingPatient != null)
            {
                await UpdatePatientInfoIfChanged(existingPatient, request.PatientInfo);
                patient = existingPatient;
                }
            else
            {
                var newPatient = new Patient
                {
                    Name = request.PatientInfo.Name,
                    Phone = phone,
                    Gender = Enum.TryParse<Gender>(request.PatientInfo.Gender, out var g) ? g : Gender.Male,
                    Dob = dob,
                    CCCD = cccd,
                    Address = string.IsNullOrEmpty(request.PatientInfo.Address) ? string.Empty : request.PatientInfo.Address,
                    InsuranceNumber = string.IsNullOrEmpty(request.PatientInfo.InsuranceNumber) ? null : request.PatientInfo.InsuranceNumber,
                    Allergies = string.IsNullOrEmpty(request.PatientInfo.Allergies) ? null : request.PatientInfo.Allergies,
                    BloodType = string.IsNullOrEmpty(request.PatientInfo.BloodType) ? null : request.PatientInfo.BloodType,
                    ImageURL = request.PatientInfo.ImageURL ?? null,
                    Status = PatientStatus.Active,
                    UserId = userId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CreateBy = patient?.Name ?? "User",
                    UpdateBy = patient?.Name ?? "User",
                    Code = $"PAT-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
                };
                _context.Patients.Add(newPatient);
                await _context.SaveChangesAsync();
                patient = newPatient;
            }

            // Kiểm tra clinic có tồn tại và hoạt động
            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Id == request.ClinicId && c.Status == ClinicStatus.Available);
            if (clinic == null)
                return BadRequest(new { success = false, message = "Phòng khám không tồn tại hoặc không hoạt động!" });

            // Kiểm tra doctor có tồn tại và làm việc tại clinic
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == request.DoctorId && d.ClinicId == request.ClinicId);
            if (doctor == null)
                return BadRequest(new { success = false, message = "Bác sĩ không làm việc tại phòng khám này!" });

            // Kiểm tra service có tồn tại
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId);
            if (service == null)
                return BadRequest(new { success = false, message = "Dịch vụ không tồn tại!" });

            // Xác định thời gian bắt đầu/kết thúc theo ca
            TimeSpan start, end;
            if (request.StartTime >= new TimeSpan(7, 0, 0) && request.StartTime < new TimeSpan(12, 0, 0))
            {
                start = new TimeSpan(7, 30, 0);
                end = new TimeSpan(12, 0, 0);
            }
            else if (request.StartTime >= new TimeSpan(13, 0, 0) && request.StartTime < new TimeSpan(17, 30, 0))
            {
                start = new TimeSpan(13, 0, 0);
                end = new TimeSpan(17, 30, 0);
            }
            else
            {
                return BadRequest(new { success = false, message = "Ca làm việc không hợp lệ! Chỉ chấp nhận từ 07:00-12:00 hoặc 13:00-17:30" });
            }

            // Kiểm tra bác sĩ có làm việc trong ca này không
            int dayOfWeek = (int)request.AppointmentDate.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7; // Sunday
            var doctorShift = await _context.Doctor_Shifts.FirstOrDefaultAsync(s => 
                s.DoctorId == request.DoctorId && 
                s.DayOfWeek == dayOfWeek && 
                s.ShiftType.ToLower() == (request.StartTime >= new TimeSpan(7, 0, 0) && request.StartTime < new TimeSpan(12, 0, 0) ? "morning" : "afternoon") &&
                s.IsActive &&
                s.StartTime <= request.StartTime &&
                s.EndTime > request.StartTime
            );
            if (doctorShift == null)
                return BadRequest(new { success = false, message = $"Bác sĩ không làm việc trong ca {(request.StartTime >= new TimeSpan(7, 0, 0) && request.StartTime < new TimeSpan(12, 0, 0) ? "sáng" : "chiều")} vào ngày này!" });

            // Kiểm tra số lượng appointment trong ca
            int appointmentCount = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == request.AppointmentDate.Date
                    && a.StartTime == start
                    && (a.EndTime.HasValue && a.EndTime.Value == end)
                    && a.Doctor_Appointments.Any(da => da.DoctorId == request.DoctorId))
                .CountAsync();
            if (appointmentCount >= 5)
                return BadRequest(new { success = false, message = "Ca làm việc này đã đầy! Vui lòng chọn ca khác." });

            // Kiểm tra trùng lịch với StartTime cụ thể
            var existingAppointment = await _context.Appointments
                .Where(a => a.ClinicId == request.ClinicId
                    && a.AppointmentDate == request.AppointmentDate.Date
                    && a.StartTime == request.StartTime)
                .Join(_context.Doctor_Appointments,
                    a => a.Id,
                    da => da.AppointmentId,
                    (a, da) => new { a, da })
                .FirstOrDefaultAsync(x => x.da.DoctorId == request.DoctorId);
            if (existingAppointment != null)
                return BadRequest(new { success = false, message = "Khung giờ này đã có người đặt! Vui lòng chọn giờ khác." });

            // Tạo mã lịch hẹn tự động
            string appointmentCode = await GenerateAppointmentCode();

            // 1. Tạo invoice trước
            var invoice = new SWP391_SE1914_ManageHospital.Models.Entities.Invoice
            {
                AppointmentId = 0, // tạm thời, sẽ cập nhật sau
                PatientId = patient.Id,
                InsuranceId = null, // Không gán mặc định 1, để null
                InitialAmount = 0,
                DiscountAmount = 0,
                TotalAmount = 0,
                Notes = "",
                Status = SWP391_SE1914_ManageHospital.Ultility.Status.InvoiceStatus.Unpaid,
                Name = $"Invoice - {patient.Name}",
                Code = $"INV-{Guid.NewGuid()}",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateBy = patient.Name ?? "Patient",
                UpdateBy = patient.Name ?? "Patient"
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // 2. Tạo appointment với Id = invoice.Id
            var appointment = new SWP391_SE1914_ManageHospital.Models.Entities.Appointment
            {
                Id = invoice.Id,
                Name = $"Lịch hẹn - {patient.Name}",
                Code = appointmentCode,
                AppointmentDate = request.AppointmentDate.Date,
                StartTime = request.StartTime,
                EndTime = null,
                Status = SWP391_SE1914_ManageHospital.Ultility.Status.AppointmentStatus.Scheduled,
                Note = !string.IsNullOrEmpty(request.Note) ? request.Note : $"Dịch vụ: {service.Name}",
                isSend = false,
                PatientId = patient.Id,
                ClinicId = request.ClinicId,
                ReceptionId = null,
                ServiceId = request.ServiceId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateBy = patient.Name ?? "Patient",
                UpdateBy = patient.Name ?? "Patient"
            };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // 3. Tạo medical_record với AppointmentId = appointment.Id (không gán Id = appointment.Id)
            var medicalRecord = new SWP391_SE1914_ManageHospital.Models.Entities.Medical_Record
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = request.DoctorId,
                PrescriptionId = null, // Cho phép null
                DiseaseId = null, // Cho phép null
                Status = SWP391_SE1914_ManageHospital.Ultility.Status.MedicalRecordStatus.Open,
                Diagnosis = "",
                TestResults = "",
                Notes = "",
                Name = $"MedicalRecord - {appointment.Name}",
                Code = $"MR-{appointment.Code}",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateBy = appointment.CreateBy,
                UpdateBy = appointment.UpdateBy
            };
            _context.Medical_Records.Add(medicalRecord);
            await _context.SaveChangesAsync();

            // Cập nhật lại AppointmentId cho invoice nếu cần
            invoice.AppointmentId = appointment.Id;
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();

            // Liên kết với doctor
            var doctorAppointment = new SWP391_SE1914_ManageHospital.Models.Entities.Doctor_Appointment
            {
                DoctorId = request.DoctorId,
                AppointmentId = appointment.Id,
                Status = SWP391_SE1914_ManageHospital.Ultility.Status.DoctorAppointmentStatus.Assigned
            };
            _context.Doctor_Appointments.Add(doctorAppointment);
            await _context.SaveChangesAsync();

            // Trả về thông tin lịch hẹn đã tạo (fix triệt để null và lỗi format)
            var responseData = new
            {
                appointmentId = appointment?.Id ?? 0,
                appointmentCode = appointment?.Code ?? string.Empty,
                appointmentDate = appointment?.AppointmentDate != null && appointment.AppointmentDate != DateTime.MinValue
                    ? appointment.AppointmentDate.ToString("yyyy-MM-dd") : string.Empty,
                startTime = appointment?.StartTime != null ? SafeTimeSpanToString(appointment.StartTime) : string.Empty,
                endTime = appointment?.EndTime.HasValue == true ? SafeTimeSpanToString(appointment.EndTime.Value) : null,
                shift = request.StartTime >= new TimeSpan(7, 0, 0) && request.StartTime < new TimeSpan(12, 0, 0) ? "morning" : "afternoon",
                clinic = clinic != null ? new { id = clinic.Id, name = clinic.Name ?? "", address = clinic.Address ?? "" } : new { id = 0, name = "", address = "" },
                doctor = doctor != null ? new { id = doctor.Id, name = doctor.Name ?? "" } : new { id = 0, name = "" },
                service = service != null ? new { id = service.Id, name = service.Name ?? "" } : new { id = 0, name = "" },
                patient = patient != null ? new { id = patient.Id, name = patient.Name ?? "", phone = patient.Phone ?? "" } : new { id = 0, name = "", phone = "" },
                status = appointment?.Status.ToString() ?? string.Empty,
                note = appointment?.Note ?? string.Empty
            };

            return Ok(new 
            { 
                success = true, 
                message = "Đặt lịch hẹn thành công! Vui lòng kiểm tra email để xác nhận.", 
                data = responseData 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                success = false, 
                message = "Đã xảy ra lỗi trong quá trình đặt lịch. Vui lòng thử lại sau!", 
                error = ex.Message,
                inner = ex.InnerException?.Message
            });
        }
    }

    /// <summary>
    /// Cập nhật thông tin bệnh nhân nếu có thay đổi
    /// </summary>
    private async Task UpdatePatientInfoIfChanged(Patient patient, SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment.PatientInfoDto? info)
    {
        if (info == null) return;

        bool isChanged = false;
        
        if (!string.IsNullOrEmpty(info.Name) && patient.Name != info.Name) 
        { 
            patient.Name = info.Name; 
            isChanged = true; 
        }
        
        if (!string.IsNullOrEmpty(info.Phone) && patient.Phone != info.Phone) 
        { 
            patient.Phone = info.Phone; 
            isChanged = true; 
        }
        
        if (!string.IsNullOrEmpty(info.Gender) && patient.Gender.ToString() != info.Gender) 
        { 
            if (Enum.TryParse(info.Gender, out Gender g)) 
            { 
                patient.Gender = g; 
                isChanged = true; 
            } 
        }
        
        if (info.Dob != default && patient.Dob != info.Dob) 
        { 
            patient.Dob = info.Dob; 
            isChanged = true; 
        }
        
        if (!string.IsNullOrEmpty(info.CCCD) && patient.CCCD != info.CCCD) 
        { 
            patient.CCCD = info.CCCD; 
            isChanged = true; 
        }
        
        if (!string.IsNullOrEmpty(info.Address) && patient.Address != info.Address) 
        { 
            patient.Address = info.Address; 
            isChanged = true; 
        }
        
        if (!string.IsNullOrEmpty(info.InsuranceNumber) && patient.InsuranceNumber != info.InsuranceNumber) 
        { 
            patient.InsuranceNumber = info.InsuranceNumber; 
            isChanged = true; 
        }
        
        if (!string.IsNullOrEmpty(info.Allergies) && patient.Allergies != info.Allergies) 
        { 
            patient.Allergies = info.Allergies; 
            isChanged = true; 
        }
        
        if (!string.IsNullOrEmpty(info.BloodType) && patient.BloodType != info.BloodType) 
        { 
            patient.BloodType = info.BloodType; 
            isChanged = true; 
        }

        if (isChanged) 
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Tạo mã lịch hẹn tự động
    /// </summary>
    private async Task<string> GenerateAppointmentCode()
    {
        var today = DateTime.Now.ToString("yyyyMMdd");
        var lastAppointment = await _context.Appointments
            .Where(a => a.Code.StartsWith($"APT{today}"))
            .OrderByDescending(a => a.Code)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastAppointment != null)
        {
            var lastNumberStr = lastAppointment.Code.Substring(11); // Bỏ "APT" + 8 số ngày
            if (int.TryParse(lastNumberStr, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"APT{today}{nextNumber:D3}"; // APT20241201001
    }

    [HttpGet("booked-times")]
    public async Task<IActionResult> GetBookedTimes([FromQuery] int clinicId, [FromQuery] int doctorId, [FromQuery] int serviceId, [FromQuery] DateTime date)
    {
        var booked = await _context.Appointments
            .Where(a => a.ClinicId == clinicId
                && a.AppointmentDate == date.Date)
            .Join(_context.Doctor_Appointments,
                a => a.Id,
                da => da.AppointmentId,
                (a, da) => new { a, da })
            .Where(x => x.da.DoctorId == doctorId)
            .Select(x => x.a.StartTime.ToString(@"hh\:mm"))
            .ToListAsync();
        return Ok(new { success = true, data = booked });
    }

    /// <summary>
    /// Test endpoint tạo appointment không cần authorize - CHỈ DÙNG CHO DEVELOPMENT
    /// </summary>
    [HttpPost("create-test")]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAppointmentTest([FromBody] SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment.AppointmentCreateRequest request, [FromQuery] int? userId = null)
    {
        try
        {
            // Validation input
            if (request == null)
                return BadRequest(new { success = false, message = "Dữ liệu yêu cầu không hợp lệ!" });

            if (request.AppointmentDate.Date < DateTime.Now.Date)
                return BadRequest(new { success = false, message = "Không thể đặt lịch cho ngày trong quá khứ!" });

            // Lấy patient theo userId nếu có, không thì lấy patient đầu tiên
            Patient patient;
            if (userId.HasValue)
            {
                patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId.Value);
                if (patient == null)
                    return BadRequest(new { success = false, message = "Không tìm thấy bệnh nhân nào với userId này!" });
            }
            else
            {
                patient = await _context.Patients.FirstOrDefaultAsync();
                if (patient == null)
                    return BadRequest(new { success = false, message = "Không tìm thấy bệnh nhân nào trong hệ thống để test!" });
            }

            // Thêm logic tạo mới bệnh nhân cho người thân (giống API chính)
            string cccd = request.PatientInfo?.CCCD?.Trim();
            string phone = request.PatientInfo?.Phone?.Trim();
            // Parse ngày sinh từ string với nhiều format
            DateTime dob = DateTime.MinValue;
            string dobStr = null;
            bool dobParseSuccess = false;
            if (request.PatientInfo != null)
            {
                object dobObj = request.PatientInfo.Dob;
                if (dobObj is DateTime dt)
                {
                    dobStr = dt.ToString("yyyy-MM-dd");
                }
                else if (dobObj is string dobString)
                {
                    dobStr = dobString;
                }
                else
                {
                    dobStr = null;
                }
                if (!string.IsNullOrEmpty(dobStr))
                {
                    string[] formats = { "yyyy-MM-dd'T'HH:mm:ss", "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" };
                    dobParseSuccess = DateTime.TryParseExact(dobStr, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dob);
                }
            }
            if (!dobParseSuccess || dob == DateTime.MinValue)
            {
                return BadRequest(new { success = false, message = "Ngày sinh không hợp lệ! Vui lòng nhập đúng định dạng yyyy-MM-dd hoặc dd/MM/yyyy." });
            }
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.CCCD == cccd || p.Phone == phone);
            if (existingPatient != null)
            {
                // Fix dữ liệu tạm thời nếu có trường nào bị null
                existingPatient.Address ??= string.Empty;
                existingPatient.InsuranceNumber ??= null;
                existingPatient.Allergies ??= null;
                existingPatient.BloodType ??= null;
                existingPatient.ImageURL ??= null;
                await _context.SaveChangesAsync();

                // Nếu thông tin bệnh nhân giống hoàn toàn thì không cập nhật lại, chỉ dùng lại bản ghi
                bool isSame = true;
                if (existingPatient.Name != request.PatientInfo.Name) isSame = false;
                if (existingPatient.Phone != phone) isSame = false;
                if (existingPatient.Gender.ToString() != request.PatientInfo.Gender) isSame = false;
                if (existingPatient.Dob != dob) isSame = false;
                if (existingPatient.CCCD != cccd) isSame = false;
                if (existingPatient.Address != (request.PatientInfo.Address ?? string.Empty)) isSame = false;
                if (existingPatient.InsuranceNumber != (request.PatientInfo.InsuranceNumber ?? null)) isSame = false;
                if (existingPatient.Allergies != (request.PatientInfo.Allergies ?? null)) isSame = false;
                if (existingPatient.BloodType != (request.PatientInfo.BloodType ?? null)) isSame = false;
                if (existingPatient.ImageURL != (request.PatientInfo.ImageURL ?? null)) isSame = false;

                if (!isSame)
                {
                    await UpdatePatientInfoIfChanged(existingPatient, request.PatientInfo);
                }
                patient = existingPatient;
            }
            else
            {
                var newPatient = new Patient
                {
                    Name = request.PatientInfo.Name,
                    Phone = phone,
                    Gender = Enum.TryParse<Gender>(request.PatientInfo.Gender, out var g) ? g : Gender.Male,
                    Dob = dob,
                    CCCD = cccd,
                    Address = string.IsNullOrEmpty(request.PatientInfo.Address) ? string.Empty : request.PatientInfo.Address,
                    InsuranceNumber = string.IsNullOrEmpty(request.PatientInfo.InsuranceNumber) ? null : request.PatientInfo.InsuranceNumber,
                    Allergies = string.IsNullOrEmpty(request.PatientInfo.Allergies) ? null : request.PatientInfo.Allergies,
                    BloodType = string.IsNullOrEmpty(request.PatientInfo.BloodType) ? null : request.PatientInfo.BloodType,
                    ImageURL = request.PatientInfo.ImageURL ?? null,
                    Status = PatientStatus.Active,
                    UserId = userId ?? 0,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CreateBy = patient?.Name ?? "User",
                    UpdateBy = patient?.Name ?? "User",
                    Code = $"PAT-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
                };
                _context.Patients.Add(newPatient);
                await _context.SaveChangesAsync();
                patient = newPatient;
            }

            // Kiểm tra clinic có tồn tại và hoạt động
            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Id == request.ClinicId && c.Status == ClinicStatus.Available);
            if (clinic == null)
                return BadRequest(new { success = false, message = "Phòng khám không tồn tại hoặc không hoạt động!" });

            // Kiểm tra doctor có tồn tại và làm việc tại clinic
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == request.DoctorId && d.ClinicId == request.ClinicId);
            if (doctor == null)
                return BadRequest(new { success = false, message = "Bác sĩ không làm việc tại phòng khám này!" });

            // Kiểm tra service có tồn tại
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId);
            if (service == null)
                return BadRequest(new { success = false, message = "Dịch vụ không tồn tại!" });

            // Xác định thời gian bắt đầu/kết thúc theo ca
            TimeSpan start, end;
            if (request.StartTime >= new TimeSpan(7, 0, 0) && request.StartTime < new TimeSpan(12, 0, 0))
            {
                start = new TimeSpan(7, 30, 0);
                end = new TimeSpan(12, 0, 0);
            }
            else if (request.StartTime >= new TimeSpan(13, 0, 0) && request.StartTime < new TimeSpan(17, 30, 0))
            {
                start = new TimeSpan(13, 0, 0);
                end = new TimeSpan(17, 30, 0);
            }
            else
            {
                return BadRequest(new { success = false, message = "Ca làm việc không hợp lệ! Chỉ chấp nhận từ 07:00-12:00 hoặc 13:00-17:30" });
            }

            // Kiểm tra bác sĩ có làm việc trong ca này không
            int dayOfWeek = (int)request.AppointmentDate.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7; // Sunday

            var doctorShift = await _context.Doctor_Shifts.FirstOrDefaultAsync(s => 
                s.DoctorId == request.DoctorId && 
                s.DayOfWeek == dayOfWeek && 
                s.ShiftType.ToLower() == (request.StartTime >= new TimeSpan(7, 0, 0) && request.StartTime < new TimeSpan(12, 0, 0) ? "morning" : "afternoon") &&
                s.IsActive &&
                s.StartTime <= request.StartTime &&
                s.EndTime > request.StartTime
            );

            if (doctorShift == null)
                return BadRequest(new { success = false, message = $"Bác sĩ không làm việc trong ca {(request.StartTime >= new TimeSpan(7, 0, 0) && request.StartTime < new TimeSpan(12, 0, 0) ? "sáng" : "chiều")} vào ngày này!" });

            // Kiểm tra số lượng appointment trong ca
            int appointmentCount = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == request.AppointmentDate.Date
                    && a.StartTime == start
                    && (a.EndTime.HasValue && a.EndTime.Value == end)
                    && a.Doctor_Appointments.Any(da => da.DoctorId == request.DoctorId))
                .CountAsync();

            if (appointmentCount >= 5)
                return BadRequest(new { success = false, message = "Ca làm việc này đã đầy! Vui lòng chọn ca khác." });

            // Kiểm tra trùng lịch với StartTime cụ thể  
            var existingAppointment = await _context.Appointments
                .Where(a => a.ClinicId == request.ClinicId
                    && a.AppointmentDate == request.AppointmentDate.Date
                    && a.StartTime == request.StartTime)
                .Join(_context.Doctor_Appointments,
                    a => a.Id,
                    da => da.AppointmentId,
                    (a, da) => new { a, da })
                .FirstOrDefaultAsync(x => x.da.DoctorId == request.DoctorId);

            if (existingAppointment != null)
                return BadRequest(new { success = false, message = "Khung giờ này đã có người đặt! Vui lòng chọn giờ khác." });

            // Tạo mã lịch hẹn tự động
            string appointmentCode = await GenerateAppointmentCode();

            // 1. Tạo invoice trước
            var invoice = new SWP391_SE1914_ManageHospital.Models.Entities.Invoice
            {
                AppointmentId = 0, // tạm thời, sẽ cập nhật sau
                PatientId = patient.Id,
                InsuranceId = null, // Không gán mặc định 1, để null
                InitialAmount = 0,
                DiscountAmount = 0,
                TotalAmount = 0,
                Notes = "",
                Status = SWP391_SE1914_ManageHospital.Ultility.Status.InvoiceStatus.Unpaid,
                Name = $"Invoice - {patient.Name}",
                Code = $"INV-{Guid.NewGuid()}",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateBy = "TEST_USER",
                UpdateBy = "TEST_USER"
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // 2. Tạo appointment với Id = invoice.Id
            var appointment = new SWP391_SE1914_ManageHospital.Models.Entities.Appointment
            {
                Id = invoice.Id,
                Name = $"Lịch hẹn Test - {patient.Name}",
                Code = appointmentCode,
                AppointmentDate = request.AppointmentDate.Date,
                StartTime = request.StartTime,
                EndTime = null,
                Status = SWP391_SE1914_ManageHospital.Ultility.Status.AppointmentStatus.Scheduled,
                Note = !string.IsNullOrEmpty(request.Note) ? request.Note : $"Dịch vụ: {service.Name}",
                isSend = false,
                PatientId = patient.Id,
                ClinicId = request.ClinicId,
                ReceptionId = 1, // Mặc định
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateBy = "TEST_USER",
                UpdateBy = "TEST_USER"
            };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // 3. Tạo medical_record với Id = appointment.Id
            var medicalRecord = new SWP391_SE1914_ManageHospital.Models.Entities.Medical_Record
            {
                Id = appointment.Id,
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = request.DoctorId,
                PrescriptionId = null, // Cho phép null
                DiseaseId = null, // Cho phép null
                Status = SWP391_SE1914_ManageHospital.Ultility.Status.MedicalRecordStatus.Open,
                Diagnosis = "",
                TestResults = "",
                Notes = "",
                Name = $"MedicalRecord - {appointment.Name}",
                Code = $"MR-{appointment.Code}",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateBy = appointment.CreateBy,
                UpdateBy = appointment.UpdateBy
            };
            _context.Medical_Records.Add(medicalRecord);
            await _context.SaveChangesAsync();

            // Cập nhật lại AppointmentId cho invoice nếu cần
            invoice.AppointmentId = appointment.Id;
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();

            // Liên kết với doctor
            var doctorAppointment = new SWP391_SE1914_ManageHospital.Models.Entities.Doctor_Appointment
            {
                DoctorId = request.DoctorId,
                AppointmentId = appointment.Id,
                Status = SWP391_SE1914_ManageHospital.Ultility.Status.DoctorAppointmentStatus.Assigned
            };
            _context.Doctor_Appointments.Add(doctorAppointment);
            await _context.SaveChangesAsync();

            // Trả về thông tin lịch hẹn đã tạo (fix triệt để null và lỗi format)
            var responseData = new
            {
                appointmentId = appointment?.Id ?? 0,
                appointmentCode = appointment?.Code ?? string.Empty,
                appointmentDate = appointment?.AppointmentDate != null && appointment.AppointmentDate != DateTime.MinValue
                    ? appointment.AppointmentDate.ToString("yyyy-MM-dd") : string.Empty,
                startTime = appointment?.StartTime != null ? SafeTimeSpanToString(appointment.StartTime) : string.Empty,
                endTime = appointment?.EndTime.HasValue == true ? SafeTimeSpanToString(appointment.EndTime.Value) : null,
                shift = request.StartTime >= new TimeSpan(7, 0, 0) && request.StartTime < new TimeSpan(12, 0, 0) ? "morning" : "afternoon",
                clinic = clinic != null ? new { id = clinic.Id, name = clinic.Name ?? "", address = clinic.Address ?? "" } : new { id = 0, name = "", address = "" },
                doctor = doctor != null ? new { id = doctor.Id, name = doctor.Name ?? "" } : new { id = 0, name = "" },
                service = service != null ? new { id = service.Id, name = service.Name ?? "" } : new { id = 0, name = "" },
                patient = patient != null ? new { id = patient.Id, name = patient.Name ?? "", phone = patient.Phone ?? "" } : new { id = 0, name = "", phone = "" },
                status = appointment?.Status.ToString() ?? string.Empty,
                note = appointment?.Note ?? string.Empty,
                testMode = true
            };

            return Ok(new 
            { 
                success = true, 
                message = "[TEST MODE] Đặt lịch hẹn thành công!", 
                data = responseData 
            });
        }
        catch (Exception ex)
        {
            // Log lỗi chi tiết ra console
            Console.WriteLine(ex.ToString());
            if (ex.InnerException != null)
                Console.WriteLine("Inner: " + ex.InnerException.ToString());

            return StatusCode(500, new 
            { 
                success = false, 
                message = "Đã xảy ra lỗi trong quá trình đặt lịch. Vui lòng thử lại sau!", 
                error = ex.Message,
                inner = ex.InnerException?.Message
            });
        }
    }

    [HttpGet("doctor-working-days/{doctorId}")]
    public async Task<IActionResult> GetDoctorWorkingDays(int doctorId, [FromQuery] int year, [FromQuery] int month)
    {
        try
        {
            const int MAX_APPOINTMENTS_PER_DAY = 5;
            
            // Lấy tất cả shifts của doctor
            var doctorShifts = await _context.Doctor_Shifts
                .Where(s => s.DoctorId == doctorId && s.IsActive)
                .ToListAsync();

            if (!doctorShifts.Any())
            {
                return Ok(new
                {
                    success = true,
                    message = "Bác sĩ không có lịch làm việc",
                    data = new List<object>()
                });
            }

            // Lấy các ngày trong tuần mà doctor làm việc
            var workingDaysOfWeek = doctorShifts.Select(s => s.DayOfWeek).Distinct().ToList();

            // Tạo danh sách các ngày trong tháng mà doctor làm việc
            var workingDays = new List<object>();
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var today = DateTime.Today;

            for (var date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                // Chuyển đổi DayOfWeek: Sunday(0) -> 7, Monday(1) -> 1, etc.
                int dayOfWeek = (int)date.DayOfWeek;
                if (dayOfWeek == 0) dayOfWeek = 7; // Sunday

                // Kiểm tra nếu ngày này là ngày làm việc và không phải ngày trong quá khứ
                if (workingDaysOfWeek.Contains(dayOfWeek) && date >= today)
                {
                    // Đếm số lượng appointment đã đặt cho ngày này
                    var appointmentCount = await _context.Doctor_Appointments
                        .Where(da => da.DoctorId == doctorId &&
                                    da.Appointment.AppointmentDate.Date == date.Date)
                        .CountAsync();

                    workingDays.Add(new
                    {
                        day = date.Day,
                        appointmentCount = appointmentCount,
                        isAvailable = appointmentCount < MAX_APPOINTMENTS_PER_DAY,
                        maxSlots = MAX_APPOINTMENTS_PER_DAY
                    });
                }
            }

            return Ok(new
            {
                success = true,
                message = "Lấy lịch làm việc của bác sĩ thành công",
                data = workingDays
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

    [HttpGet("available-time-slots")]
    public async Task<IActionResult> GetAvailableTimeSlots([FromQuery] int doctorId, [FromQuery] string date)
    {
        try
        {
            // Parse date
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid date format. Expected format: yyyy-MM-dd",
                    data = (object)null
                });
            }

            // Lấy thứ trong tuần
            int dayOfWeek = (int)parsedDate.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7; // Sunday

            // Lấy lịch làm việc của bác sĩ cho ngày này
            var doctorShifts = await _context.Doctor_Shifts
                .Where(s => s.DoctorId == doctorId && 
                           s.DayOfWeek == dayOfWeek && 
                           s.IsActive)
                .ToListAsync();

            if (!doctorShifts.Any())
            {
                return Ok(new
                {
                    success = true,
                    message = "Bác sĩ không làm việc vào ngày này",
                    data = new
                    {
                        morning = new List<object>(),
                        afternoon = new List<object>()
                    }
                });
            }

            // Ca sáng: 08:00 - 11:30 (15 phút/slot)
            var morningSlots = new List<string>
            {
                "08:00", "08:15", "08:30", "08:45", "09:00", "09:15", "09:30", "09:45",
                "10:00", "10:15", "10:30", "10:45", "11:00", "11:15", "11:30"
            };
            // Ca chiều:13/slot)
            var afternoonSlots = new List<string>
            {
                "13:15", "13:30", "13:45", "14:00", "14:15", "14:30", "14:45", "15:00",
                "15:15", "15:30", "15:45", "16:00", "16:15", "16:30", "16:45"
            };

            // Lấy các time slots đã được đặt
            var bookedSlots = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == parsedDate.Date
                    && a.Doctor_Appointments.Any(da => da.DoctorId == doctorId))
                .Select(a => a.StartTime)
                .ToListAsync();

            var bookedTimeStrings = bookedSlots.Select(t => $"{t.Hours:D2}:{t.Minutes:D2}").ToList();

            // Kiểm tra lịch làm việc của bác sĩ
            var morningShift = doctorShifts.FirstOrDefault(s => s.ShiftType.ToLower() == "morning");
            var afternoonShift = doctorShifts.FirstOrDefault(s => s.ShiftType.ToLower() == "afternoon");

            // Tạo response cho ca sáng
            var morningResponse = morningSlots.Select(slot =>
            {
                var time = TimeSpan.Parse(slot);
                var isInWorkingHours = morningShift != null &&
                                     time >= morningShift.StartTime &&
                                     time <= morningShift.EndTime;
                var isBooked = bookedTimeStrings.Contains(slot);

                return new
                {
                    time = slot,
                    available = isInWorkingHours && !isBooked,
                    booked = isBooked,
                    disabled = !isInWorkingHours,
                    reason = !isInWorkingHours ? "Ngoài giờ làm việc" :
                             isBooked ? "Đã được đặt" : "Có sẵn"
                };
            }).ToList();

            // Tạo response cho ca chiều
            var afternoonResponse = afternoonSlots.Select(slot =>
            {
                var time = TimeSpan.Parse(slot);
                var isInWorkingHours = afternoonShift != null &&
                                     time >= afternoonShift.StartTime &&
                                     time <= afternoonShift.EndTime;
                var isBooked = bookedTimeStrings.Contains(slot);

                return new
                {
                    time = slot,
                    available = isInWorkingHours && !isBooked,
                    booked = isBooked,
                    disabled = !isInWorkingHours,
                    reason = !isInWorkingHours ? "Ngoài giờ làm việc" :
                             isBooked ? "Đã được đặt" : "Có sẵn"
                };
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "Lấy danh sách time slots thành công",
                data = new
                {
                    morning = morningResponse,
                    afternoon = afternoonResponse,
                    doctorShifts = doctorShifts.Select(s => new
                    {
                        shiftType = s.ShiftType,
                        startTime = s.StartTime.ToString(@"hh\:mm"),
                        endTime = s.EndTime.ToString(@"hh\:mm")
                    }).ToList()
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Đã xảy ra lỗi khi lấy danh sách time slots",
                error = ex.Message
            });
        }
    }

    // Thêm hàm helper an toàn cho TimeSpan
    private string SafeTimeSpanToString(TimeSpan? time)
    {
        if (time == null) return string.Empty;
        try { return time.Value.ToString(@"hh\:mm"); } catch { return string.Empty; }
    }
    private string SafeTimeSpanToString(TimeSpan time)
    {
        try { return time.ToString(@"hh\:mm"); } catch { return string.Empty; }
    }
} 