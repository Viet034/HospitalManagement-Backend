using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter;
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Service.Impl;

namespace SWP391_SE1914_ManageHospital.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientFilterController : ControllerBase
{
    private readonly IPatientFilterService _patientService;

    public PatientFilterController(IPatientFilterService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Lấy danh sách bệnh nhân của bác sĩ, có thể lọc theo ngày, giờ, tên bệnh nhân.
    /// </summary>
    /// <param name="filter">Bộ lọc: DoctorId, FromDate, ToDate, PatientName</param>
    /// <returns>Danh sách bệnh nhân</returns>
    [HttpPost("doctor-patients")]
    public async Task<IActionResult> GetPatientsByDoctor([FromBody] PatientFilter filter)
    {
        if (filter == null || filter.DoctorId <= 0)
            return BadRequest("Thiếu thông tin DoctorId hoặc filter không hợp lệ.");

        var result = await _patientService.GetPatientsByDoctorAsync(filter);
        return Ok(result);
    }
}