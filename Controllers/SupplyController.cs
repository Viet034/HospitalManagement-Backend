using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supply;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Services.Interface;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyController : ControllerBase
    {
        private readonly ISupplyService _supplyService;

        public SupplyController(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SupplyResponseDTO>>> GetAll()
        {
            var result = await _supplyService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplyResponseDTO>> GetById(int id)
        {
            var result = await _supplyService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SupplyResponseDTO>> Create([FromBody] SupplyCreate dto)
        {
            var created = await _supplyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplyUpdate dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var success = await _supplyService.UpdateAsync(dto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _supplyService.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpPost("search")]
        public async Task<ActionResult<List<SupplyResponseDTO>>> Search([FromBody] SupplySearch searchDto)
        {
            var result = await _supplyService.SearchAsync(searchDto);
            return Ok(result);
        }
    }
}
