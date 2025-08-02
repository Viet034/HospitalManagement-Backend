using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

public class AppointmentService_1 : IAppointmentService_1
{
    private readonly ApplicationDBContext _context;

    public AppointmentService_1(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppointmentResponseDTO_1>> GetAllAsync()
    {
        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Clinic)
            .Include(a => a.Reception)
            .Include(a => a.Service)
            .ToListAsync();

        var result = appointments.Select(a => new AppointmentResponseDTO_1
        {
            Id = a.Id,
            AppointmentDate = a.AppointmentDate,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status,
            Note = a.Note,
            isSend = a.isSend,

            PatientId = a.PatientId,
            PatientName = a.Patient?.Name?? "(Không có)",

            ClinicId = a.ClinicId,
            ClinicName = a.Clinic?.Name ?? "(Không có)",

            ReceptionId = a.ReceptionId,
            ReceptionName = a.Reception?.Name?? null,

            ServiceId = a.ServiceId,
            ServiceName = a.Service?.Name ?? null,

            Code = a.Code,
            CreateDate = a.CreateDate,
            UpdateDate = a.UpdateDate,
            CreateBy = a.CreateBy ?? "system",
            UpdateBy = a.UpdateBy ?? "system"
        });

        return result;
    }

    public async Task<AppointmentResponseDTO_1?> GetByIdAsync(int id)
    {
        var a = await _context.Appointments
            .Include(x => x.Patient)
            .Include(x => x.Clinic)
            .Include(x => x.Reception)
            .Include(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (a == null) return null;

        return new AppointmentResponseDTO_1
        {
            Id = a.Id,
            AppointmentDate = a.AppointmentDate,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status,
            Note = a.Note,
            isSend = a.isSend,
            PatientId = a.PatientId,
            PatientName = a.Patient?.Name ?? "(Không có)",
            ClinicId = a.ClinicId,
            ClinicName = a.Clinic?.Name ?? "(Không có)",
            ReceptionId = a.ReceptionId,
            ReceptionName = a.Reception?.Name ?? null,
            ServiceId = a.ServiceId,
            ServiceName = a.Service?.Name ?? null,
            Code = a.Code,
            CreateDate = a.CreateDate,
            UpdateDate = a.UpdateDate,
            CreateBy = a.CreateBy ?? "system",
            UpdateBy = a.UpdateBy ?? "system"
        };
    }
    public async Task<double> CalculateGrowthPercentageAsync()
    {
        var now = DateTime.UtcNow;

        var thisMonth = await _context.Appointments
            .CountAsync(a => a.CreateDate.Month == now.Month && a.CreateDate.Year == now.Year);

        var lastMonthDate = now.AddMonths(-1);
        var lastMonth = await _context.Appointments
            .CountAsync(a => a.CreateDate.Month == lastMonthDate.Month && a.CreateDate.Year == lastMonthDate.Year);

        if (lastMonth == 0)
        {
            return thisMonth > 0 ? 100.0 : 0.0; // tránh chia cho 0
        }

        double growth = ((double)(thisMonth - lastMonth) / lastMonth) * 100;
        return growth;
    }


    public async Task<IEnumerable<AppointmentResponseDTO_1>> GetByPatientIdAsync(int patientId)
    {
        var appointments = await _context.Appointments
            .Include(x => x.Doctor_Appointments)
                .ThenInclude(da => da.Doctor)
            .Include(x => x.Patient)
            .Include(x => x.Clinic)
            .Include(x => x.Reception)
            .Include(x => x.Service)
            .Where(x => x.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .ToListAsync();

        if (appointments == null || appointments.Count == 0)
        {
            throw new Exception("Không có bản ghi nào");
        }

        var result = appointments.Select(x => new AppointmentResponseDTO_1
        {
            Id = x.Id,
            AppointmentDate = x.AppointmentDate,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            Status = x.Status,
            Note = x.Note,
            isSend = x.isSend,

            DoctorName = x.Doctor_Appointments
            .FirstOrDefault()?.Doctor?.Name ?? "N/A",

            PatientId = x.PatientId,
            PatientName = x.Patient?.Name ?? "N/A",

            ClinicId = x.ClinicId,
            ClinicName = x.Clinic?.Name ?? "N/A",

            ReceptionId = x.ReceptionId,
            ReceptionName = x.Reception != null ? x.Reception.Name ?? "N/A" : null,

            ServiceId = x.ServiceId,
            ServiceName = x.Service != null ? x.Service.Name : null,

            Code = x.Code,
            CreateDate = x.CreateDate,
            UpdateDate = x.UpdateDate,
            CreateBy = x.CreateBy,
            UpdateBy = x.UpdateBy
        });

        return result;
    }

    public async Task<AppointmentResponseDTO_1> SoftDelete(int id, AppointmentStatus newStatus)
    {
        var appointment = await _context.Appointments
            .Include(x => x.Doctor_Appointments)
                .ThenInclude(da => da.Doctor)
            .Include(x => x.Patient)
            .Include(x => x.Clinic)
            .Include(x => x.Reception)
            .Include(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (appointment == null)
        {
            throw new Exception("ID lịch hẹn không tồn tại!");
        }

        
        appointment.Status = newStatus;
        appointment.UpdateDate = DateTime.UtcNow; 

      
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();

        var doctorAppointment = await _context.Doctor_Appointments
            .FirstOrDefaultAsync(da => da.AppointmentId == id);
        if (doctorAppointment != null)
        {
            doctorAppointment.Status = DoctorAppointmentStatus.Cancelled;
            _context.Doctor_Appointments.Update(doctorAppointment);
            await _context.SaveChangesAsync();
        }


        var invoice = await _context.Invoices
        .FirstOrDefaultAsync(inv => inv.AppointmentId == id);
        if (invoice != null)
        {
            invoice.Status = InvoiceStatus.Cancelled;
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }


        return new AppointmentResponseDTO_1
        {
            Id = appointment.Id,
            AppointmentDate = appointment.AppointmentDate,
            StartTime = appointment.StartTime,
            EndTime = appointment.EndTime,
            Status = appointment.Status,
            Note = appointment.Note,
            isSend = appointment.isSend,

            DoctorName = appointment.Doctor_Appointments.FirstOrDefault()?.Doctor?.Name ?? "N/A",
            PatientId = appointment.PatientId,
            PatientName = appointment.Patient?.Name ?? "N/A",
            ClinicId = appointment.ClinicId,
            ClinicName = appointment.Clinic?.Name ?? "N/A",
            ReceptionId = appointment.ReceptionId,
            ReceptionName = appointment.Reception?.Name ?? "N/A",
            ServiceId = appointment.ServiceId,
            ServiceName = appointment.Service?.Name,
            Code = appointment.Code,
            CreateDate = appointment.CreateDate,
            UpdateDate = appointment.UpdateDate,
            CreateBy = appointment.CreateBy,
            UpdateBy = appointment.UpdateBy
        };


    }

    public async Task<IEnumerable<AppointmentResponseDTO_1>> GetAllForAdminAsync()
    {
        var appointments = await _context.Appointments
            .Include(x => x.Doctor_Appointments)
                .ThenInclude(da => da.Doctor)
            .Include(x => x.Patient)
            .Include(x => x.Clinic)
            .Include(x => x.Reception)
            .Include(x => x.Service)
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .ToListAsync();

        if (appointments == null || appointments.Count == 0)
        {
            throw new Exception("Không có bản ghi nào");
        }

        var result = appointments.Select(x => new AppointmentResponseDTO_1
        {
            Id = x.Id,
            AppointmentDate = x.AppointmentDate,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            Status = x.Status,
            Note = x.Note,
            isSend = x.isSend,

            DoctorName = x.Doctor_Appointments
            .FirstOrDefault()?.Doctor?.Name ?? "N/A",

            PatientId = x.PatientId,
            PatientName = x.Patient?.Name ?? "N/A",

            ClinicId = x.ClinicId,
            ClinicName = x.Clinic?.Name ?? "N/A",

            ReceptionId = x.ReceptionId,
            ReceptionName = x.Reception != null ? x.Reception.Name ?? "N/A" : null,

            ServiceId = x.ServiceId,
            ServiceName = x.Service != null ? x.Service.Name : null,

            Code = x.Code,
            CreateDate = x.CreateDate,
            UpdateDate = x.UpdateDate,
            CreateBy = x.CreateBy,
            UpdateBy = x.UpdateBy
        });

        return result;
    }
}
