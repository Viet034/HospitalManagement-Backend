using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientFilterController : ControllerBase
    {
        private readonly IPatientFilterService _patientService;

        public PatientFilterController(IPatientFilterService patientService)
        {
            _patientService = patientService;
        }

        /// Lấy danh sách bệnh nhân của bác sĩ, có thể lọc theo ngày, giờ, tên bệnh nhân.
        [HttpPost("doctor-patients")]
        public async Task<IActionResult> GetPatientsByDoctor([FromBody] PatientFilter filter)
        {
            try
            {
                var result = await _patientService.GetPatientsByDoctorAsync(filter);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Có lỗi xảy ra khi xử lý yêu cầu.");
            }
        }

        /// Lấy danh sách bệnh nhân của bác sĩ trong ngày hiện tại.
        [HttpGet("doctor/{doctorId}/today")]
        public async Task<IActionResult> GetTodayPatientsByDoctor(int doctorId)
        {
            try
            {
                var result = await _patientService.GetTodayPatientsByDoctorAsync(doctorId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Có lỗi xảy ra khi xử lý yêu cầu.");
            }
        }
    }
}