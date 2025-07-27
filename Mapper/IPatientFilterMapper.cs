using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PatientFilter;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using System.Collections.Generic;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface IPatientFilterMapper
    {
        PatientFilterResponse EntityToResponse(Appointment appointment, int doctorId = 0, string doctorName = "");
        List<PatientFilterResponse> ListEntityToResponse(List<Appointment> appointments, int doctorId = 0);
    }
}