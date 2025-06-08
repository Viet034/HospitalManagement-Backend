using Microsoft.AspNetCore.Http.HttpResults;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Patient;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class PatientMapper : IpatientMapper
    {
        public Patient CreateToEntity(PatientCreate create)
        {
            Patient patient = new Patient();
            patient.Name = create.Name;
            patient.Code = create.Code;
            patient.Gender = create.Gender;
            patient.Dob = create.Dob;
            patient.CCCD = create.CCCD;
            patient.Phone = create.Phone;
            patient.EmergencyContact = create.EmergencyContact;
            patient.Address = create.Address;
            patient.InsuranceNumber = create.InsuranceNumber;
            patient.Allergies = create.Allergies;
            patient.Status = create.Status;
            patient.BloodType = create.BloodType;
            patient.ImageURL = create.ImageURL;
            patient.UserId = create.UserId;
            patient.CreateDate = create.CreateDate;
            patient.UpdateDate = create.UpdateDate;
            patient.CreateBy = create.CreateBy;
            patient.UpdateBy = create.UpdateBy;

            return patient;
        }

        public Patient DeleteToEntity(PatientDelete delete)
        {
            Patient patient = new Patient();
            patient.Id = delete.Id;
            patient.Name = delete.Name;
            patient.Code = delete.Code;
            patient.Gender = delete.Gender;
            patient.Dob = delete.Dob;
            patient.CCCD = delete.CCCD;
            patient.Phone = delete.Phone;
            patient.EmergencyContact = delete.EmergencyContact;
            patient.Address = delete.Address;
            patient.InsuranceNumber = delete.InsuranceNumber;
            patient.Allergies = delete.Allergies;
            patient.Status = delete.Status;
            patient.BloodType = delete.BloodType;
            patient.ImageURL = delete.ImageURL;
            patient.UserId = delete.UserId;
            patient.CreateDate = delete.CreateDate;
            patient.UpdateDate = delete.UpdateDate;
            patient.CreateBy = delete.CreateBy;
            patient.UpdateBy = delete.UpdateBy;

            return patient;
        }



        public PatientRespone EntityToRespone(Patient entity)
        {
            PatientRespone respone = new PatientRespone();
            respone.Id = entity.Id;
            respone.Name = entity.Name;
            respone.Code = entity.Code;
            respone.Gender = entity.Gender;
            respone.Dob = entity.Dob;
            respone.CCCD = entity.CCCD;
            respone.Phone = entity.Phone;
            respone.EmergencyContact = entity.EmergencyContact;
            respone.Address = entity.Address;
            respone.InsuranceNumber = entity.InsuranceNumber;
            respone.Allergies = entity.Allergies;
            respone.Status = entity.Status;
            respone.BloodType = entity.BloodType;
            respone.ImageURL = entity.ImageURL;
            respone.UserId = entity.UserId;
            respone.CreateDate = entity.CreateDate;
            respone.UpdateDate = entity.UpdateDate;
            respone.CreateBy = entity.CreateBy;
            respone.UpdateBy = entity.UpdateBy;

            return respone;
        }

        public IEnumerable<PatientRespone> ListEntityToRespone(IEnumerable<Patient> entities)
        {
            return entities.Select(e => EntityToRespone(e)).ToList();
        }

        public Patient UpdateToEntity(PatientUpdate update)
        {
            Patient patient = new Patient();
            patient.Id = update.Id;
            patient.Name = update.Name;
            patient.Code = update.Code;
            patient.Gender = update.Gender;
            patient.Dob = update.Dob;
            patient.CCCD = update.CCCD;
            patient.Phone = update.Phone;
            patient.EmergencyContact = update.EmergencyContact;
            patient.Address = update.Address;
            patient.InsuranceNumber = update.InsuranceNumber;
            patient.Allergies = update.Allergies;
            patient.Status = update.Status;
            patient.BloodType = update.BloodType;
            patient.ImageURL = update.ImageURL;
            patient.UserId = update.UserId;
            patient.CreateDate = update.CreateDate;
            patient.UpdateDate = update.UpdateDate;
            patient.CreateBy = update.CreateBy;
            patient.UpdateBy = update.UpdateBy;

            return patient;
        }
    }
}
