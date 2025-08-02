using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Service;
using System;
using System.Collections.Generic;
using System.Net;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [ApiController]
    [Route("api/medical-record-admin")]
    public class MedicalRecordAdminController : ControllerBase
    {
        private readonly IMedicalRecordAdminService _adminService;
        private readonly ILogger<MedicalRecordAdminController> _logger;

        public MedicalRecordAdminController(
            IMedicalRecordAdminService adminService,
            ILogger<MedicalRecordAdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả Medical Records (Admin)
        /// </summary>
        [HttpGet("GetAll")]
        public IActionResult GetAll([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var records = _adminService.GetAllMedicalRecords(startDate, endDate);
                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách bệnh án");
                return StatusCode(500, "Lỗi máy chủ");
            }
        }


        [HttpGet("GetByDoctorName")]
        public IActionResult GetByDoctorName([FromQuery] string doctorName)
        {
            var result = _adminService.GetMedicalRecordsByDoctorName(doctorName);
            return Ok(result);
        }
    }


}

