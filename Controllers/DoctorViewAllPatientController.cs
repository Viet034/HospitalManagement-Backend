using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [ApiController]
    [Route("api/doctor/{doctorId}/patients")]
    public class DoctorViewAllPatientController : ControllerBase
    {
        private readonly IDoctorViewAllPatientService _service;

        public DoctorViewAllPatientController(IDoctorViewAllPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllPatients(int doctorId)
        {
            try
            {
                var patients = _service.GetAllPatientsByDoctorId(doctorId);
                return Ok(patients);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}