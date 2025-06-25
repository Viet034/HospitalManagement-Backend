using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineImport;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineImportDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class MedicineImportDetailService : IMedicineImportDetailService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMedicineImportDetailMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MedicineImportDetailService(ApplicationDBContext context, IMedicineImportDetailMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value ?? "unknown";
        }

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GenerateClinicCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.MedicineImportDetails.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<MedicineImportDetailResponseDTO> CreateMedicineImportDetail(MedicineImportDetailCreate create)
        {
            if (!string.IsNullOrEmpty(create.BatchNumber) && await _context.MedicineImportDetails.AnyAsync(m =>m.BatchNumber != null &&m.BatchNumber.ToLower() == create.BatchNumber.ToLower()))
            {
                throw new Exception("Batch Number used!");
            }
            var MID = _mapper.CreateToEntity(create);
            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                MID.Code = create.Code;
            }
            else
            {
                MID.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Clinics.AnyAsync(p => p.Code == MID.Code))
            {
                MID.Code = await CheckUniqueCodeAsync();
            }

            MID.CreateDate = DateTime.Now;
            MID.CreateBy = GetCurrentUserId();
            await _context.MedicineImportDetails.AddAsync(MID);
            await _context.SaveChangesAsync();
            return _mapper.EntityToResponse(MID);
        }

        public async Task<IEnumerable<MedicineImportDetailResponseDTO>> GetAllMedicineImportDetailAsync()
        {
            
            var MID = await _context.MedicineImportDetails.ToListAsync();
            return _mapper.ListEntityToResponse(MID);
        }

        public async Task<IEnumerable<MedicineImportDetailResponseDTO>> SearchMedicineImportDetailAsync(string batchnumber)
        {
            var batch = await _context.MedicineImportDetails.FromSqlRaw("Select * from medicine_import_details where BatchNumber like {0}", "%" + batchnumber + "%").ToListAsync();
            if(batchnumber == null)
            {
                throw new Exception($"Không tìm thấy lô {batchnumber}");
            }
            var res = _mapper.ListEntityToResponse(batch);
            return res;
        }

        public async Task<MedicineImportDetailResponseDTO> UpdateMedicineImportDetail(MedicineImportDetailUpdate update, int id)
        {
            var up = await _context.MedicineImportDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (up == null)
            {
                throw new Exception("Không tìm thấy!");
            }
            up.BatchNumber = update.BatchNumber;
            up.Quantity = update.Quantity;
            up.UnitPrice = update.UnitPrice;
            up.UnitId = update.UnitId;

            up.UpdateDate = DateTime.Now;
            up.UpdateBy = GetCurrentUserId();

            _context.MedicineImportDetails.Update(up);
            await _context.SaveChangesAsync();
            return _mapper.EntityToResponse(up);

        }
    }
}
