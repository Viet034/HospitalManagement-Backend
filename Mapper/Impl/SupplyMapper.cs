using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supply;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class SupplyMapper : ISupplyMapper
    {
        public SupplyResponseDTO ToResponseDTO(Supply entity)
        {
            return new SupplyResponseDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Status = entity.Status,
                Description = entity.Description,
                Unit = (string)entity.Unit,
                AppointmentId = entity.AppointmentId,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
                CreateBy = entity.CreateBy,
                UpdateBy = entity.UpdateBy
            };
        }

        public IEnumerable<SupplyResponseDTO> ListEntityToResponse(IEnumerable<Supply> entities)
        {
            return entities.Select(ToResponseDTO);
        }

        public Supply ToEntity(SupplyCreate dto)
        {
            return new Supply
            {
                Name = dto.Name,
                Code = dto.Code,
                Status = dto.Status,
                Description = dto.Description,
                Unit = dto.Unit,
                AppointmentId = dto.AppointmentId,
                CreateDate = dto.CreateDate,
                CreateBy = dto.CreateBy
            };
        }

        public void UpdateEntity(Supply entity, SupplyUpdate dto)
        {
            entity.Name = dto.Name;
            entity.Code = dto.Code;
            entity.Status = dto.Status;
            entity.Description = dto.Description;
            entity.Unit = dto.Unit;
            entity.AppointmentId = dto.AppointmentId;
            entity.UpdateDate = dto.UpdateDate;
            entity.UpdateBy = dto.UpdateBy;
        }
    }
}
