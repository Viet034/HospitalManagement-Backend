using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.AppointmentVer2;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IAppoinrmentServicever2
    {
        public Task<IEnumerable<AppointmentResponseDTOVer2>> GetAllAppointment();
        public Task<IEnumerable<AppointmentResponseDTOVer2>> SearchByKey(string key);
        public Task<IEnumerable<AppointmentResponseDTOVer2>> SearchByID(int Id);
        public Task<IEnumerable<AppointmentResponseDTOVer2>> CreateAppoiment(AppointmentCreate create);
        public Task<AppointmentResponseDTOVer2> UpdateApointmentStatus(int id, AppointmentStatus newStatus);

    }
}
