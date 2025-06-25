using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supplier;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDBContext _context;
        private readonly ISupplierMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupplierService(ApplicationDBContext context, ISupplierMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        
        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value ?? "unknown";
        }


        public async Task<IEnumerable<SupplierResponeDTO>> GetAllSupplierAsync()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            return _mapper.ListEntityToResponse(suppliers);
        }

        public async Task<IEnumerable<SupplierResponeDTO>> SearchSupplierByKeyAsync(string name)
        {
            var sid = await _context.Suppliers.FromSqlRaw("Select * from Suppliers where Name like {0}", "%" + name + "%").ToListAsync();
            if (sid == null)
            {
                throw new Exception($"Không có tên {name} nào tồn tại!");
            }
            var response = _mapper.ListEntityToResponse(sid);
            return response;
        }

        public async Task<SupplierResponeDTO> UpdateSupplerAsync(int id, SupplierUpdate update)
        {
            var existing = await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null)
            {
                throw new Exception("Không tìm thấy nhà cung cấp!");
            }

            existing.Name = update.Name;
            existing.Phone = update.Phone;
            existing.Email = update.Email;
            existing.Address = update.Address;
            existing.UpdateDate = DateTime.Now;
            existing.UpdateBy = GetCurrentUserId();

            _context.Suppliers.Update(existing);
            await _context.SaveChangesAsync();

            return _mapper.EntityToResponse(existing);
        }

        public async Task<SupplierResponeDTO> CreateSupplerAsync(SupplierCreate create)
        {
            if (await _context.Suppliers.AnyAsync(x => x.Name.Trim().ToLower() == create.Name.Trim().ToLower() && x.Phone == create.Phone && x.Email.Trim().ToLower() == create.Email.Trim().ToLower()))
            {
                throw new Exception("Nhà cung cấp đã tồn tại !");
            }
            var supplier = _mapper.CreateToEntity(create);
            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                supplier.Code = create.Code;
            }
            else
            {
                supplier.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Clinics.AnyAsync(p => p.Code == supplier.Code))
            {
                supplier.Code = await CheckUniqueCodeAsync();
            }
            supplier.CreateDate = DateTime.Now;
            supplier.CreateBy = GetCurrentUserId();
            await _context.Suppliers.AddAsync(supplier);

            await _context.SaveChangesAsync();
            return _mapper.EntityToResponse(supplier);
        }

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GenerateClinicCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Suppliers.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }
    }
}
