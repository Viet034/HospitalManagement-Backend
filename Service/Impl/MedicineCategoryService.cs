using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data; // assume ApplicationDbContext here
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Service
{
    public class MedicineCategoryService : IMedicineCategoryService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMedicineCategoryMapper _mapper;

        public MedicineCategoryService(
            ApplicationDBContext context,
            IMedicineCategoryMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MedicineCategoryResponseDTO>> GetAllAsync()
        {
            var list = await _context.MedicineCategories
                                     .AsNoTracking()
                                     .ToListAsync();
            return list.Select(_mapper.MapToDTO).ToList();
        }

        public async Task<MedicineCategoryResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _context.MedicineCategories
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Id == id);
            return entity is null ? null : _mapper.MapToDTO(entity);
        }

        public async Task<MedicineCategoryResponseDTO> CreateAsync(MedicineCategoryCreate request)
        {
            var entity = new MedicineCategory
            {
                Name = request.Name,
                
                Description = request.Description,
                ImageURL = request.ImageURL,
                Status = request.Status,
                CreateDate = DateTime.Now,
                CreateBy = request.CreateBy
            };
            _context.MedicineCategories.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.MapToDTO(entity);
        }

        public async Task<bool> UpdateAsync(int id, MedicineCategoryCreate request)
        {
            var entity = await _context.MedicineCategories.FindAsync(id);
            if (entity is null) return false;

            entity.Name = request.Name;
            
            entity.Description = request.Description;
            entity.ImageURL = request.ImageURL;
            entity.Status = request.Status;
            entity.UpdateDate = DateTime.Now;
            entity.UpdateBy = request.CreateBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.MedicineCategories.FindAsync(id);
            if (entity is null) return false;

            _context.MedicineCategories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
