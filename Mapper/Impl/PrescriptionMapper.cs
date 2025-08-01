﻿using static SWP391_SE1914_ManageHospital.Ultility.Status;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Prescription;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

public class PrescriptionMapper : IPrescriptionMapper
{
    public PrescriptionResponseDTO MapToResponse(Prescription prescription)
    {
        return new PrescriptionResponseDTO
        {
            Id = prescription.Id,
            Note = prescription.Note,
            Status = (PrescriptionStatus)prescription.Status,
            PatientId = prescription.PatientId,
            DoctorId = prescription.DoctorId,
            Name = prescription.Name,
            Code = prescription.Code,
            Amount = prescription.Amount,
            CreateDate = prescription.CreateDate,
            UpdateDate = prescription.UpdateDate ?? DateTime.UtcNow,
            CreateBy = prescription.CreateBy ?? "system",
            UpdateBy = prescription.UpdateBy ?? "system"
        };
    }

    public Prescription MapToEntity(PrescriptionRequest request)
    {
        return new Prescription
        {
            Note = request.Note,
            
            Name = request.Name,
            
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
            CreateBy = "system",
            UpdateBy = "system"
        };
    }

    public void MapToExistingEntity(PrescriptionRequest request, Prescription prescription)
    {
        prescription.Note = request.Note;
        
        prescription.Name = request.Name;
        
        prescription.UpdateDate = DateTime.UtcNow;
        prescription.UpdateBy = "system";
    }
}
