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

    public async Task<bool> ChangePasswordAsync(int userId, UserType userType, string oldPassword, string newPassword)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !_passwordHasher.VerifyPassword(oldPassword, user.Password))
            {
                throw new Exception("Mật khẩu cũ không đúng!");
            }
            user.Password = _passwordHasher.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }catch (Exception ex)
        {
            return false;
        }  
    }

    public async Task<bool> ForgotPasswordAsync(string email, UserType userType)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Patients)
                .Include(u => u.Doctors)
                .Include(u => u.Nurses)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) throw new Exception("Email không tồn tại!");

            var roles = await _context.User_Roles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name.Trim().ToLower())
                .ToListAsync();

            if (!roles.Contains(userType.ToString().ToLower()))
                throw new Exception("Email không đúng!");

            user.ResetPasswordToken = Guid.NewGuid().ToString();
            user.ResetPasswordTokenExpiryTime = DateTime.Now.AddHours(1); // Token 1h

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu cần
            throw new Exception("Quên mật khẩu không thành công", ex);
        }
    }

    public Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            throw new Exception("Đăng nhập không thành công");
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu cần
            throw new Exception("Đăng nhập không thành công", ex);
        }
    }

    public async Task<bool> LogoutAsync(int userId, UserType userType)
    {
        try
        {
            if (userType == UserType.Patient)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.RefreshToken = null; // Xóa RefreshToken
                    user.RefreshTokenExpiryTime = null; // Xóa thời gian hết hạn RefreshToken
                }
                else
                {
                    return false; // Người dùng không tồn tại
                }
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu cần
            throw new Exception("Đăng xuất không thành công", ex);
        }
    }

    public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
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

    public async Task<bool> ResetPasswordAsync(string token, string newPassword, UserType userType)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Patients)
                .Include(u => u.Doctors)
                .Include(u => u.Nurses)
                .FirstOrDefaultAsync(u => u.ResetPasswordToken == token && u.ResetPasswordTokenExpiryTime > DateTime.Now.AddHours(7));
            if (user == null) throw new Exception("Token không hợp lệ hoặc đã hết hạn!");
            var hasshedPassword = _passwordHasher.HashPassword(newPassword);

            user.Password = hasshedPassword;
            user.ResetPasswordToken = null; // Xóa token sau khi reset
            user.ResetPasswordTokenExpiryTime = null; // Xóa thời gian hết hạn token
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu cần
            throw new Exception("Đặt lại mật khẩu không thành công", ex);
        }
    }
}
