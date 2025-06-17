using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalRecordController : ControllerBase
{
    private readonly IMedicalRecordListService _service;

    public MedicalRecordController(IMedicalRecordListService service)
    {
        _service = service;
    }

    // Lấy toàn bộ lịch sử Medical Record của bệnh nhân (gần nhất lên đầu)
    [HttpGet("patient/{patientId}")]
    public IActionResult GetMedicalRecordsByPatientId(int patientId)
    {
        var records = _service.GetMedicalRecordsByPatientId(patientId);
        return Ok(records);
    }

    // Lấy chi tiết một Medical Record
    [HttpGet("{medicalRecordId}")]
    public IActionResult GetMedicalRecordList(int medicalRecordId)
    {
        var detail = _service.GetMedicalRecordsByPatientId(medicalRecordId);
        if (detail == null)
            return NotFound();
        return Ok(detail);
    }

}