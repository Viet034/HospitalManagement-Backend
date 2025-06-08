using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class MedicalRecordDetailMapper : IMedicalRecordDetailMapper
    {
        public MedicalRecordDetailResponse EntityToDetailResponse(Medical_Record entity)
        {
            return new MedicalRecordDetailResponse
            {
                Id = entity.Id,
                Diagnosis = entity.Diagnosis,
                TestResults = entity.TestResults,
                Notes = entity.Notes,
                Status = entity.Status.ToString(),
                CreateDate = entity.CreateDate,
                DoctorName = entity.Doctor?.Name ?? "",
                PatientName = entity.Patient?.Name ?? "",
                DiseaseName = entity.Disease?.Name ?? "",
                AppointmentId = entity.AppointmentId,
                PrescriptionId = entity.PrescriptionId
            };
        }
    }
}