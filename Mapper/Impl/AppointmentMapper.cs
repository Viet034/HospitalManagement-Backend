using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl;

public class AppointmentMapper : IAppointmentMapper
{
    private readonly ApplicationDBContext _context;

    public AppointmentMapper(ApplicationDBContext context)
    {
        _context = context;
    }

    public Appointment CreateToEntity(AppointmentCreate create)
    {
        return new Appointment
        {
            Name = create.Name,
            Code = create.Code,
            AppointmentDate = create.AppointmentDate,
            StartTime = create.StartTime,
            EndTime = create.EndTime,
            Status = create.Status,
            Note = create.Note ?? string.Empty,
            PatientId = create.PatientId,
            ClinicId = create.ClinicId,
            ReceptionId = create.ReceptionId,
            CreateDate = create.CreateDate != default ? create.CreateDate : DateTime.Now,
            CreateBy = !string.IsNullOrEmpty(create.CreateBy) ? create.CreateBy : "System",
            UpdateDate = DateTime.Now,
            UpdateBy = "System"
        };
    }

    public Appointment DeleteToEntity(AppointmentDelete delete)
    {
        return new Appointment
        {
            Name = delete.Name,
            Code = delete.Code,
            Status = delete.Status,
            CreateDate = delete.CreateDate,
            UpdateDate = delete.UpdateDate,
            CreateBy = delete.CreateBy,
            UpdateBy = delete.UpdateBy
        };
    }

    public AppointmentResponseDTO EntityToResponse(Appointment entity)
    {
        var response = new AppointmentResponseDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            AppointmentDate = entity.AppointmentDate,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            Status = entity.Status,
            Note = entity.Note,
            CreateDate = entity.CreateDate,
            UpdateDate = entity.UpdateDate,
            CreateBy = entity.CreateBy,
            UpdateBy = entity.UpdateBy,
            PatientId = entity.PatientId,
            ClinicId = entity.ClinicId,
            ReceptionId = entity.ReceptionId
        };

        // Lấy thông tin bệnh nhân từ cơ sở dữ liệu
        var patient = _context.Patients
            .Include(p => p.User)
            .FirstOrDefault(p => p.Id == entity.PatientId);

        if (patient != null)
        {
            response.PatientName = patient.Name;
            response.PatientEmail = patient.User?.Email;
            response.PatientImage = patient.ImageURL;
            response.Type = patient.Status == Ultility.Status.PatientStatus.Active ? "Old Patient" : "New Patient";
        }

        return response;
    }

    public IEnumerable<AppointmentResponseDTO> ListEntityToResponse(IEnumerable<Appointment> entities)
    {
        return entities.Select(entity => EntityToResponse(entity));
    }

    public void UpdateEntityFromDto(AppointmentUpdate update, Appointment entity)
    {
        entity.Name = update.Name;
        entity.Code = update.Code;
        entity.AppointmentDate = update.AppointmentDate;
        entity.StartTime = update.StartTime;
        entity.EndTime = update.EndTime;
        entity.Status = update.Status;
        entity.Note = update.Note;
        entity.PatientId = update.PatientId;
        entity.ClinicId = update.ClinicId;
        entity.ReceptionId = update.ReceptionId;
        entity.CreateDate = update.CreateDate;
        entity.UpdateDate = update.UpdateDate;
        entity.CreateBy = update.CreateBy;
        entity.UpdateBy = update.UpdateBy;
    }
} 