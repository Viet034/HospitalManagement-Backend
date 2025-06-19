using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.DTOs;
using SWP391_SE1914_ManageHospital.Models;
using SWP391_SE1914_ManageHospital.Service;
[Route("api/[controller]")]
[ApiController]
public class LabRequestController : ControllerBase
{
	private readonly ILabRequestService _labRequestService;

	public LabRequestController(ILabRequestService labRequestService)
	{
		_labRequestService = labRequestService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllLabRequests()
	{
		var labRequests = await _labRequestService.GetAllLabRequests();
		return Ok(labRequests);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetLabRequestById(int id)
	{
		var labRequest = await _labRequestService.GetLabRequestById(id);
		if (labRequest == null) return NotFound();
		return Ok(labRequest);
	}

	[HttpPost]
	public async Task<IActionResult> CreateLabRequest([FromBody] LabRequestDto labRequestDto)
	{
		if (labRequestDto == null) return BadRequest();
		var labRequest = await _labRequestService.CreateLabRequest(labRequestDto);
        return CreatedAtAction(nameof(GetLabRequestById), new { id = labRequest.Id }, labRequest);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateLabRequest(int id, [FromBody] LabRequestDto labRequestDto)
	{
		if (labRequestDto == null) return BadRequest();
		var labRequest = await _labRequestService.UpdateLabRequest(id, labRequestDto);
		if (labRequest == null) return NotFound();
		return Ok(labRequest);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteLabRequest(int id)
	{
		var result = await _labRequestService.DeleteLabRequest(id);
		if (!result) return NotFound();
		return NoContent();
	}
}