using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineImport;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineImportDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IMedicineImportDetailService
    {
        public Task<IEnumerable<MedicineImportDetailResponseDTO>> GetAllMedicineImportDetailAsync();
        public Task<IEnumerable<MedicineImportDetailResponseDTO>> SearchMedicineImportDetailAsync(string batchnumber);
        public Task<MedicineImportDetailResponseDTO> CreateMedicineImportDetail(MedicineImportDetailCreate create);
        public Task<MedicineImportDetailResponseDTO> UpdateMedicineImportDetail(MedicineImportDetailUpdate update, int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}
