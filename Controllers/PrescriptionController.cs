using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _service;

        public PrescriptionController(IPrescriptionService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult CreatePrescription([FromBody] CreatePrescriptionDTO dto)
        {
            var result = _service.CreatePrescription(dto);
            return result ? Ok("Prescription created") : BadRequest("Failed");
        }
    }
}
