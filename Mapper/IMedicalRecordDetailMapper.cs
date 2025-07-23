using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicalRecord;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface IMedicalRecordDetailMapper
    {
        MedicalRecordDetailResponse EntityToDetailResponse(Medical_Record entity);
        IEnumerable<MedicalRecordDetailResponse> ListEntityToResponse(IEnumerable<Medical_Record> entities);
        Medical_Record CreateRequestToEntity(MedicalRecordCreateRequest request, string doctorName);
        Medical_Record DeleteRequestToEntity(MedicalRecordDeleteRequest request);
        void UpdateEntityFromRequest(Medical_Record entity, MedicalRecordUpdateRequest request, string doctorName);
    }
}