﻿using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO
{
    public class MedicineResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public MedicineStatus Status { get; set; }
        public int MedicineCategoryId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string? UpdateBy { get; set; }
    }
}
