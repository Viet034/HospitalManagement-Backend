using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.AppointmentVer2;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class AppointmentMapperVer2 : IAppointmentMapperVer2
    {
        public Appointment CreateToEntity(AppointmentCreate create)
        {
            Appointment appointment = new Appointment();
            appointment.AppointmentDate = create.AppointmentDate;
            appointment.StartTime = create.StartTime;
            //appointment.EndTime = create.EndTime;
            appointment.PatientId = create.PatientId;           
            appointment.ClinicId = create.ClinicId;
            appointment.ServiceId = create.ServiceId;   
            return appointment;

        }

        public AppointmentResponseDTOVer2 EntityToRespone(Appointment entity)
        {
            AppointmentResponseDTOVer2 apm = new AppointmentResponseDTOVer2();
            apm.StartTime = TimeSpan.FromHours(entity.StartTime.Hours);
            apm.Status = entity.Status;
            return apm;

        }

        public IEnumerable<AppointmentResponseDTOVer2> ListEntityToResponse(IEnumerable<Appointment> entities)
        {
            throw new NotImplementedException();
        }

        public Appointment UpdateToEntity(AppointmentDestroy update)
        {
            throw new NotImplementedException();
        }
    }
}
