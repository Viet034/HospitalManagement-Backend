using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Department;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Doctor;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl;

public class DoctorMapper : IDoctorMapper
{
    public Doctor CreateToEntity(DoctorCreate create)
    {
        throw new NotImplementedException();
    }

    public Doctor DeleteToEntity(DoctorDelete delete)
    {
        Doctor doctor = new Doctor();
        doctor.Code = delete.Code;
        doctor.Name = delete.Name;
        doctor.CCCD = delete.CCCD;
        doctor.Status = delete.Status;
        doctor.CreateDate = delete.CreateDate;
        doctor.UpdateDate = delete.UpdateDate;
        doctor.CreateBy = delete.CreateBy;
        doctor.UpdateBy = delete.UpdateBy;
        return doctor;
    }

    public DoctorResponseDTO EntityToResponse(Doctor entity)
    {
        DoctorResponseDTO response = new DoctorResponseDTO();
        response.Id = entity.Id;
        response.Name = entity.Name;
        response.Code = entity.Code;
        response.CreateDate = entity.CreateDate;
        response.UpdateDate = entity.UpdateDate;
        response.Gender = entity.Gender;
        response.Dob = entity.Dob;
        response.CCCD = entity.CCCD;
        response.ImageURL = entity.ImageURL;
        response.Phone = entity.Phone;
        response.LicenseNumber = entity.LicenseNumber;
        response.YearOfExperience = entity.YearOfExperience;
        response.WorkingHours = entity.WorkingHours;
        response.Status = entity.Status;
        response.UserId = entity.UserId;
        response.DepartmentId = entity.DepartmentId;
        response.UserId = entity.UserId;
        response.CreateBy = entity.CreateBy;
        response.UpdateBy = entity.UpdateBy;
        return response;
    }

    public IEnumerable<DoctorResponseDTO> ListEntityToResponse(IEnumerable<Doctor> entities)
    {
        return entities.Select(x => EntityToResponse(x)).ToList();
    }

    public Doctor UpdateToEntity(DoctorUpdate update)
    {
        Doctor doctor = new Doctor();
        
        doctor.Name = update.Name;
        doctor.Code = update.Code;
        doctor.CreateDate = update.CreateDate;
        doctor.UpdateDate = update.UpdateDate;
        doctor.Gender = update.Gender;
        doctor.Dob = update.Dob;
        doctor.CCCD = update.CCCD;
        doctor.Phone = update.Phone;
        doctor.ImageURL = update.ImageURL;
        doctor.LicenseNumber = update.LicenseNumber;
        doctor.YearOfExperience = update.YearOfExperience;
        doctor.WorkingHours = update.WorkingHours;
        doctor.Status = update.Status;
        
        doctor.DepartmentId = update.DepartmentId;
        doctor.CreateBy = update.CreateBy;
        doctor.UpdateBy = update.UpdateBy;
        return doctor;

    }
}