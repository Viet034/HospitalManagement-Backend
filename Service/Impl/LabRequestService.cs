using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.DTOs;
using SWP391_SE1914_ManageHospital.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class LabRequestService : ILabRequestService
{
	private readonly ApplicationDBContext _context;
	private readonly IMapper _mapper;

	public LabRequestService(ApplicationDBContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IEnumerable<LabRequestDto>> GetAllLabRequests()
	{
		var labRequests = await _context.LabRequests.ToListAsync();
		return _mapper.Map<IEnumerable<LabRequestDto>>(labRequests);
	}

    public async Task<LabRequestDto> GetLabRequestById(int id)
	{
		var labRequest = await _context.LabRequests.FindAsync(id);
		if (labRequest == null) return null;
		return _mapper.Map<LabRequestDto>(labRequest);
	}

	public async Task<LabRequestDto> CreateLabRequest(LabRequestDto labRequestDto)
	{
		var labRequest = _mapper.Map<LabRequest>(labRequestDto);
		labRequest.CreateDate = DateTime.UtcNow;
		labRequest.UpdateDate = DateTime.UtcNow;
		labRequest.CreateBy = "Admin"; // You can set it based on your application logic
		labRequest.UpdateBy = "Admin"; // You can set it based on your application logic

		_context.LabRequests.Add(labRequest);
		await _context.SaveChangesAsync();
		return _mapper.Map<LabRequestDto>(labRequest);
	}

	public async Task<LabRequestDto> UpdateLabRequest(int id, LabRequestDto labRequestDto)
	{
		var labRequest = await _context.LabRequests.FindAsync(id);
		if (labRequest == null) return null;

		_mapper.Map(labRequestDto, labRequest);
		labRequest.UpdateDate = DateTime.UtcNow;
		labRequest.UpdateBy = "Admin"; // You can set it based on your application logic

		_context.LabRequests.Update(labRequest);
		await _context.SaveChangesAsync();
		return _mapper.Map<LabRequestDto>(labRequest);
	}

	public async Task<bool> DeleteLabRequest(int id)
	{
		var labRequest = await _context.LabRequests.FindAsync(id);
		if (labRequest == null) return false;

		_context.LabRequests.Remove(labRequest);
		await _context.SaveChangesAsync();
		return true;
	}
}
