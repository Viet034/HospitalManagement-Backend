using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Service;
using Microsoft.Extensions.Logging;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [ApiController]
    [Route("api/medical-records")]
    public class MedicalRecordListController : ControllerBase
    {
        private readonly IMedicalRecordListService _listService;
        private readonly ILogger<MedicalRecordListController> _logger;

        public MedicalRecordListController(IMedicalRecordListService listService, ILogger<MedicalRecordListController> logger)
        {
            _listService = listService;
            _logger = logger;
        }

        [HttpGet("patient/{patientId}")]
        public IActionResult GetMedicalRecordsByPatientId(int patientId)
        {
            try
            {
                if (patientId <= 0)
                    return BadRequest("ID bệnh nhân không hợp lệ");

                var records = _listService.GetMedicalRecordsByPatientId(patientId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy hồ sơ bệnh án để lấy ID bệnh nhân {PatientId}", patientId);
                return StatusCode(500, "Đã xảy ra lỗi khi lấy danh sách Medical Records");
            }
        }
    }
}