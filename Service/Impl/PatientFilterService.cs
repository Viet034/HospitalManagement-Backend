using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class PatientFilterService : IPatientFilterService
    {
        private readonly ApplicationDBContext _context;
        private readonly IPatientFilterMapper _patientMapper;

        public PatientFilterService(ApplicationDBContext context, IPatientFilterMapper patientMapper)
        {
            _context = context;
            _patientMapper = patientMapper;
        }

        public async Task<List<PatientFilterResponse>> GetPatientsByDoctorAsync(PatientFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Bộ lọc không được để trống.");
            }

            if (filter.DoctorId <= 0)
            {
                throw new ArgumentException("Thiếu thông tin DoctorId hoặc DoctorId không hợp lệ.", nameof(filter));
            }

            // Kiểm tra bác sĩ có tồn tại
            bool doctorExists = await _context.Doctors.AnyAsync(d => d.Id == filter.DoctorId);
            if (!doctorExists)
            {
                throw new ArgumentException($"Không tìm thấy bác sĩ có ID: {filter.DoctorId}", nameof(filter));
            }

            // Lấy danh sách cuộc hẹn của bác sĩ
            List<Doctor_Appointment> doctorAppointments = await _context.Doctor_Appointments
                .Where(da => da.DoctorId == filter.DoctorId)
                .ToListAsync();

            // Tạo danh sách ID cuộc hẹn
            List<int> appointmentIds = new List<int>();
            foreach (Doctor_Appointment da in doctorAppointments)
            {
                appointmentIds.Add(da.AppointmentId);
            }

            // Lấy thông tin chi tiết cuộc hẹn
            var query = _context.Appointments
                .Where(a => appointmentIds.Contains(a.Id));

            // Áp dụng các bộ lọc ngày tháng
            if (filter.FromDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate <= filter.ToDate.Value);
            }

            // Áp dụng bộ lọc trạng thái nếu có
            if (!string.IsNullOrEmpty(filter.AppointmentStatus))
            {
                // Cần chuyển đổi string thành enum để so sánh
                if (Enum.TryParse<AppointmentStatus>(filter.AppointmentStatus, out AppointmentStatus status))
                {
                    query = query.Where(a => a.Status == status);
                }
            }

            // Lấy ra kết quả cuộc hẹn và include bệnh nhân
            List<Appointment> appointments = await query
                .Include(a => a.Patient)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            // Lọc theo tên bệnh nhân nếu có
            if (!string.IsNullOrEmpty(filter.PatientName))
            {
                List<Appointment> filteredAppointments = new List<Appointment>();
                foreach (Appointment appointment in appointments)
                {
                    if (appointment.Patient != null &&
                        appointment.Patient.Name.Contains(filter.PatientName, StringComparison.OrdinalIgnoreCase))
                    {
                        filteredAppointments.Add(appointment);
                    }
                }
                appointments = filteredAppointments;
            }

            // Chuyển đổi dữ liệu sang định dạng cho mapper
            List<PatientAppointmentData> patientDataList = new List<PatientAppointmentData>();

            foreach (Appointment appointment in appointments)
            {
                if (appointment.Patient != null)
                {
                    PatientAppointmentData patientData = new PatientAppointmentData();
                    patientData.Patient = appointment.Patient;
                    patientData.ExaminationTime = appointment.AppointmentDate;
                    patientData.AppointmentStatus = appointment.Status.ToString();

                    patientDataList.Add(patientData);
                }
            }

            // Chuyển đổi sang response
            List<PatientFilterResponse> result = _patientMapper.ListEntityToResponse(patientDataList);
            return result;
        }

        public async Task<List<PatientFilterResponse>> GetTodayPatientsByDoctorAsync(int doctorId)
        {
            // Validation
            if (doctorId <= 0)
            {
                throw new ArgumentException("ID bác sĩ không hợp lệ", nameof(doctorId));
            }

            PatientFilter filter = new PatientFilter();
            filter.DoctorId = doctorId;

            DateTime today = DateTime.Today;
            filter.FromDate = today;
            filter.ToDate = today.AddDays(1).AddTicks(-1); // Cuối ngày hôm nay

            return await GetPatientsByDoctorAsync(filter);
        }
    }
}