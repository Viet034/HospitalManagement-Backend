using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Service;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using System.Text.RegularExpressions;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class ServiceService : IServiceService
{
    private readonly ApplicationDBContext _context;

    public ServiceService(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceResponseDTO>> GetAllServicesAsync()
    {
        var services = await _context.Set<Servicess>()
            .Include(s => s.Department)
            .Select(s => new ServiceResponseDTO
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                Price = s.Price,
                Status = s.Status,
                DepartmentId = s.DepartmentId,
                DepartmentName = s.Department != null ? s.Department.Name : null,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                CreateBy = s.CreateBy,
                UpdateBy = s.UpdateBy
            })
            .ToListAsync();

        return services;
    }

    public async Task<ServiceResponseDTO?> GetServiceByIdAsync(int id)
    {
        var service = await _context.Set<Servicess>()
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (service == null)
            return null;

        return new ServiceResponseDTO
        {
            Id = service.Id,
            Name = service.Name,
            Code = service.Code,
            Description = service.Description,
            ImageUrl = service.ImageUrl,
            Price = service.Price,
            Status = service.Status,
            DepartmentId = service.DepartmentId,
            DepartmentName = service.Department != null ? service.Department.Name : null,
            CreateDate = service.CreateDate,
            UpdateDate = service.UpdateDate,
            CreateBy = service.CreateBy,
            UpdateBy = service.UpdateBy
        };
    }

    public async Task<ServiceResponseDTO> CreateServiceAsync(ServiceRequestDTO request)
    {

        if (await _context.Services.AnyAsync(x => x.Name == request.Name))
        {
            throw new Exception("Tên dịch vụ đã được sử dụng");
        }

        
        if (await _context.Services.AnyAsync(x => x.Code == request.Code))
        {
            throw new Exception("Mã dịch vụ đã được sử dụng");
        }


        string specialCharPattern = @"^[\p{L}0-9\s\-.,]+$";


        if (!Regex.IsMatch(request.Name, specialCharPattern))
        {
            throw new Exception("Tên dịch vụ chứa ký tự không hợp lệ.");
        }

        if (!Regex.IsMatch(request.Code, specialCharPattern))
        {
            throw new Exception("Mã dịch vụ chứa ký tự không hợp lệ.");
        }
        if (!Regex.IsMatch(request.Code, @"^S\d{5}$"))
        {
            throw new Exception("Mã dịch vụ không hợp lệ. Định dạng hợp lệ: S00001");
        }
        if (!string.IsNullOrEmpty(request.Description) &&
            !Regex.IsMatch(request.Description, specialCharPattern))
        {
            throw new Exception("Mô tả dịch vụ chứa ký tự không hợp lệ.");
        }

       
        if (request.Price <= 0)
        {
            throw new Exception("Giá dịch vụ phải lớn hơn 0.");
        }
        var service = new Servicess
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Price = request.Price,
            Status = request.Status,
            DepartmentId = request.DepartmentId,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            CreateBy = "System", 
            UpdateBy = "System"  
        };

        _context.Set<Servicess>().Add(service);
        await _context.SaveChangesAsync();

        return await GetServiceByIdAsync(service.Id) ?? new ServiceResponseDTO();
    }

    public async Task<ServiceResponseDTO?> UpdateServiceAsync(int id, ServiceRequestDTO request)
    {
        var service = await _context.Set<Servicess>().FindAsync(id);
        if (service == null)
            return null;

        
        if (await _context.Services.AnyAsync(x => x.Id != id && x.Name == request.Name))
        {
            throw new Exception("Tên dịch vụ đã được sử dụng");
        }

        
        if (await _context.Services.AnyAsync(x => x.Id != id && x.Code == request.Code))
        {
            throw new Exception("Mã dịch vụ đã được sử dụng");
        }

        
        if (!Regex.IsMatch(request.Code, @"^S\d{5}$"))
        {
            throw new Exception("Mã dịch vụ không hợp lệ. Định dạng hợp lệ: S00001");
        }


        string specialCharPattern = @"^[\p{L}0-9\s\-.,]+$";


        if (!Regex.IsMatch(request.Name, specialCharPattern))
        {
            throw new Exception("Tên dịch vụ chứa ký tự không hợp lệ.");
        }
        if (!Regex.IsMatch(request.Code, @"^S\d{5}$"))
        {
            throw new Exception("Mã dịch vụ không hợp lệ. Định dạng hợp lệ: S00001");
        }
        if (!string.IsNullOrEmpty(request.Description) &&
            !Regex.IsMatch(request.Description, specialCharPattern))
        {
            throw new Exception("Mô tả dịch vụ chứa ký tự không hợp lệ.");
        }

        
        if (request.Price <= 0)
        {
            throw new Exception("Giá dịch vụ phải lớn hơn 0.");
        }

        service.Name = request.Name;
        service.Code = request.Code;
        service.Description = request.Description;
        service.ImageUrl = request.ImageUrl;
        service.Price = request.Price;
        service.Status = request.Status;
        service.DepartmentId = request.DepartmentId;
        service.UpdateDate = DateTime.Now;
        service.UpdateBy = "System"; 

        await _context.SaveChangesAsync();

        return await GetServiceByIdAsync(service.Id);
    }

    public async Task<bool> DeleteServiceAsync(int id)
    {
        var service = await _context.Set<Servicess>().FindAsync(id);
        if (service == null)
            return false;

        _context.Set<Servicess>().Remove(service);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ServiceResponseDTO>> GetServicesByDepartmentAsync(int departmentId)
    {
        var services = await _context.Set<Servicess>()
            .Include(s => s.Department)
            .Where(s => s.DepartmentId == departmentId)
            .Select(s => new ServiceResponseDTO
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                Price = s.Price,
                Status = s.Status,
                DepartmentId = s.DepartmentId,
                DepartmentName = s.Department != null ? s.Department.Name : null,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                CreateBy = s.CreateBy,
                UpdateBy = s.UpdateBy
            })
            .ToListAsync();

        return services;
    }
} 