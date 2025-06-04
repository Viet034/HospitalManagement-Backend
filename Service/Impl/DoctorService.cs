using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class DoctorService : IDoctorService
{
    private readonly ApplicationDBContext _context;
    private readonly IDoctorMapper _mapper;
    private readonly IDoctorScheduleMapper _scheduleMapper;

    public DoctorService(ApplicationDBContext context, IDoctorMapper mapper, IDoctorScheduleMapper scheduleMapper)
    {
        _context = context;
        _mapper = mapper;
        _scheduleMapper = scheduleMapper;
    }

    public async Task<IEnumerable<DoctorResponseDTO>> GetActiveDoctorsByClinicAsync(int clinicId)
    {
        // Kiểm tra phòng khám có tồn tại và đang hoạt động
        var clinic = await _context.Clinics
            .FirstOrDefaultAsync(c => c.Id == clinicId && c.Status == ClinicStatus.Available)
            ?? throw new Exception($"Không tìm thấy phòng khám có ID {clinicId} hoặc phòng khám không hoạt động");

        // Lấy danh sách các cuộc hẹn của phòng khám
        var doctorIds = await _context.Appointments
            .Where(a => a.ClinicId == clinicId)
            .SelectMany(a => a.Doctor_Appointments)
            .Select(da => da.DoctorId)
            .Distinct()
            .ToListAsync();

        // Lấy thông tin các bác sĩ đang hoạt động
        var doctors = await _context.Doctors
            .Include(d => d.Department)
            .Where(d => doctorIds.Contains(d.Id) && d.Status == DoctorStatus.Available)
            .ToListAsync();

        if (!doctors.Any())
        {
            throw new Exception($"Không có bác sĩ nào đang hoạt động tại phòng khám có ID {clinicId}");
        }

        return _mapper.ListEntityToResponse(doctors);
    }

    public async Task<IEnumerable<DoctorScheduleResponseDTO>> GetDoctorScheduleInClinicAsync(int doctorId, int clinicId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        // Kiểm tra bác sĩ có tồn tại và đang hoạt động
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == doctorId && d.Status == DoctorStatus.Available)
            ?? throw new Exception($"Không tìm thấy bác sĩ có ID {doctorId} hoặc bác sĩ không hoạt động");

        // Kiểm tra phòng khám có tồn tại và đang hoạt động
        var clinic = await _context.Clinics
            .FirstOrDefaultAsync(c => c.Id == clinicId && c.Status == ClinicStatus.Available)
            ?? throw new Exception($"Không tìm thấy phòng khám có ID {clinicId} hoặc phòng khám không hoạt động");

        // Query cơ bản
        var query = _context.Doctor_Appointments
            .Include(da => da.Appointment)
                .ThenInclude(a => a.Clinic)
            .Include(da => da.Doctor)
            .Where(da => da.DoctorId == doctorId && 
                        da.Appointment.ClinicId == clinicId);

        // Thêm điều kiện lọc theo ngày nếu có
        if (fromDate.HasValue)
        {
            query = query.Where(da => da.Appointment.AppointmentDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(da => da.Appointment.AppointmentDate <= toDate.Value);
        }

        // Lấy danh sách lịch làm việc
        var doctorAppointments = await query
            .OrderBy(da => da.Appointment.AppointmentDate)
            .ThenBy(da => da.Appointment.StartTime)
            .ToListAsync();

        if (!doctorAppointments.Any())
        {
            throw new Exception($"Không tìm thấy lịch làm việc nào của bác sĩ {doctor.Name} tại phòng khám {clinic.Name} trong khoảng thời gian yêu cầu");
        }

        return _scheduleMapper.MapToResponseList(doctorAppointments);
    }
} 