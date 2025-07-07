using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

public class MedicineService : IMedicineService
{
    private readonly ApplicationDBContext _context;
    private readonly IMedicineMapper _mapper;

    public MedicineService(ApplicationDBContext context, IMedicineMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MedicineResponseDTO>> GetAllAsync()
    {
        var medicines = await _context.Medicines
            .Include(m => m.MedicineCategory)  // Bao gồm bảng MedicineCategory để lấy tên danh mục
            .Include(m => m.Unit)              // Bao gồm bảng Unit để lấy tên đơn vị
            .AsNoTracking()                   // Đảm bảo không theo dõi thay đổi (tăng hiệu suất)
            .ToListAsync();                   // Lấy dữ liệu từ cơ sở dữ liệu

        return medicines.Select(m => new MedicineResponseDTO
        {
            Id = m.Id,
            Name = m.Name,
            Code = m.Code,
            Dosage = m.Dosage,
            ImageUrl = m.ImageUrl,
            UnitId = m.UnitId,
            MedicineCategoryName = m.MedicineCategory.Name, // Truy xuất tên từ bảng MedicineCategory
            UnitName = m.Unit.Name,                         // Truy xuất tên từ bảng Unit
            Status = (MedicineStatus)m.Status,              // Chuyển từ int sang enum MedicineStatus
            UnitPrice = m.UnitPrice,                        // Giá thuốc
            MedicineCategoryId = m.MedicineCategoryId,     // ID của danh mục thuốc
            Prescribed = m.Prescribed.ToString(),          // Trạng thái prescribed
            CreateDate = m.CreateDate,                      // Ngày tạo
            UpdateDate = m.UpdateDate,                      // Ngày cập nhật
            CreateBy = m.CreateBy,                          // Người tạo
            UpdateBy = m.UpdateBy,                          // Người cập nhật
            Description = m.Description                     // Mô tả
        }).ToList(); // Ánh xạ từ entity sang DTO
    }


    public async Task<MedicineResponseDTO?> GetByIdAsync(int id)
    {
        var entity = await _context.Medicines.FindAsync(id);
        return entity == null ? null : _mapper.MapToDTO(entity);
    }

    public async Task<MedicineResponseDTO> CreateAsync(MedicineRequest request)
    {
        var entity = _mapper.MapToEntity(request);
        _context.Medicines.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.MapToDTO(entity);
    }

    public async Task<MedicineResponseDTO?> UpdateAsync(int id, MedicineRequest request)
    {
        var entity = await _context.Medicines.FindAsync(id);
        if (entity == null) return null;

        _mapper.MapToExistingEntity(request, entity);
        await _context.SaveChangesAsync();
        return _mapper.MapToDTO(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Medicines.FindAsync(id);
        if (entity == null) return false;

        _context.Medicines.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<MedicineResponseDTO>> GetByPrescribedAsync(int prescribed)
    {
        var list = await _context.Medicines
            .Where(m => (int)m.Prescribed == prescribed)
            .AsNoTracking()
            .ToListAsync();

        return list.Select(_mapper.MapToDTO).ToList();
    }

    public async Task<IEnumerable<MedicineResponseDTO>> GetByCategoryIdAsync(int categoryId)
    {
        var medicines = await _context.Medicines
            .Include(m => m.MedicineCategory) // Bao gồm bảng MedicineCategory
            .Include(m => m.Unit) // Bao gồm bảng Unit 
            
            .Where(m => m.MedicineCategoryId == categoryId)
            .Select(m => new MedicineResponseDTO
            {
                Id = m.Id,
                Name = m.Name, // Lấy trực tiếp từ bảng Medicines
                Code = m.Code,
                ImageUrl = m.ImageUrl,
                UnitPrice = m.UnitPrice,
                Description =m.Description,
                MedicineCategoryName = m.MedicineCategory.Name, // Lấy tên từ bảng MedicineCategory
                UnitName = m.Unit.Name, // Lấy tên từ bảng Unit
                Status = (MedicineStatus)m.Status,  // Ánh xạ Status từ int sang enum MedicineStatus
            })
            .ToListAsync();

        return medicines;
    }




}
