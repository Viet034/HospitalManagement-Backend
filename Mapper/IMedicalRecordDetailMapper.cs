using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface IMedicalRecordDetailMapper
    {
        MedicalRecordDetailResponse EntityToDetailResponse(Medical_Record entity);
    }
}