using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Patient;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using System.Net;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        [HttpPost("AddPatient")]
        [ProducesResponseType(typeof(IEnumerable<Patient>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> AddPatient([FromBody] PatientCreate create)
        {
            try
            {
                var respone = await _service.CreatePatientAsync(create);
                return Ok(respone);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }


        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<Patient>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<Patient>>> GetAllPatient()
        {
            try
            {
                var response = await _service.GetAllPatientAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("FindByName/{name}")]
        [ProducesResponseType(typeof(IEnumerable<Patient>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindByName(string key)
        {
            try
            {
                var response = await _service.SearchPatientByKeyAsync(key);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("findId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<Patient>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var response = await _service.FindPatientByIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("MedicalHistory/{patientId}")]
        public async Task<IActionResult> GetMedicalHistory(int patientId)
        {
            var result = await _service.GetMedicalHistoryByPatientId(patientId);
            return Ok(result);
        }

        [HttpGet("Prescriptions/{patientId}")]
        public async Task<IActionResult> GetPrescriptions(int patientId)
        {
            var result = await _service.GetPrescriptionsByPatientId(patientId);
            return Ok(result);
        }
    }
}
