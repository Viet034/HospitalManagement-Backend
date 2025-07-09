// Mapper/Impl/PrescriptionDetailMapper.cs
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.PrescriptionDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class PrescriptionDetailMapper : IPrescriptionDetailMapper
    {
        public PrescriptionDetailResponseDTO MapToResponse(PrescriptionDetail e) => new()
        {
            Id = e.Id,
            PrescriptionId = e.PrescriptionId,
            MedicineId = e.MedicineId,
            Quantity = e.Quantity,
            Usage = e.Usage,
            Status = e.Status.ToString(),
            CreateDate = e.CreateDate,
            UpdateDate = e.UpdateDate,
            CreateBy = e.CreateBy!,
            UpdateBy = e.UpdateBy
        };

        public PrescriptionDetail MapToEntity(PrescriptionDetailRequest req, string userName) => new()
        {
            PrescriptionId = req.PrescriptionId,
            MedicineId = req.MedicineId,
            Quantity = req.Quantity,
            Usage = req.Usage,
            Status = (SWP391_SE1914_ManageHospital.Ultility.Status.PrescriptionDetailStatus)req.Status,
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
            CreateBy = userName,
            UpdateBy = userName
        };

        public void MapToExistingEntity(PrescriptionDetailRequest req, PrescriptionDetail e, string userName)
        {
            e.MedicineId = req.MedicineId;
            e.Quantity = req.Quantity;
            e.Usage = req.Usage;
            e.Status = (SWP391_SE1914_ManageHospital.Ultility.Status.PrescriptionDetailStatus)req.Status;
            e.UpdateDate = DateTime.UtcNow;
            e.UpdateBy = userName;
        }
    }
}
