using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl;

public class DoctorMapper : IDoctorMapper
{
    public DoctorResponseDTO EntityToResponse(Doctor entity)
    {
        return new DoctorResponseDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            Gender = entity.Gender,
            Dob = entity.Dob,
            Phone = entity.Phone,
            ImageURL = entity.ImageURL,
            LicenseNumber = entity.LicenseNumber,
            YearOfExperience = entity.YearOfExperience,
            WorkingHours = entity.WorkingHours,
            Status = entity.Status,
            DepartmentId = entity.DepartmentId,
            DepartmentName = entity.Department?.Name ?? "",
            CreateDate = entity.CreateDate,
            UpdateDate = entity.UpdateDate,
            CreateBy = entity.CreateBy,
            UpdateBy = entity.UpdateBy
        };
    }

    public IEnumerable<DoctorResponseDTO> ListEntityToResponse(IEnumerable<Doctor> entities)
    {
        return entities.Select(x => EntityToResponse(x)).ToList();
    }
} 