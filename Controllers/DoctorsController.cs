using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Doctor;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<DoctorResponseDTO>>> GetDoctors()
        {
            try
            {
                var doctors = await _doctorService.GetAllDoctorsAsync();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("FindById/{id}")]
        public async Task<ActionResult<DoctorResponseDTO>> GetDoctor(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);
                if (doctor == null)
                {
                    return NotFound($"Doctor with ID {id} not found.");
                }
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("FindByName/{name}")]
        public async Task<ActionResult<IEnumerable<DoctorResponseDTO>>> GetDoctorByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Name cannot be empty.");
                }
                var doctors = await _doctorService.GetDoctorByNameAsync(name);
                if (!doctors.Any())
                {
                    return NotFound($"No doctors found with name containing: {name}");
                }
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddDoctor")]
        public async Task<ActionResult<DoctorResponseDTO>> CreateDoctor([FromBody] DoctorCreate doctorCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var createdDoctor = await _doctorService.CreateDoctorAsync(doctorCreateDto);
                return CreatedAtAction(nameof(GetDoctor), new { id = createdDoctor.Id }, createdDoctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateDoctor/{userid}")]
        public async Task<IActionResult> UpdateDoctor(int userid, [FromBody] DoctorUpdate doctorUpdateDto)
        {
            try
            {
                //if (id != doctorUpdateDto.Id)
                //{
                //    return BadRequest("Doctor ID mismatch.");
                //}
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedDoctor = await _doctorService.UpdateDoctorAsync(userid, doctorUpdateDto);
                if (updatedDoctor == null)
                {
                    return NotFound($"Doctor with ID {userid} not found.");
                }
                return Ok(updatedDoctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                var deleted = await _doctorService.DeleteDoctorAsync(id, null); // Adjust if DoctorDelete DTO is required
                if (!deleted)
                {
                    return NotFound($"Doctor with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("FindByUserId/{userId}")]
        public async Task<ActionResult<DoctorResponseDTO>> GetDoctorByUserId(int userId)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByUserIdAsync(userId);
                if (doctor == null)
                {
                    return NotFound($"Doctor with UserId {userId} not found.");
                }
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-by-departmentId")]
        [ProducesResponseType(typeof(IEnumerable<Doctor>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetAllDepartment(int departmentId)
        {
            try
            {
                var response = await _doctorService.GetDoctorsByDepartmentAsync(departmentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("doctor-calculate%")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDoctorGrowthPercentage()
        {
            try
            {
                var growthPercentage = await _doctorService.GetDoctorGrowthPercentageAsync();
                return Ok(growthPercentage);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("top-prescriptions")]
        [ProducesResponseType(typeof(List<DoctorPrescriptionTopDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTopDoctorsByPrescription([FromQuery] int top = 3)
        {
            try
            {
                var result = await _doctorService.GetTopDoctorsByPrescriptionAsync(top);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

    }
}