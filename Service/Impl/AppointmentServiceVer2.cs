using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.AppointmentVer2;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class AppointmentServiceVer2 : IAppoinrmentServicever2
    {
        private readonly ApplicationDBContext _context;
        private readonly IAppointmentMapperVer2 _mapper;

        public AppointmentServiceVer2(ApplicationDBContext context, IAppointmentMapperVer2 mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentResponseDTOVer2>> CreateAppoiment(AppointmentCreate create)
        {
            try
            {
                // Kiểm tra lịch làm việc bác sĩ
                var doctorShift = await _context.Doctor_Shifts
                    .Where(ds => ds.DoctorId == create.DoctorId && ds.ShiftDate.Date == create.AppointmentDate.Date)
                    .Where(ds => ds.ShiftType == create.ShiftType)
                    .FirstOrDefaultAsync();

                if (doctorShift == null)
                    throw new Exception("Bác sĩ không có lịch làm việc vào ngày và ca này.");

                // Kiểm tra lịch hẹn bác sĩ
                var existingDoctorAppointment = await _context.Doctor_Appointments
                    .Where(da => da.DoctorId == create.DoctorId && da.Status == (int)AppointmentStatus.Scheduled)
                    .Join(_context.Appointments, da => da.AppointmentId, a => a.Id, (da, a) => new { da, a })
                    .Where(x => x.a.AppointmentDate.Date == create.AppointmentDate.Date && x.a.StartTime == create.StartTime)
                    .FirstOrDefaultAsync();

                if (existingDoctorAppointment != null)
                    throw new Exception("Bác sĩ đã có lịch hẹn vào thời gian này.");

                // Kiểm tra lịch hẹn phòng khám
                var existingRoomAppointment = await _context.Appointments
                    .Where(a => a.ClinicId == create.ClinicId && a.Status == (int)AppointmentStatus.Scheduled)
                    .Where(a => a.AppointmentDate.Date == create.AppointmentDate.Date && a.StartTime == create.StartTime)
                    .FirstOrDefaultAsync();

                if (existingRoomAppointment != null)
                    throw new Exception("Phòng này đã có lịch hẹn vào thời gian này. Vui lòng chọn phòng khác.");

                // Tạo Appointment
                var appointmentCode = await GenerateUniqueAppointmentCodeAsync();
                //var note = string.IsNullOrWhiteSpace(create.Note) ? "Không có ghi chú" : create.Note;

                var appointment = new Appointment
                {
                    AppointmentDate = create.AppointmentDate,
                    StartTime = create.StartTime,
                    EndTime = null,
                    Status = AppointmentStatus.Scheduled,
                    Note = create.Note,
                    PatientId = create.PatientId,
                    ClinicId = create.ClinicId,
                    ServiceId = create.ServiceId,
                    ReceptionId = null,
                    Name = create?.Name ?? "Chưa thêm",
                    Code = appointmentCode,
                    isSend = false,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateBy = "System",
                    UpdateBy = "System"
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync(); // Tạo Appointment trước

                var appointmentId = appointment.Id;

                // Tạo Invoice
                var invoiceCode = await GenerateUniqueInvoiceCodeAsync();
                var invoice = new Invoice
                {
                    AppointmentId = appointmentId,
                    PatientId = create?.PatientId ?? 1,
                    InitialAmount = 0,
                    DiscountAmount = 0,
                    TotalAmount = 0,
                    Status = InvoiceStatus.Unpaid,
                    InsuranceId = 1,
                    Name = "Chưa thêm",
                    Code = invoiceCode,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateBy = "System",
                    UpdateBy = "System"
                };

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

      

                // Tạo Doctor_Appointment
                var doctorAppointment = new Doctor_Appointment
                {
                    AppointmentId = appointmentId,
                    DoctorId = create?.DoctorId ?? 1,
                    Status = DoctorAppointmentStatus.Assigned
                };

                _context.Doctor_Appointments.Add(doctorAppointment);
                await _context.SaveChangesAsync();

                // Trả kết quả
                var appointmentResponse = new List<AppointmentResponseDTOVer2>
        {
            new AppointmentResponseDTOVer2
            {
                AppointmentDate = appointment.AppointmentDate,
                StartTime = appointment.StartTime,
                Status = (AppointmentStatus)appointment.Status,
                PatientId = appointment.PatientId,
                ClinicId = appointment.ClinicId,
                ServiceId = appointment.ServiceId ?? 0,
                ReceptionId = appointment.ReceptionId,
                Name = appointment.Name,
                Code = appointment.Code,
                DoctorId = doctorAppointment.DoctorId
            }
        };

                return appointmentResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo lịch hẹn: " + ex.Message, ex);
            }
        }





        public async Task<string> GenerateUniqueMedicalRecordCodeAsync()
        {
            string code = "";
            bool isUnique = false;


            while (!isUnique)
            {

                code = GenerateCode.GenerateMedicalRecordCode();


                var existingAppointment = await _context.Appointments
                    .Where(a => a.Code == code)
                    .FirstOrDefaultAsync();


                if (existingAppointment == null)
                {
                    isUnique = true;
                }
            }

            return code;
        }
        public async Task<string> GenerateUniqueAppointmentCodeAsync()
        {
            string code = "";
            bool isUnique = false;


            while (!isUnique)
            {

                code = GenerateCode.GenerateAppointmentCode();


                var existingAppointment = await _context.Appointments
                    .Where(a => a.Code == code)
                    .FirstOrDefaultAsync();


                if (existingAppointment == null)
                {
                    isUnique = true;
                }
            }

            return code;
        }
        public async Task<string> GenerateUniqueInvoiceCodeAsync()
        {
            string code = "";
            bool isUnique = false;


            while (!isUnique)
            {

                code = GenerateCode.GenerateInvoiceCode();


                var existingAppointment = await _context.Appointments
                    .Where(a => a.Code == code)
                    .FirstOrDefaultAsync();


                if (existingAppointment == null)
                {
                    isUnique = true;
                }
            }

            return code;
        }

        public Task<IEnumerable<AppointmentResponseDTOVer2>> GetAllAppointment()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppointmentResponseDTOVer2>> SearchByID(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppointmentResponseDTOVer2>> SearchByKey(string key)
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentResponseDTOVer2> UpdateApointmentStatus(int id, Status.AppointmentStatus newStatus)
        {
            throw new NotImplementedException();
        }
    }
}
