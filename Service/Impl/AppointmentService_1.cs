using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

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


}
