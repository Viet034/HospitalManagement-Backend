
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Department;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using System.Security.Cryptography;
using System.Text.RegularExpressions;


namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class DepartmentService : IDepartmentService
{
    private readonly ApplicationDBContext _context;
    private readonly IDepartmentMapper _mapper;

    public DepartmentService(ApplicationDBContext context, IDepartmentMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<string> CheckUniqueCodeAsync()
    {
        string newCode;
        bool isExist;

        do
        {
            newCode = GenerateCode.GenerateDepartmentCode();
            _context.ChangeTracker.Clear();
            isExist = await _context.Departments.AnyAsync(p => p.Code == newCode);
        }
        while (isExist);

        return newCode;
    }

    public async Task<DepartmentResponseDTO> CreateDepartmentAsync(DepartmentCreate create)
    {
        create.Name = create.Name.Trim();
        if (string.IsNullOrEmpty(create.Name))
            throw new Exception("Không được để trống tên");

        if (!Regex.IsMatch(create.Name, @"^[a-zA-ZÀ-ỹĂăÂâĐđÊêÔôƠơƯư\s]+$"))
            throw new Exception("Tên không được chứa kí tự đặc biệt");
        if (await _context.Departments.AnyAsync(x => x.Name == create.Name))
        {
            throw new Exception("Tên đã được sử dụng");
        }
        //requestDTO -> response
        Department entity = _mapper.CreateToEntity(create);
        if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
        {
            entity.Code = create.Code;
        }
        else
        {
            entity.Code = await CheckUniqueCodeAsync();
        }

        while (await _context.Departments.AnyAsync(p => p.Code == entity.Code))
        {
            entity.Code = await CheckUniqueCodeAsync();
        }
        await _context.Departments.AddAsync(entity);
        await _context.SaveChangesAsync();
        var response = _mapper.EntityToResponse(entity);
        return response;
    }

    public async Task<DepartmentResponseDTO> FindDepartmentByIdAsync(int id)
    {
        var coId = await _context.Departments.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không có Id {id} tồn tại");

        var response = _mapper.EntityToResponse(coId);
        return response;
    }

    public async Task<IEnumerable<DepartmentResponseDTO>> GetAllDepartmentAsync()
    {
        var co = await _context.Departments.OrderByDescending(x => x.CreateDate).ToListAsync();
        var result = new List<DepartmentResponseDTO>();

        foreach (var d in co)
        {
            var doctorCount = await _context.Doctors.CountAsync(x => x.DepartmentId == d.Id);
            var nurseCount = await _context.Nurses.CountAsync(x => x.DepartmentId == d.Id);
            var total = doctorCount + nurseCount;

            var dto = _mapper.EntityToResponse(d);
            dto.TotalAmountOfPeople = total;
            result.Add(dto);
        }

        return result; 
    }

    public async Task<bool> HardDeleteDepartmentAsync(int id)
    {
        var coId = await _context.Departments.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không có Id {id} tồn tại");

        _context.Departments.Remove(coId);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<DepartmentResponseDTO>> SearchDepartmentByKeyAsync(string name)
    {
        var cid = await _context.Departments.FromSqlRaw("Select * from Departments where Name like {0}", "%" + name + "%").ToListAsync();
        if (cid == null)
        {
            throw new Exception($"Không có tên {name} nào tồn tại!");
        }
        var response = _mapper.ListEntityToResponse(cid);
        return response;
    }

    public async Task<DepartmentResponseDTO> SoftDeleteDepartmentAsync(int id, Status.DepartmentStatus newStatus)
    {
        var coId = await _context.Departments.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không có Id {id} tồn tại");

        coId.Status = newStatus;

        await _context.SaveChangesAsync();

        var response = _mapper.EntityToResponse(coId);
        return response;
    }

    public async Task<DepartmentResponseDTO> UpdateDepartmentAsync(int id, DepartmentUpdate update)
    {
        var coId = await _context.Departments.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không có ID {id} tồn tại");
        if (await _context.Departments.AnyAsync(x => x.Name == update.Name))
        {
            throw new Exception("Tên đã được sử dụng");
        }
        var result = _mapper.UpdateToEntity(update);
        coId.Code = update.Code;
        coId.Name = result.Name;
        coId.Description = result.Description;
        coId.TotalAmountOfPeople = result.TotalAmountOfPeople;
        coId.Status = result.Status;
        coId.CreateDate = result.CreateDate;
        coId.UpdateDate = result.UpdateDate;
        coId.CreateBy = result.CreateBy;
        coId.UpdateBy = result.UpdateBy;
        await _context.SaveChangesAsync();

        var response = _mapper.EntityToResponse(coId);
        return response;
    }
}
