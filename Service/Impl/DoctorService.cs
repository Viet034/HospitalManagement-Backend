using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_SE1914_ManageHospital.Services.Impl
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

        public async Task<IEnumerable<DoctorResponseDTO>> GetAllDoctorsAsync()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.User)
                .ToListAsync();
            return doctors.Select(_mapper.MapToDTO);
        }

        public async Task<DoctorResponseDTO> GetDoctorByIdAsync(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);
            return doctor != null ? _mapper.MapToDTO(doctor) : null;
        }

        public async Task<DoctorResponseDTO> CreateDoctorAsync(DoctorCreate doctorDTO)
        {
            var doctor = _mapper.MapToEntity(doctorDTO);
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return _mapper.MapToDTO(doctor);
        }

        public async Task<bool> UpdateDoctorAsync(int id, DoctorUpdate doctorDTO)
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
            updatedDoctor.CreateDate = doctor.CreateDate; 
            _context.Entry(doctor).CurrentValues.SetValues(updatedDoctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDoctorAsync(DoctorDelete doctorDTO)
        {
            var doctor = await _context.Doctors.FindAsync(doctorDTO.Id);
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