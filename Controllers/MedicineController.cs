using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Service.Impl;

[Route("api/[controller]")]
[ApiController]
public class MedicineController : ControllerBase
{
    private readonly IMedicineService _service;

    public MedicineController(IMedicineService service)
    {
        _service = service;
    }

    [HttpGet("Get_all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("Get_By_Id")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicineCreate request)
    {
        var result = await _service.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut("Update_by_Id")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicineCreate request)
    {
        var updated = await _service.UpdateAsync(id, request);
        return updated ? Ok() : NotFound();
    }

    [HttpDelete("Delete_By_Id")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? Ok() : NotFound();
    }
}
