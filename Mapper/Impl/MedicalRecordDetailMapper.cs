using CloudinaryDotNet.Actions;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http.HttpResults;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicalRecord;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class MedicalRecordDetailMapper : IMedicalRecordDetailMapper
    {
        public MedicalRecordDetailResponse EntityToDetailResponse(Medical_Record entity)
        {
            return new MedicalRecordDetailResponse(
                id: entity.Id,
                diagnosis: entity.Diagnosis,
                testResults: entity.TestResults,
                notes: entity.Notes,
                status: entity.Status.ToString(),
                appointmentId: entity.AppointmentId,
                patientId: entity.PatientId,
                doctorId: entity.DoctorId,
                prescriptionId: entity.PrescriptionId,
                diseaseId: entity.DiseaseId,
                name: entity.Name,
                code: entity.Code,
                createDate: entity.CreateDate,
                updateDate: entity.UpdateDate,
                createBy: entity.CreateBy,
                updateBy: entity.UpdateBy,
                doctorName: entity.Doctor?.Name ?? "",
                patientName: entity.Patient?.Name ?? "",
                diseaseName: entity.Disease?.Name ?? ""
            );
        }

        public IEnumerable<MedicalRecordDetailResponse> ListEntityToResponse(IEnumerable<Medical_Record> entities)
        {
            return entities.Select(x => EntityToDetailResponse(x)).ToList();
        }

        public Medical_Record CreateRequestToEntity(MedicalRecordCreateRequest request, string doctorName)
        {
            return new Medical_Record
            {
                Diagnosis = request.Diagnosis,
                TestResults = request.TestResults,
                Notes = request.Notes,
                Status = Enum.TryParse<MedicalRecordStatus>(request.Status, out var status) ? status : MedicalRecordStatus.Open,
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                DiseaseId = request.DiseaseId,
                AppointmentId = request.AppointmentId,
                PrescriptionId = request.PrescriptionId,
                Name = request.Diagnosis,
                Code = GenerateMedicalRecordCode(),
                CreateDate = DateTime.UtcNow.AddHours(7),
                CreateBy = doctorName,
                UpdateDate = DateTime.UtcNow.AddHours(7),
                UpdateBy = doctorName
            };
        }

        public Medical_Record DeleteRequestToEntity(MedicalRecordDeleteRequest request)
        {
            return new Medical_Record
            {
                Id = request.Id
            };
        }

        public void UpdateEntityFromRequest(Medical_Record entity, MedicalRecordUpdateRequest request, string doctorName)
        {
            entity.Diagnosis = request.Diagnosis;
            entity.TestResults = request.TestResults;
            entity.Notes = request.Notes;
            entity.Status = Enum.TryParse<MedicalRecordStatus>(request.Status, out var status) ? status : entity.Status;
            entity.DoctorId = request.DoctorId;
            entity.PatientId = request.PatientId;
            entity.DiseaseId = request.DiseaseId;
            entity.AppointmentId = request.AppointmentId;
            entity.PrescriptionId = request.PrescriptionId;
            entity.Name = request.Diagnosis;
            entity.UpdateDate = DateTime.UtcNow.AddHours(7);
            entity.UpdateBy = doctorName;
        }

        private string GenerateMedicalRecordCode()
        {
            return $"MR-{DateTime.UtcNow.Ticks}";
        }
    }
}