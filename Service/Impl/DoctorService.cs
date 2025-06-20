using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Doctor;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Ultility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDBContext _context;
        private readonly IDoctorMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public DoctorService(ApplicationDBContext context, IDoctorMapper mapper, IPasswordHasher passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<DoctorResponseDTO> GetDoctorByIdAsync(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Id == id);
            return _mapper.MapToDto(doctor);
        }

        public async Task<IEnumerable<DoctorResponseDTO>> GetAllDoctorsAsync()
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .ToListAsync();
            return _mapper.MapToDtoList(doctors);
        }

        public async Task<IEnumerable<DoctorResponseDTO>> GetDoctorByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<DoctorResponseDTO>();
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .Where(d => EF.Functions.Like(d.Name, $"%{name}%"))
                .ToListAsync();
            return _mapper.MapToDtoList(doctors);
        }

        public async Task<DoctorResponseDTO> CreateDoctorAsync(DoctorCreate doctorCreateDto)
        {
            var doctor = _mapper.MapToEntity(doctorCreateDto);
            doctor.CreateDate = DateTime.Now.AddHours(7);
            doctor.UpdateDate = DateTime.Now.AddHours(7);

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return _mapper.MapToDto(doctor);
        }

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GeneratePatientCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Doctors.AnyAsync(d => d.Code == newCode);
            } while (isExist);

            return newCode;
        }

        public async Task<DoctorResponseDTO> UpdateDoctorAsync(int id, DoctorUpdate doctorUpdateDto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return null;

            _mapper.MapToEntity(doctorUpdateDto, doctor);
            doctor.UpdateDate = DateTime.Now.AddHours(7);

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            return _mapper.MapToDto(doctor);
        }

        public async Task<bool> DeleteDoctorAsync(int id, DoctorDelete doctorDeleteDto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return false;

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}