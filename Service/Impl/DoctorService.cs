using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Doctor;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Nurse;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Ultility;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

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
            return _mapper.EntityToResponse(doctor);
        }

        public async Task<IEnumerable<DoctorResponseDTO>> GetAllDoctorsAsync()
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .ToListAsync();
            return _mapper.ListEntityToResponse(doctors);
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
            return _mapper.ListEntityToResponse(doctors);
        }

        public async Task<DoctorResponseDTO> CreateDoctorAsync(DoctorCreate doctorCreateDto)
        {
            var doctor = _mapper.CreateToEntity(doctorCreateDto);
            doctor.CreateDate = DateTime.Now.AddHours(7);
            doctor.UpdateDate = DateTime.Now.AddHours(7);

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return _mapper.EntityToResponse(doctor);
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

        public async Task<DoctorResponseDTO> UpdateDoctorAsync(int userId, DoctorUpdate doctorUpdateDto)
        {
            var coID = await _context.Doctors.FirstOrDefaultAsync(p => p.UserId == userId)
                ?? throw new Exception($"Không thể tìm Bệnh nhân có user id: {userId}");



            coID.Name = doctorUpdateDto.Name;
            coID.Code = doctorUpdateDto.Code;
            coID.Gender = doctorUpdateDto.Gender;
            coID.Dob = doctorUpdateDto.Dob;
            coID.CCCD = doctorUpdateDto.CCCD;
            coID.Phone = doctorUpdateDto.Phone;
            coID.ImageURL = doctorUpdateDto.ImageURL;
            coID.LicenseNumber = doctorUpdateDto.LicenseNumber;
            coID.YearOfExperience = doctorUpdateDto.YearOfExperience;
            coID.WorkingHours = doctorUpdateDto.WorkingHours;
            coID.Status = doctorUpdateDto.Status;
            coID.DepartmentId = doctorUpdateDto.DepartmentId;
            
            coID.CreateBy = "Admin";
            coID.UpdateBy = "Admin";
            await _context.SaveChangesAsync();

            return _mapper.EntityToResponse(coID);
        }

        public async Task<bool> DeleteDoctorAsync(int id, DoctorDelete doctorDeleteDto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return false;

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DoctorRegisterResponse> DoctorRegisterAsync(DoctorRegisterRequest request)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == request.DepartmentId);
            if (department == null)
            {
                throw new Exception("DepartmentId không tồn tại.");
            }
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("Email đã tồn tại");
            }
            if (await _context.Doctors.AnyAsync(u => u.Phone == request.Phone))
            {
                throw new Exception("Số điện thoại đã tồn tại");
            }
            if (await _context.Doctors.AnyAsync(u => u.CCCD == request.CCCD))
            {
                throw new Exception("CCCD đã tồn tại");
            }
            request.FullName = request.FullName.Trim();
            if (string.IsNullOrEmpty(request.FullName))
                throw new Exception("Không được để trống tên");

            if (!Regex.IsMatch(request.FullName, @"^[a-zA-ZÀ-ỹ\s]+$"))
                throw new Exception("Tên không được chứa kí tự đặc biệt");
            var hashedPassword = _passwordHasher.HashPassword(request.Password);
            var newUser = new User
            {
                Email = request.Email,
                Password = hashedPassword,
                Status = UserStatus.Active
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            var doctor = new Doctor
            {
                Code = request.Code,
                Name = request.FullName,
                Gender = request.Gender,
                Dob = request.Dob,
                CCCD = request.CCCD,
                Phone = request.Phone,
                ImageURL = "",
                Status = DoctorStatus.Available,
                LicenseNumber = request.LicenseNumber,
                CreateDate = DateTime.Now.AddHours(7),
                UpdateDate = DateTime.Now.AddHours(7),
                CreateBy = request.FullName,
                UpdateBy = request.FullName,
                DepartmentId = request.DepartmentId,
                UserId = newUser.Id,
            };
            if (!string.IsNullOrEmpty(request.Code) && request.Code != "string")
            {
                doctor.Code = request.Code;
            }
            else
            {
                doctor.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Doctors.AnyAsync(n => n.Code == doctor.Code))
            {
                doctor.Code = await CheckUniqueCodeAsync();
            }

            await _context.Doctors.AddAsync(doctor);

            var doctorRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name.Trim().ToLower() == "doctor");
            if (doctorRole != null)
            {
                await _context.User_Roles.AddAsync(new User_Role
                {
                    UserId = newUser.Id,
                    RoleId = doctorRole.Id
                });
            }
            await _context.SaveChangesAsync();
            return new DoctorRegisterResponse
            {
                NurseId = doctor.Id,
                FullName = doctor.Name,
                Email = newUser.Email,
            };
        }

        public async Task<int?> GetDepartmentIdByDoctorIdAsync(int doctorId)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            return doctor?.DepartmentId;
        }


            return _mapper.EntityToResponse(doctor);
        }

        public async Task<IEnumerable<DoctorResponseDTO>> GetDoctorsByDepartmentAsync(int departmentId)
        {
            var doctors = await _context.Doctors
                .Where(d => d.DepartmentId == departmentId)
                .ToListAsync();

            return _mapper.ListEntityToResponse(doctors);

        public async Task<DoctorResponseDTO> GetDoctorByUserIdAsync(int userId)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.UserId == userId);
            return _mapper.MapToDto(doctor);

        }
    }
}