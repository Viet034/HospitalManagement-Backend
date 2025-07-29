using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter;
using SWP391_SE1914_ManageHospital.Service;
using Microsoft.Extensions.Logging;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientFilterController : ControllerBase
    {
        private readonly IPatientFilterService _patientService;
        private readonly ILogger<PatientFilterController> _logger;

        public PatientFilterController(IPatientFilterService patientService, ILogger<PatientFilterController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        /// Lấy danh sách lịch khám của bác sĩ theo bộ lọc (lọc theo ngày, tên bệnh nhân, ...)
        [HttpPost("doctor-schedule")]
        public async Task<IActionResult> FilterDoctorSchedule([FromBody] PatientFilter filter)
        {
            try
            {
                _logger.LogInformation("FilterDoctorSchedule called with DoctorId: {DoctorId}, FromDate: {FromDate}, ToDate: {ToDate}, PatientName: {PatientName}",
                    filter.DoctorId, filter.FromDate, filter.ToDate, filter.PatientName);

                var result = await _patientService.FilterScheduleAsync(filter);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException in FilterDoctorSchedule");
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ArgumentException in FilterDoctorSchedule");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in FilterDoctorSchedule");
                return StatusCode(500, new { message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        /// Lấy danh sách lịch khám của bác sĩ trong ngày hiện tại
        [HttpGet("doctor/{doctorId}/today")]
        public async Task<IActionResult> GetTodayScheduleByDoctor(int doctorId)
        {
            try
            {
                _logger.LogInformation("GetTodayScheduleByDoctor called with DoctorId: {DoctorId}", doctorId);
                var result = await _patientService.GetTodayScheduleByDoctorAsync(doctorId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ArgumentException in GetTodayScheduleByDoctor");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetTodayScheduleByDoctor");
                return StatusCode(500, new { message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        /// Lấy toàn bộ lịch khám cho admin (có cả DoctorId, DoctorName)
        [HttpGet("admin/all-schedules")]
        public async Task<IActionResult> GetAllSchedules()
        {
            try
            {
                _logger.LogInformation("GetAllSchedules called by admin");
                var result = await _patientService.GetAllSchedulesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllSchedules");
                return StatusCode(500, new { message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
    }
}