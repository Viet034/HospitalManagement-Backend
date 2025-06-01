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
        private readonly INurseService _nurseService;

        public NurseController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNurses()
        {
            var nurses = await _nurseService.GetAllNursesAsync();
            return Ok(nurses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNurseById(int id)
        {
            var nurse = await _nurseService.GetNurseByIdAsync(id);
            if (nurse == null) return NotFound();
            return Ok(nurse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNurse([FromBody] NurseDTO nurseDto)
        {
            if (nurseDto == null) return BadRequest();
            var createdNurse = await _nurseService.CreateNurseAsync(nurseDto);
            return CreatedAtAction(nameof(GetNurseById), new { id = createdNurse.Id }, createdNurse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNurse(int id, [FromBody] NurseDTO nurseDto)
        {
            if (nurseDto == null) return BadRequest();
            var updatedNurse = await _nurseService.UpdateNurseAsync(id, nurseDto);
            if (updatedNurse == null) return NotFound();
            return Ok(updatedNurse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNurse(int id)
        {
            var result = await _nurseService.DeleteNurseAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}