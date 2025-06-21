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

    public AppointmentService(ApplicationDBContext context, IAppointmentMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppointmentResponseDTO> ChangeAppointmentStatusAsync(int id, AppointmentStatus status)
    {
        var appointment = await _context.Appointments.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không tìm thấy cuộc hẹn với ID: {id}");

        appointment.Status = status;
        appointment.UpdateDate = DateTime.Now;
        appointment.UpdateBy = "System";

        await _context.SaveChangesAsync();

        return _mapper.EntityToResponse(appointment);
    }

    public async Task<AppointmentResponseDTO> CreateAppointmentAsync(AppointmentCreate create)
    {
        try
        {
            // Kiểm tra xem bệnh nhân có tồn tại không
            var patient = await _context.Patients.FindAsync(create.PatientId)
                ?? throw new KeyNotFoundException($"Không tìm thấy bệnh nhân với ID: {create.PatientId}");

            // Kiểm tra xem phòng khám có tồn tại không
            var clinic = await _context.Clinics.FindAsync(create.ClinicId)
                ?? throw new KeyNotFoundException($"Không tìm thấy phòng khám với ID: {create.ClinicId}");

            // Kiểm tra xem lễ tân có tồn tại không
            var reception = await _context.Receptions.FindAsync(create.ReceptionId)
                ?? throw new KeyNotFoundException($"Không tìm thấy lễ tân với ID: {create.ReceptionId}");

            // Tạo mã duy nhất nếu không có
            if (string.IsNullOrEmpty(create.Code) || create.Code == "string")
            {
                create.Code = await GenerateUniqueCodeAsync();
            }

            // Nếu không có các giá trị ngày tạo và người tạo, thiết lập giá trị mặc định
            if (create.CreateDate == default)
            {
                create.CreateDate = DateTime.Now;
            }

            if (string.IsNullOrEmpty(create.CreateBy))
            {
                create.CreateBy = "System";
            }

            var entity = _mapper.CreateToEntity(create);
            
            await _context.Appointments.AddAsync(entity);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Ghi log lỗi chi tiết từ inner exception
                var innerException = dbEx.InnerException;
                Console.WriteLine($"Database error: {dbEx.Message}");
                Console.WriteLine($"Inner exception: {innerException?.Message}");
                
                // Ném ngoại lệ với thông tin chi tiết hơn
                throw new Exception($"Lỗi khi lưu dữ liệu: {dbEx.Message}", dbEx);
            }

            return _mapper.EntityToResponse(entity);
        }
        catch (KeyNotFoundException)
        {
            throw; // Re-throw the key not found exceptions
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateAppointmentAsync: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw new Exception($"Lỗi khi tạo cuộc hẹn: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAppointmentAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không tìm thấy cuộc hẹn với ID: {id}");

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
            _context.ChangeTracker.Clear();
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
            .ToListAsync();

        return _mapper.ListEntityToResponse(appointments);
    }

    public async Task<AppointmentResponseDTO> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không tìm thấy cuộc hẹn với ID: {id}");

        return _mapper.EntityToResponse(appointment);
    }

    public async Task<IEnumerable<AppointmentResponseDTO>> GetAppointmentsByPatientIdAsync(int patientId)
    {
        var patient = await _context.Patients.FindAsync(patientId)
            ?? throw new KeyNotFoundException($"Không tìm thấy bệnh nhân với ID: {patientId}");

        var appointments = await _context.Appointments
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .ToListAsync();

        return _mapper.ListEntityToResponse(appointments);
    }

    public async Task<IEnumerable<AppointmentResponseDTO>> SearchAppointmentsByNameAsync(string name)
    {
        var appointments = await _context.Appointments
            .FromSqlRaw("SELECT * FROM Appointments WHERE Name LIKE {0}", "%" + name + "%")
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .ToListAsync();

        return _mapper.ListEntityToResponse(appointments);
    }

    public async Task<AppointmentResponseDTO> UpdateAppointmentAsync(int id, AppointmentUpdate update)
    {
        var appointment = await _context.Appointments.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không tìm thấy cuộc hẹn với ID: {id}");

        // Kiểm tra xem bệnh nhân có tồn tại không
        if (appointment.PatientId != update.PatientId)
        {
            var patient = await _context.Patients.FindAsync(update.PatientId)
                ?? throw new KeyNotFoundException($"Không tìm thấy bệnh nhân với ID: {update.PatientId}");
        }

        // Kiểm tra xem phòng khám có tồn tại không
        if (appointment.ClinicId != update.ClinicId)
        {
            var clinic = await _context.Clinics.FindAsync(update.ClinicId)
                ?? throw new KeyNotFoundException($"Không tìm thấy phòng khám với ID: {update.ClinicId}");
        }

        // Kiểm tra xem lễ tân có tồn tại không
        if (appointment.ReceptionId != update.ReceptionId)
        {
            var reception = await _context.Receptions.FindAsync(update.ReceptionId)
                ?? throw new KeyNotFoundException($"Không tìm thấy lễ tân với ID: {update.ReceptionId}");
        }

        appointment.Name = update.Name;
        appointment.Code = update.Code;
        appointment.AppointmentDate = update.AppointmentDate;
        appointment.StartTime = update.StartTime;
        appointment.EndTime = update.EndTime;
        appointment.Status = update.Status;
        appointment.Note = update.Note;
        appointment.PatientId = update.PatientId;
        appointment.ClinicId = update.ClinicId;
        appointment.ReceptionId = update.ReceptionId;
        appointment.CreateDate = update.CreateDate;
        appointment.UpdateDate = update.UpdateDate;
        appointment.CreateBy = update.CreateBy;
        appointment.UpdateBy = update.UpdateBy;

        await _context.SaveChangesAsync();

        return _mapper.EntityToResponse(appointment);
    }
} 