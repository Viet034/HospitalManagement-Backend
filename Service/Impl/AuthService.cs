using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Authenication;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using System.Text.RegularExpressions;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class AuthService : IAuthService
{
    private readonly ApplicationDBContext _context;
    private readonly IPasswordHasher _passwordHasher;
    public AuthService(ApplicationDBContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public Task<bool> ChangePasswordAsync(int userId, UserType userType, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ForgotPasswordAsync(string email, UserType userType)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> LogoutAsync(int userId, UserType userType)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponse> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public async Task<PatientRegisterResponse> RegisterPatientAsync(PatientRegisterRequest request)
    {
        // 1. Kiểm tra Email tồn tại chưa
        if (await _context.Users.AnyAsync(x => x.Email == request.Email))
            throw new Exception("Email đã tồn tại!");

        // 2. Kiểm tra Phone và CCCD
        if (await _context.Patients.AnyAsync(x => x.Phone == request.Phone))
            throw new Exception("Số điện thoại đã tồn tại");

        if (await _context.Patients.AnyAsync(x => x.CCCD == request.CCCD))
            throw new Exception("CCCD đã tồn tại");

        // 3. Kiểm tra và chuẩn hóa tên
        request.FullName = request.FullName.Trim();
        if (string.IsNullOrEmpty(request.FullName))
            throw new Exception("Không được để trống tên");

        if (!Regex.IsMatch(request.FullName, @"^[a-zA-ZÀ-ỹ\s]+$"))
            throw new Exception("Tên không được chứa kí tự đặc biệt");

        // 4. Tạo User mới
        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        var newUser = new User
        {
            Email = request.Email,
            Password = hashedPassword,
            Status = UserStatus.Active
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync(); // Phải save để lấy được newUser.Id

        // 5. Gán thông tin Patient
        var patient = new Patient
        {
            Code = request.Code,
            Name = request.FullName,
            Phone = request.Phone,
            CCCD = request.CCCD,
            Address = request.Address,
            Gender = request.Gender,
            Dob = request.Dob,
            EmergencyContact = request.EmergencyContact,
            InsuranceNumber = "",
            Allergies = "",
            BloodType = "",
            Status = PatientStatus.Active,
            CreateDate = DateTime.Now.AddHours(7),
            UpdateDate = DateTime.Now.AddHours(7),
            CreateBy = request.FullName,
            UpdateBy = request.FullName,
            ImageURL = "",
            UserId = newUser.Id
        };

        await _context.Patients.AddAsync(patient);

        // 6. Gán role PATIENT (nếu dùng User_Role)
        var patientRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name.Trim().ToLower() == "bệnh nhân");
        if (patientRole != null)
        {
            await _context.User_Roles.AddAsync(new User_Role
            {
                UserId = newUser.Id,
                RoleId = patientRole.Id
            });
        }

        await _context.SaveChangesAsync();

        // 7. Trả về response
        return new PatientRegisterResponse
        {
            PatientId = patient.Id,
            FullName = patient.Name,
            Email = newUser.Email
        };
    }   

    public Task<bool> ResetPasswordAsync(string token, string newPassword, UserType userType)
    {
        throw new NotImplementedException();
    }
}
