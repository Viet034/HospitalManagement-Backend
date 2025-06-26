using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class AppointmentService : IAppointmentService
{
    private readonly ApplicationDBContext _context;
    private readonly IAppointmentMapper _mapper;
    private const string DefaultUser = "System";
    private const string StringPlaceholder = "string";

    public AppointmentService(ApplicationDBContext context, IAppointmentMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppointmentResponseDTO> ChangeAppointmentStatusAsync(int id, AppointmentStatus status)
    {
        var appointment = await GetAppointmentByIdInternalAsync(id);

        appointment.Status = status;
        appointment.UpdateDate = DateTime.Now;
        appointment.UpdateBy = DefaultUser;

        await _context.SaveChangesAsync();

        return _mapper.EntityToResponse(appointment);
    }

    public async Task<AppointmentResponseDTO> CreateAppointmentAsync(AppointmentCreate create)
    {
        await ValidatePatientExistsAsync(create.PatientId);
        await ValidateClinicExistsAsync(create.ClinicId);
        await ValidateReceptionExistsAsync(create.ReceptionId);

        if (string.IsNullOrEmpty(create.Code) || create.Code == StringPlaceholder)
        {
            create.Code = await GenerateUniqueCodeAsync();
        }

        var now = DateTime.Now;
        create.CreateDate = now;
        create.CreateBy = DefaultUser;

        var entity = _mapper.CreateToEntity(create);
        entity.UpdateDate = now;
        entity.UpdateBy = DefaultUser;

        await _context.Appointments.AddAsync(entity);
        await _context.SaveChangesAsync();

        return _mapper.EntityToResponse(entity);
    }

    public async Task<bool> DeleteAppointmentAsync(int id)
    {
        var appointment = await GetAppointmentByIdInternalAsync(id);
        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> GenerateUniqueCodeAsync()
    {
        string newCode;
        bool isExist;

        do
        {
            newCode = GenerateCode.GenerateAppointmentCode();
            isExist = await _context.Appointments.AnyAsync(p => p.Code == newCode);
        }
        while (isExist);

        return newCode;
    }

    public async Task<IEnumerable<AppointmentResponseDTO>> GetAllAppointmentsAsync()
    {
        var appointments = await _context.Appointments
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.ListEntityToResponse(appointments);
    }

    public async Task<AppointmentResponseDTO> GetAppointmentByIdAsync(int id)
    {
        var appointment = await GetAppointmentByIdInternalAsync(id);
        return _mapper.EntityToResponse(appointment);
    }

    public async Task<IEnumerable<AppointmentResponseDTO>> GetAppointmentsByPatientIdAsync(int patientId)
    {
        await ValidatePatientExistsAsync(patientId);

        var appointments = await _context.Appointments
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.ListEntityToResponse(appointments);
    }

    public async Task<IEnumerable<AppointmentResponseDTO>> SearchAppointmentsByNameAsync(string name)
    {
        var appointments = await _context.Appointments
            .Where(a => a.Name.Contains(name))
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.ListEntityToResponse(appointments);
    }

    public async Task<AppointmentResponseDTO> UpdateAppointmentAsync(int id, AppointmentUpdate update)
    {
        var appointment = await GetAppointmentByIdInternalAsync(id);

        await ValidatePatientExistsAsync(update.PatientId);
        await ValidateClinicExistsAsync(update.ClinicId);
        await ValidateReceptionExistsAsync(update.ReceptionId);

        _mapper.UpdateEntityFromDto(update, appointment);
        
        appointment.UpdateDate = DateTime.Now;
        appointment.UpdateBy = DefaultUser;

        await _context.SaveChangesAsync();

        return _mapper.EntityToResponse(appointment);
    }
    
    private async Task<Appointment> GetAppointmentByIdInternalAsync(int id)
    {
        return await _context.Appointments.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không tìm thấy cuộc hẹn với ID: {id}");
    }

    private async Task ValidatePatientExistsAsync(int patientId)
    {
        if (!await _context.Patients.AnyAsync(p => p.Id == patientId))
        {
            throw new KeyNotFoundException($"Không tìm thấy bệnh nhân với ID: {patientId}");
        }
    }

    private async Task ValidateClinicExistsAsync(int clinicId)
    {
        if (!await _context.Clinics.AnyAsync(c => c.Id == clinicId))
        {
            throw new KeyNotFoundException($"Không tìm thấy phòng khám với ID: {clinicId}");
        }
    }

    private async Task ValidateReceptionExistsAsync(int receptionId)
    {
        if (!await _context.Receptions.AnyAsync(r => r.Id == receptionId))
        {
            throw new KeyNotFoundException($"Không tìm thấy lễ tân với ID: {receptionId}");
        }
    }
} 