using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Nurse;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Mapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service
{
    public class NurseService : INurseService
    {
        private readonly ApplicationDBContext _context;
        private readonly INurseMapper _mapper;

        public NurseService(ApplicationDBContext context, INurseMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<NurseResponseDTO> GetNurseByIdAsync(int id)
        {
            var nurse = await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Department)
                .FirstOrDefaultAsync(n => n.Id == id);
            return _mapper.MapToDto(nurse);
        }

        public async Task<IEnumerable<NurseResponseDTO>> GetAllNursesAsync()
        {
            var nurses = await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Department)
                .ToListAsync();
            return _mapper.MapToDtoList(nurses);
        }

        public async Task<NurseResponseDTO> CreateNurseAsync(NurseCreate nurseCreateDto)
        {
            var nurse = _mapper.MapToEntity(nurseCreateDto);
            nurse.CreateDate = DateTime.Now;
            nurse.UpdateDate = DateTime.Now;

            _context.Nurses.Add(nurse);
            await _context.SaveChangesAsync();

            return _mapper.MapToDto(nurse);
        }

        public async Task<NurseResponseDTO> UpdateNurseAsync(int id, NurseUpdate nurseUpdateDto)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return null;

            _mapper.MapToEntity(nurseUpdateDto, nurse);
            nurse.UpdateDate = DateTime.Now;

            _context.Nurses.Update(nurse);
            await _context.SaveChangesAsync();

            return _mapper.MapToDto(nurse);
        }

        public async Task<bool> DeleteNurseAsync(int id, NurseDelete nurseDeleteDto)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return false;

            _context.Nurses.Remove(nurse);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}