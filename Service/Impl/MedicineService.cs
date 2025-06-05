using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;

public class MedicineService : IMedicineService
{
    private readonly ApplicationDBContext _context;

    public MedicineService(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<MedicineResponseDTO>> GetAllAsync()
    {
        return await _context.Medicines
            .Select(m => new MedicineResponseDTO
            {
                Id = m.Id,
                Name = m.Name,
                Code = m.Code,
                Description = m.Description,
                Dosage = m.Dosage,
                Unit = m.Unit,
                Status = m.Status ,
                MedicineCategoryId = m.MedicineCategoryId,
                CreateDate = m.CreateDate,
                UpdateDate = m.UpdateDate,
                CreateBy = m.CreateBy,
                UpdateBy = m.UpdateBy
            })
            .ToListAsync();
    }

    public async Task<MedicineResponseDTO?> GetByIdAsync(int id)
    {
        var m = await _context.Medicines.FindAsync(id);
        if (m == null) return null;

        return new MedicineResponseDTO
        {
            Id = m.Id,
            Name = m.Name,
            Code = m.Code,
            Description = m.Description,
            Dosage = m.Dosage,
            Unit = m.Unit,
            Status = m.Status,
            MedicineCategoryId = m.MedicineCategoryId,
            CreateDate = m.CreateDate,
            UpdateDate = m.UpdateDate,
            CreateBy = m.CreateBy,
            UpdateBy = m.UpdateBy
        };
    }

    public async Task<MedicineResponseDTO> CreateAsync(MedicineCreate request)
    {
        var m = new Medicine
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            Dosage = request.Dosage,
            Unit = request.Unit,
            MedicineCategoryId = request.MedicineCategoryId,
            Status = request.Status,
            CreateDate = request.CreateDate,
            UpdateDate = request.UpdateDate,
            CreateBy = request.CreateBy,
            UpdateBy = request.UpdateBy
        };

        _context.Medicines.Add(m);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(m.Id) ?? throw new Exception("Tạo thất bại");
    }

    public async Task<bool> UpdateAsync(int id, MedicineCreate request)
    {
        var m = await _context.Medicines.FindAsync(id);
        if (m == null) return false;

        m.Name = request.Name;
        m.Code = request.Code;
        m.Description = request.Description;
        m.Dosage = request.Dosage;
        m.Unit = request.Unit;
        m.MedicineCategoryId = request.MedicineCategoryId;
        m.Status = request.Status;
        m.UpdateDate = request.UpdateDate;
        m.UpdateBy = request.UpdateBy;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var m = await _context.Medicines.FindAsync(id);
        if (m == null) return false;

        _context.Medicines.Remove(m);
        await _context.SaveChangesAsync();
        return true;
    }
}
