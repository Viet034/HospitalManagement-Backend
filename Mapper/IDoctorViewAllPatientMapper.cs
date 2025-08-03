using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using System.Collections.Generic;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface IDoctorViewAllPatientMapper
    {
        DoctorViewAllPatientResponse EntityToDoctorViewAllPatientResponse(Appointment entity);
        IEnumerable<DoctorViewAllPatientResponse> ListEntityToDoctorViewAllPatientResponse(IEnumerable<Appointment> entities);
    }
}