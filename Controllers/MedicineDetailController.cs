using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineDetailController : ControllerBase
    {
        private readonly IMedicineDetailService _service;

        public MedicineDetailController(IMedicineDetailService service)
        {
            _service = service;
        }

        // GET: api/MedicineDetail/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET: api/MedicineDetail/get-by-id/{id}
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/MedicineDetail/add-medicine-detail
        [HttpPost("add-medicine-detail")]
        public async Task<IActionResult> Create([FromBody] MedicineDetailRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/MedicineDetail/update-medicine-detail/{id}
        [HttpPut("update-medicine-detail/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicineDetailRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/MedicineDetail/delete-medicine-detail/{id}
        [HttpDelete("delete-medicine-detail/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
