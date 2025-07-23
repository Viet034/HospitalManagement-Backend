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

        public DoctorResponseDTO MapToDto(Doctor doctor)
        {
            DoctorResponseDTO response = new DoctorResponseDTO();
            response.Id = doctor.Id;
            response.Name = doctor.Name;
            response.Code = doctor.Code;
            response.Gender = (Gender)doctor.Gender;
            response.Dob = doctor.Dob;
            response.CCCD = doctor.CCCD;
            response.Phone = doctor.Phone;
            response.ImageURL = doctor.ImageURL;
            response.LicenseNumber = doctor.LicenseNumber;
            response.YearOfExperience = doctor.YearOfExperience;
            response.WorkingHours = doctor.WorkingHours;
            response.Status = (DoctorStatus)doctor.Status;
            response.UserId = doctor.UserId;
            response.CreateDate = doctor.CreateDate;
            response.UpdateDate = doctor.UpdateDate;
            response.CreateBy = doctor.CreateBy;
            response.UpdateBy = doctor.UpdateBy;

            return response;
        }

        public IEnumerable<DoctorResponseDTO> MapToDtoList(IEnumerable<Doctor> doctors)
        {
            List<DoctorResponseDTO> responses = new List<DoctorResponseDTO>();

            foreach (Doctor doctor in doctors)
            {
                DoctorResponseDTO response = new DoctorResponseDTO();
                response.Id = doctor.Id;
                response.Name = doctor.Name;
                response.Code = doctor.Code;
                response.Gender = (Gender)doctor.Gender;
                response.Dob = doctor.Dob;
                response.CCCD = doctor.CCCD;
                response.Phone = doctor.Phone;
                response.ImageURL = doctor.ImageURL;
                response.LicenseNumber = doctor.LicenseNumber;
                response.YearOfExperience = doctor.YearOfExperience;
                response.WorkingHours = doctor.WorkingHours;
                response.Status = (DoctorStatus)doctor.Status;
                response.UserId = doctor.UserId;
                response.CreateDate = doctor.CreateDate;
                response.UpdateDate = doctor.UpdateDate;
                response.CreateBy = doctor.CreateBy;
                response.UpdateBy = doctor.UpdateBy;

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


        public Doctor MapToEntity(DoctorCreate create)
        {
            Doctor doctor = new Doctor();
            doctor.Id = create.Id;
            doctor.Name = create.Name;
            doctor.Code = create.Code;
            doctor.Gender = (Gender)create.Gender;
            doctor.Dob = create.Dob;
            doctor.CCCD = create.CCCD;
            doctor.Phone = create.Phone;
            doctor.ImageURL = create.ImageURL;
            doctor.LicenseNumber = create.LicenseNumber;
            doctor.YearOfExperience = create.YearOfExperience;
            doctor.WorkingHours = create.WorkingHours;
            doctor.Status = (DoctorStatus)create.Status;
            doctor.UserId = create.UserId;
            doctor.CreateDate = create.CreateDate;
            doctor.UpdateDate = create.UpdateDate;
            doctor.CreateBy = create.CreateBy;
            doctor.UpdateBy = create.UpdateBy;

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


        public void MapToEntity(DoctorUpdate update, Doctor doctor)
        {
            doctor.Id = update.Id;
            doctor.Name = update.Name;
            doctor.Code = update.Code;
            doctor.Gender = update.Gender.HasValue ? (Gender)update.Gender.Value : doctor.Gender;
            doctor.Dob = update.Dob.HasValue ? update.Dob.Value : doctor.Dob;
            doctor.CCCD = update.CCCD;
            doctor.Phone = update.Phone;
            doctor.ImageURL = update.ImageURL;
            doctor.LicenseNumber = update.LicenseNumber;
            doctor.YearOfExperience = update.YearOfExperience.HasValue ? update.YearOfExperience.Value : doctor.YearOfExperience;
            doctor.WorkingHours = update.WorkingHours.HasValue ? update.WorkingHours.Value : doctor.WorkingHours;
            doctor.Status = update.Status.HasValue ? (DoctorStatus)update.Status.Value : doctor.Status;
            doctor.UserId = update.UserId.HasValue ? update.UserId.Value : doctor.UserId;
            doctor.CreateDate = update.CreateDate.HasValue ? update.CreateDate.Value : doctor.CreateDate;
            doctor.UpdateDate = update.UpdateDate.HasValue ? update.UpdateDate.Value : doctor.UpdateDate;
            doctor.CreateBy = update.CreateBy;
            doctor.UpdateBy = update.UpdateBy;
        }

    }
}