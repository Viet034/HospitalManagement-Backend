using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [ApiController]
    [Route("api/medical-record-details")]
    public class MedicalRecordDetailController : ControllerBase
    {
        private readonly IMedicalRecordDetailService _detailService;
        private readonly ILogger<MedicalRecordDetailController> _logger;

        public MedicalRecordDetailController(IMedicalRecordDetailService detailService, ILogger<MedicalRecordDetailController> logger)
        {
            _detailService = detailService;
            _logger = logger;
        }

        [HttpGet("{medicalRecordId}")]
        public IActionResult GetMedicalRecordDetail(int medicalRecordId)
        {
            try
            {
                if (medicalRecordId <= 0)
                    return BadRequest("ID Medical Record không hợp lệ");

                var detail = _detailService.GetMedicalRecordDetail(medicalRecordId);
                if (detail == null)
                    return NotFound($"Không tìm thấy Medical Record với ID: {medicalRecordId}");

                return Ok(detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin chi tiết Medical Record.");
                return StatusCode(500, "Đã xảy ra lỗi khi lấy chi tiết Medical Record");
            }
        }
    }
}