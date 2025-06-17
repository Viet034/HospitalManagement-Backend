using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicalRecord;
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

        /// <summary>
        /// [VIEW DETAIL] Lấy thông tin chi tiết một Medical Record theo ID.
        /// </summary>
        /// <param name="medicalRecordId">ID của Medical Record</param>
        /// <returns>MedicalRecordDetailResponse</returns>
        [HttpGet("{medicalRecord-View}")]
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

        /// <summary>
        /// [CREATE] Tạo mới một Medical Record.
        /// </summary>
        /// <param name="request">Thông tin Medical Record cần tạo</param>
        /// <returns>MedicalRecordDetailResponse</returns>
        [HttpPost("{medicalRecord-Create}")]
        public IActionResult CreateMedicalRecord([FromBody] MedicalRecordCreateRequest request)
        {
            try
            {
                var result = _detailService.CreateMedicalRecord(request);
                return CreatedAtAction(nameof(GetMedicalRecordDetail), new { medicalRecordId = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi tạo mới Medical Record.");
                return StatusCode(500, "Đã xảy ra lỗi khi tạo mới Medical Record");
            }
        }

        /// <summary>
        /// [UPDATE] Cập nhật thông tin một Medical Record theo ID.
        /// </summary>
        /// <param name="medicalRecordId">ID của Medical Record cần cập nhật</param>
        /// <param name="request">Thông tin cập nhật</param>
        /// <returns>MedicalRecordDetailResponse</returns>
        [HttpPut("{medicalRecord-Update}")]
        public IActionResult UpdateMedicalRecord(int medicalRecordId, [FromBody] MedicalRecordUpdateRequest request)
        {
            try
            {
                var result = _detailService.UpdateMedicalRecord(medicalRecordId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật Medical Record.");
                return StatusCode(500, "Đã xảy ra lỗi khi cập nhật Medical Record");
            }
        }

        /// <summary>
        /// [DELETE] Xóa một Medical Record theo ID.
        /// </summary>
        /// <param name="medicalRecordId">ID của Medical Record cần xóa</param>
        /// <returns>NoContent nếu xóa thành công</returns>
        [HttpDelete("{medicalRecord-Delete}")]
        public IActionResult DeleteMedicalRecord(int medicalRecordId)
        {
            try
            {
                var success = _detailService.DeleteMedicalRecord(medicalRecordId);
                if (!success)
                    return NotFound($"Không tìm thấy Medical Record với ID: {medicalRecordId}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi xóa Medical Record.");
                return StatusCode(500, "Đã xảy ra lỗi khi xóa Medical Record");
            }
        }
    }
}