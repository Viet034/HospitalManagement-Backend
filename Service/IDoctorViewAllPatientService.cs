using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using System.Collections.Generic;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IDoctorViewAllPatientService
    {
        IEnumerable<DoctorViewAllPatientResponse> GetAllPatientsByDoctorId(int doctorId);
    }
}