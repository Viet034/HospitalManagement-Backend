using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineImport;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface IMedicineImportExcelMapper
    {
        Medicine CreateToEntity (MedicineCreate create);
        MedicineResponseDTO EntityToResponse(Medicine entity);
        MedicineImport CreateImportEntity(int supplierId);
        MedicineDetail MapToMedicineDetail(MedicineCreate dto, int medicineId);
        MedicineImportDetail MapToImportDetail(MedicineCreate dto, int importId, int medicineId);
        Medicine_Inventory MapToInventory(MedicineCreate dto, int medicineId, int importDetailId);
        IEnumerable<MedicineResponseDTO> ListEntityToResponse(IEnumerable<Medicine> entities);
    }
}
