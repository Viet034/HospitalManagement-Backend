using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Clinic;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper;

public interface ILaboratoryStaffMapper
{
    LaboratoryStaffDTO ToDTO(LaboratoryStaff entity);
    //LaboratoryStaff ToEntity(LaboratoryStaffRequestDTO dto);
}
