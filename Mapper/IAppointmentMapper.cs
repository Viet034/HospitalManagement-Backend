using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Appointment;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper;

public interface IAppointmentMapper
{
    // Request -> Entity
    Appointment CreateToEntity(AppointmentCreate create);
    void UpdateEntityFromDto(AppointmentUpdate update, Appointment entity);
    Appointment DeleteToEntity(AppointmentDelete delete);

    // Entity -> Response
    AppointmentResponseDTO EntityToResponse(Appointment entity);
    IEnumerable<AppointmentResponseDTO> ListEntityToResponse(IEnumerable<Appointment> entities);
} 