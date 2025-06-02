using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class PatientFilterService : IPatientFilterService
{
    private readonly ApplicationDBContext _context;
    private readonly IPatientFilterMapper _patientMapper;

    public PatientFilterService(ApplicationDBContext context, IPatientFilterMapper patientMapper)
    {
        _context = context;
        _patientMapper = patientMapper;
    }

    public async Task<IEnumerable<PatientFilterResponse>> GetPatientsByDoctorAsync(PatientFilter filter)
    {
        var query = _context.Medical_Records
            .Where(mr => mr.DoctorId == filter.DoctorId);

        if (filter.FromDate.HasValue)
            query = query.Where(mr => mr.CreateDate >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            query = query.Where(mr => mr.CreateDate <= filter.ToDate.Value);

        if (!string.IsNullOrEmpty(filter.PatientName))
            query = query.Where(mr => mr.Name.Contains(filter.PatientName));

        var patientIds = await query
            .Select(mr => mr.PatientId)
            .Distinct()
            .ToListAsync();

        var patients = await _context.Patients
            .Where(p => patientIds.Contains(p.Id))
            .ToListAsync();

        return _patientMapper.ListEntityToResponse(patients);
    }
}