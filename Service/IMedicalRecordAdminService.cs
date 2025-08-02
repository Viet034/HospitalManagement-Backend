namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IMedicalRecordAdminService
    {
        List<MedicalRecordAdminResponse> GetAllMedicalRecords(DateTime? startDate = null, DateTime? endDate = null);
        List<MedicalRecordAdminResponse> GetMedicalRecordsByDoctorName(string doctorName);
    }
}
