using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.AppointmentVer2;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface IAppointmentMapperVer2
    {
        Appointment CreateToEntity(AppointmentCreate create);
        Appointment UpdateToEntity(AppointmentDestroy update);
        AppointmentResponseDTOVer2 EntityToRespone(Appointment entity);
        IEnumerable<AppointmentResponseDTOVer2> ListEntityToResponse(IEnumerable<Appointment> entities);
    }
}
