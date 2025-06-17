using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineCategoryController : ControllerBase
    {
        private readonly IMedicineCategoryService _service;

        public MedicineCategoryController(IMedicineCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MedicineCategoryResponseDTO>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineCategoryResponseDTO>> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<MedicineCategoryResponseDTO>> Create([FromBody] MedicineCategoryCreate req)
        {
            var dto = await _service.CreateAsync(req);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicineCategoryCreate req)
        {
            return await _service.UpdateAsync(id, req)
                ? NoContent()
                : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.DeleteAsync(id)
                ? NoContent()
                : NotFound();
        }
    }
}
