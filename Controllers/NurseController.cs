using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using SWP391_SE1914_ManageHospital.Service;
using System.Threading.Tasks;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseController : ControllerBase
    {
        private readonly NurseService _nurseService;

        public NurseController(NurseService nurseService)
        {
            _nurseService = nurseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNurses()
        {
            try
            {
                var nurses = await _nurseService.GetAllNurses();
                return Ok(nurses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNurse(int id)
        {
            try
            {
                var nurse = await _nurseService.GetNurseById(id);
                if (nurse == null) return NotFound();
                return Ok(nurse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNurse([FromBody] NurseDTO nurseDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var createdNurse = await _nurseService.CreateNurse(nurseDto);
                return CreatedAtAction(nameof(GetNurse), new { id = createdNurse.Id }, createdNurse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNurse(int id, [FromBody] NurseDTO nurseDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var updatedNurse = await _nurseService.UpdateNurse(id, nurseDto);
                if (updatedNurse == null) return NotFound();
                return Ok(updatedNurse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNurse(int id)
        {
            try
            {
                var result = await _nurseService.DeleteNurse(id);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("appointment")]
        public async Task<IActionResult> CreateNurseAppointment([FromBody] Nurse_AppointmentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var createdDto = await _nurseService.CreateNurseAppointment(dto);
                return CreatedAtAction(nameof(GetNurseAppointment), new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("appointment/{id}")]
        public async Task<IActionResult> GetNurseAppointment(int id)
        {
            try
            {
                var appointment = await _nurseService.GetNurseAppointmentById(id);
                if (appointment == null) return NotFound();
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}