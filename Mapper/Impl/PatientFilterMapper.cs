using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.Helps;
using System;
using System.Collections.Generic;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class PatientFilterMapper : IPatientFilterMapper
    {
        public PatientFilterResponse EntityToResponse(Patient entity, DateTime examinationTime, string appointmentStatus)
        {
            PatientFilterResponse response = new PatientFilterResponse();
            response.Id = entity.Id;
            response.Name = entity.Name;
            response.ExaminationTime = examinationTime;
            response.AppointmentStatus = appointmentStatus;

            return response;
        }

        public List<PatientFilterResponse> ListEntityToResponse(List<PatientAppointmentData> patientDataList)
        {
            List<PatientFilterResponse> result = new List<PatientFilterResponse>();

            foreach (PatientAppointmentData data in patientDataList)
            {
                PatientFilterResponse response = EntityToResponse(
                    data.Patient,
                    data.ExaminationTime,
                    data.AppointmentStatus);

                result.Add(response);
            }

            return result;
        }
    }
}