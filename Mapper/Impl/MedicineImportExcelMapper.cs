using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineImport;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineImportDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using static SWP391_SE1914_ManageHospital.Ultility.Status;


namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class MedicineImportExcelMapper : IMedicineImportExcelMapper
    {
        public MedicineImport CreateImportEntity(int supplierId)
        {
            return new MedicineImport
            {
                SupplierId = supplierId,
                Name = $"Import {DateTime.Now:yyyyMMdd_HHmmss}",
                Code = "IMP_" + Guid.NewGuid().ToString("N")[..8],
                Notes = "Import từ file Excel",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
        }

        public Medicine CreateToEntity(MedicineCreate create)
        {
            return new Medicine
            {
                Name = create.Name,
                Code = create.Code,
                Description = create.Description,
                Dosage = create.Dosage,
                UnitId = create.UnitId,
                MedicineCategoryId = create.MedicineCategoryId,
                Prescribed = 0,
                Status = MedicineStatus.Active,
                CreateBy = create.CreateBy,
                UpdateBy = create.UpdateBy ?? create.CreateBy,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
        }

        public MedicineResponseDTO EntityToResponse(Medicine entity)
        {
            return new MedicineResponseDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                Dosage = entity.Dosage,
                UnitId = entity.UnitId,
                MedicineCategoryId = entity.MedicineCategoryId,
                Status = entity.Status,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
                CreateBy = entity.CreateBy,
                UpdateBy = entity.UpdateBy
            };
        }

        public IEnumerable<MedicineResponseDTO> ListEntityToResponse(IEnumerable<Medicine> entities)
        {
            return entities.Select(EntityToResponse).ToList();
        }

        public MedicineImportDetail MapToImportDetail(MedicineCreate dto, int importId, int medicineId)
        {
            return new MedicineImportDetail
            {
                ImportId = importId,
                MedicineId = medicineId,
                BatchNumber = "AUTO_BATCH",
                Quantity = 100, // có thể nhận từ DTO excel nếu mở rộng
                UnitPrice = 0,
                ManufactureDate = DateTime.Now.AddMonths(-1),
                ExpiryDate = DateTime.Now.AddYears(2),
                UnitId = dto.UnitId,
                Name = dto.Name,
                Code = "IMPD_" + Guid.NewGuid().ToString("N")[..8],
                CreateBy = dto.CreateBy,
                UpdateBy = dto.UpdateBy ?? dto.CreateBy,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
        }

        public Medicine_Inventory MapToInventory(MedicineCreate dto, int medicineId, int importDetailId)
        {
            return new Medicine_Inventory
            {
                MedicineId = medicineId,
                ImportDetailId = importDetailId,
                Quantity = 100, // có thể lấy từ Excel DTO
                BatchNumber = "AUTO_BATCH",
                UnitPrice = 0,
                ExpiryDate = DateTime.Now.AddYears(2),
                ImportDate = DateTime.Now,
                Status = MedicineInventoryStatus.InStock,
            };
        }

        public MedicineDetail MapToMedicineDetail(MedicineCreate dto, int medicineId)
        {
            throw new NotImplementedException();
        }

        
    }
}
