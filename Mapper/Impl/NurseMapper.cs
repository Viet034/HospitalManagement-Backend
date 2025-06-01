using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public class NurseMapper : INurseMapper
    {
        public NurseDTO MapToDto(Nurse nurse)
        {
            if (nurse == null) return null;
            return new NurseDTO
            {
                Id = nurse.Id,
                Name = nurse.Name,
                Code = nurse.Code,
                Gender = nurse.Gender,
                Dob = nurse.Dob,
                CCCD = nurse.CCCD,
                Phone = nurse.Phone,
                ImageURL = nurse.ImageURL,
                Status = nurse.Status,
                UserId = nurse.UserId,
                DepartmentId = nurse.DepartmentId,
                CreateDate = nurse.CreateDate,
                UpdateDate = nurse.UpdateDate,
                CreateBy = nurse.CreateBy,
                UpdateBy = nurse.UpdateBy
            };
        }

        public Nurse MapToEntity(NurseDTO nurseDto)
        {
            if (nurseDto == null) return null;
            return new Nurse
            {
                Id = nurseDto.Id,
                Name = nurseDto.Name,
                Code = nurseDto.Code,
                Gender = nurseDto.Gender,
                Dob = nurseDto.Dob,
                CCCD = nurseDto.CCCD,
                Phone = nurseDto.Phone,
                ImageURL = nurseDto.ImageURL,
                Status = nurseDto.Status,
                UserId = nurseDto.UserId,
                DepartmentId = nurseDto.DepartmentId,
                CreateDate = nurseDto.CreateDate,
                UpdateDate = nurseDto.UpdateDate,
                CreateBy = nurseDto.CreateBy,
                UpdateBy = nurseDto.UpdateBy
            };
        }

        public IEnumerable<NurseDTO> MapToDtoList(IEnumerable<Nurse> nurses)
        {
            return nurses?.Select(n => MapToDto(n)).ToList() ?? new List<NurseDTO>();
        }
    }
}