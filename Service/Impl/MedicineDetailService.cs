using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineDetail;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Services
{
    public class MedicineDetailService : IMedicineDetailService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMedicineDetailMapper _mapper;

        public MedicineDetailService(ApplicationDBContext context, IMedicineDetailMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MedicineDetailResponseDTO>> GetAllAsync()
        {
            var list = await _context.MedicineDetails.AsNoTracking().ToListAsync();
            return list.Select(_mapper.MapToDTO).ToList();
        }

        public async Task<MedicineDetailResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _context.MedicineDetails.FindAsync(id);
            return entity == null ? null : _mapper.MapToDTO(entity);
        }

        public async Task<MedicineDetailResponseDTO> CreateAsync(MedicineDetailRequest request)
        {
            var entity = _mapper.MapToEntity(request);
            _context.MedicineDetails.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.MapToDTO(entity);
        }

        public async Task<MedicineDetailResponseDTO?> UpdateAsync(int id, MedicineDetailRequest request)
        {
            var entity = await _context.MedicineDetails.FindAsync(id);
            if (entity == null) return null;

            _mapper.MapToExistingEntity(request, entity);
            await _context.SaveChangesAsync();
            return _mapper.MapToDTO(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.MedicineDetails.FindAsync(id);
            if (entity == null) return false;

            _context.MedicineDetails.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
