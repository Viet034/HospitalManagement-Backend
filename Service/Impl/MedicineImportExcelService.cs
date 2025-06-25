using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class MedicineImportExcelService : IMedicineImportExcelService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMedicineImportExcelMapper _mapper;

        public MedicineImportExcelService(ApplicationDBContext context, IMedicineImportExcelMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<MedicineResponseDTO>> ImportFromExcelAsync(List<MedicineCreate> medicines)
        {
            
        }
    }
}
