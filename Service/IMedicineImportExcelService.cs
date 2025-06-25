using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IMedicineImportExcelService
    {
        public Task<List<MedicineResponseDTO>> ImportFromExcelAsync(List<MedicineCreate> medicines);
    }
}
