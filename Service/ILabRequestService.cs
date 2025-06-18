using SWP391_SE1914_ManageHospital.DTOs;

public interface ILabRequestService
{
	Task<IEnumerable<LabRequestDto>> GetAllLabRequests();
	Task<LabRequestDto> GetLabRequestById(int id);
	Task<LabRequestDto> CreateLabRequest(LabRequestDto labRequestDto);
	Task<LabRequestDto> UpdateLabRequest(int id, LabRequestDto labRequestDto);
	Task<bool> DeleteLabRequest(int id);
}