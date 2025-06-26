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
        var list = await _context.Medicines.AsNoTracking().ToListAsync();
        return list.Select(_mapper.MapToDTO).ToList();
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
            .Where(m => m.MedicineCategoryId == categoryId)
            .Select(m => new MedicineResponseDTO
            {
                Id = m.Id,
                Name = m.Name,
                Code = m.Code,
                MedicineCategoryName = m.MedicineCategory.Name,
                UnitName = m.Unit.Name,
                Status = MedicineStatus.Active,
                // thêm các trường khác nếu cần
            })
            .ToListAsync();

        return medicines;
    }


}
