using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Patient;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class PatientRegistrationService : IPatientRegistrationService
{
    private readonly ApplicationDBContext _context;

    public PatientRegistrationService(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<PatientProfileDTO> RegisterNewPatientAsync(PatientRegistrationDTO request)
    {
        // Validate unique email and contact
        if (await IsEmailRegisteredAsync(request.Email))
            throw new Exception("Email is already registered");

        if (await IsContactRegisteredAsync(request.Contact))
            throw new Exception("Contact number is already registered");

        // Create new user account
        var user = new User
        {
            Email = request.Email,
            Status = UserStatus.Active
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Create user profile
        var userProfile = new UserProfile
        {
            Phone = request.Contact,
            UserId = user.Id,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            CreateBy = "System",
            UpdateBy = "System"
        };

        await _context.UserProfiles.AddAsync(userProfile);
        await _context.SaveChangesAsync();

        // Create patient profile
        var patient = await CreatePatientProfileAsync(request, user.Id.ToString());

        return MapToPatientProfileDTO(patient, user, userProfile, true);
    }

    public async Task<PatientProfileDTO> RegisterExistingUserAsPatientAsync(PatientRegistrationDTO request, string userId)
    {
        // Check if user already has a patient profile
        if (await HasPatientProfileAsync(userId))
            throw new Exception("User already has a patient profile");

        var user = await _context.Users
            .Include(u => u.UserProfile)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId)
            ?? throw new Exception("User not found");

        // Update user profile
        if (user.UserProfile == null)
        {
            user.UserProfile = new UserProfile
            {
                Phone = request.Contact,
                UserId = user.Id,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId
            };
            await _context.UserProfiles.AddAsync(user.UserProfile);
        }
        else
        {
            user.UserProfile.Phone = request.Contact;
            user.UserProfile.UpdateDate = DateTime.Now;
            user.UserProfile.UpdateBy = userId;
        }

        user.Email = request.Email;
        await _context.SaveChangesAsync();

        // Create patient profile
        var patient = await CreatePatientProfileAsync(request, userId);

        return MapToPatientProfileDTO(patient, user, user.UserProfile, true);
    }

    public async Task<bool> IsEmailRegisteredAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> IsContactRegisteredAsync(string contact)
    {
        return await _context.UserProfiles.AnyAsync(u => u.Phone == contact);
    }

    public async Task<PatientProfileDTO?> GetPatientProfileByUserIdAsync(string userId)
    {
        var patient = await _context.Patients
            .Include(p => p.User)
            .ThenInclude(u => u.UserProfile)
            .FirstOrDefaultAsync(p => p.User.Id.ToString() == userId);

        if (patient == null || patient.User.UserProfile == null)
            return null;

        return MapToPatientProfileDTO(patient, patient.User, patient.User.UserProfile, true);
    }

    public async Task<bool> HasPatientProfileAsync(string userId)
    {
        return await _context.Patients.AnyAsync(p => p.User.Id.ToString() == userId);
    }

    private async Task<Patient> CreatePatientProfileAsync(PatientRegistrationDTO request, string userId)
    {
        string patientCode = await GeneratePatientCodeAsync();

        var patient = new Patient
        {
            Name = $"{request.FirstName} {request.LastName}",
            Code = patientCode,
            Gender = request.Gender,
            Status = PatientStatus.Active,
            UserId = int.Parse(userId),
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            CreateBy = "System",
            UpdateBy = "System"
        };

        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        return patient;
    }

    private async Task<string> GeneratePatientCodeAsync()
    {
        string code;
        do
        {
            code = $"PT{DateTime.Now:yyyyMMddHHmmss}";
        } while (await _context.Patients.AnyAsync(p => p.Code == code));

        return code;
    }

    private PatientProfileDTO MapToPatientProfileDTO(Patient patient, User user, UserProfile userProfile, bool hasAccount)
    {
        var names = patient.Name.Split(' ', 2);
        return new PatientProfileDTO
        {
            Id = patient.Id,
            FirstName = names[0],
            LastName = names.Length > 1 ? names[1] : "",
            FullName = patient.Name,
            Code = patient.Code,
            Email = user.Email,
            Contact = userProfile.Phone,
            Gender = patient.Gender,
            Status = patient.Status,
            CreateDate = patient.CreateDate,
            HasAccount = hasAccount
        };
    }
} 