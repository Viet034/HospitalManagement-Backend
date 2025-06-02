using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO.Impl
{
    public class DoctorMapper : IDoctorMapper
    {
        public DoctorResponseDTO MapToDTO(Doctor doctor)
        {
            return new DoctorResponseDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Code = doctor.Code,
                Gender = doctor.Gender,
                Dob = doctor.Dob,
                CCCD = doctor.CCCD,
                Phone = doctor.Phone,
                ImageURL = doctor.ImageURL,
                LicenseNumber = doctor.LicenseNumber,
                YearOfExperience = doctor.YearOfExperience,
                WorkingHours = doctor.WorkingHours,
                Status = doctor.Status,
                UserId = doctor.UserId,
                DepartmentId = doctor.DepartmentId,
                CreateDate = doctor.CreateDate,
                UpdateDate = doctor.UpdateDate,
                CreateBy = doctor.CreateBy,
                UpdateBy = doctor.UpdateBy
            };
        }

        public Doctor MapToEntity(DoctorCreate doctorDTO)
        {
            return new Doctor
            {
                Name = doctorDTO.Name,
                Code = doctorDTO.Code,
                Gender = doctorDTO.Gender,
                Dob = doctorDTO.Dob,
                CCCD = doctorDTO.CCCD,
                Phone = doctorDTO.Phone,
                ImageURL = doctorDTO.ImageURL,
                LicenseNumber = doctorDTO.LicenseNumber,
                YearOfExperience = doctorDTO.YearOfExperience,
                WorkingHours = doctorDTO.WorkingHours,
                Status = doctorDTO.Status,
                UserId = doctorDTO.UserId,
                DepartmentId = doctorDTO.DepartmentId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateBy = doctorDTO.CreateBy,
                UpdateBy = null
            };
        }

        public Doctor MapToEntity(DoctorUpdate doctorDTO)
        {
            return new Doctor
            {
                Id = doctorDTO.Id,
                Name = doctorDTO.Name,
                Code = doctorDTO.Code,
                Gender = doctorDTO.Gender,
                Dob = doctorDTO.Dob,
                CCCD = doctorDTO.CCCD,
                Phone = doctorDTO.Phone,
                ImageURL = doctorDTO.ImageURL,
                LicenseNumber = doctorDTO.LicenseNumber,
                YearOfExperience = doctorDTO.YearOfExperience,
                WorkingHours = doctorDTO.WorkingHours,
                Status = doctorDTO.Status,
                UserId = doctorDTO.UserId,
                DepartmentId = doctorDTO.DepartmentId,
                UpdateDate = DateTime.Now,
                UpdateBy = doctorDTO.UpdateBy
            };
        }
    }
}