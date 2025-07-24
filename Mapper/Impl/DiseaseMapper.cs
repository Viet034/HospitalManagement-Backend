using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Disease;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using System;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class DiseaseMapper : IDiseaseMapper
    {
        public DiseaseResponse MapToResponse(DiseaseDTO entity)
        {
            if (entity == null)
                return null;

            return new DiseaseResponse(
                entity.Id,
                entity.Name,
                entity.Code,
                entity.Description,
                entity.Status,
                entity.CreateDate,
                entity.UpdateDate,
                entity.CreateBy,
                entity.UpdateBy
            );
        }

        public Disease MapCreateRequestToEntity(DiseaseCreateRequest request)
        {
            return new Disease
            {
                Name = request.Name,
                Code = GenerateDiseaseCode(),
                Description = request.Description,
                Status = request.Status,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7),
                CreateBy = request.CreateBy,
                UpdateBy = request.CreateBy
            };
        }

        public void MapUpdateRequestToEntity(Disease entity, DiseaseUpdateRequest request)
        {
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Status = request.Status;
            entity.UpdateDate = DateTime.UtcNow.AddHours(7);
            entity.UpdateBy = request.UpdateBy;
        }
        private string GenerateDiseaseCode()
        {
            return $"DS-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6]}";
        }
    }
}