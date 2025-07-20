﻿using System;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.ShiftRequest
{
    public class ShiftRequestRequestDTO
    {
        public int DoctorId { get; set; }
        public int ShiftId { get; set; }
        public ShiftRequestType RequestType { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}