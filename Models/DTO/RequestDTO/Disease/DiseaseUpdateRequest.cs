﻿using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Disease
{
    public class DiseaseUpdateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DiseaseStatus Status { get; set; }
        public string UpdateBy { get; set; }

        public DiseaseUpdateRequest()
        {
        }

        public DiseaseUpdateRequest(
            string name,
            string description,
            DiseaseStatus status,
            string updateBy)
        {
            Name = name;
            Description = description;
            Status = status;
            UpdateBy = updateBy;
        }
    }
}