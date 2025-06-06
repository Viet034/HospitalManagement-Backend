using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IMedicalRecordService
    {
        IEnumerable<MedicalRecordResponse> GetMedicalRecordsByPatientId(int patientId);
        MedicalRecordResponse? GetMedicalRecordDetail(int medicalRecordId);
    }
}