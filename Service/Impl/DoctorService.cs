using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;

namespace SWP391_SE1914_ManageHospital.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDBContext _context;
        private readonly IDoctorMapper _mapper;

        public DoctorService(ApplicationDBContext context, IDoctorMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.User)
                .ToListAsync();
            return doctors.Select(_mapper.MapToDTO);
        }

        public async Task<DoctorDTO> GetDoctorByIdAsync(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);
            return doctor != null ? _mapper.MapToDTO(doctor) : null;
        }

        public async Task<DoctorDTO> CreateDoctorAsync(DoctorDTO doctorDTO)
        {
            var doctor = _mapper.MapToEntity(doctorDTO);
            doctor.CreateDate = DateTime.Now;
            doctor.UpdateDate = DateTime.Now;
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return _mapper.MapToDTO(doctor);
        }

        public async Task<bool> UpdateDoctorAsync(int id, DoctorDTO doctorDTO)
        {
            if (id != doctorDTO.Id)
            {
                return false;
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return false;
            }

            var updatedDoctor = _mapper.MapToEntity(doctorDTO);
            updatedDoctor.UpdateDate = DateTime.Now;
            _context.Entry(doctor).CurrentValues.SetValues(updatedDoctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return false;
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}