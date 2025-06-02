using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Clinic;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl;

public class ClinicService : IClinicService
{
    private readonly ApplicationDBContext _context;
    private readonly IClinicMapper _mapper;

    public ClinicService(ApplicationDBContext context, IClinicMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<string> CheckUniqueCodeAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ClinicResponseDTO> CreateClinicAsync(ClinicCreate create)
    {
        //requestDTO -> response
        Clinic entity =  _mapper.CreateToEntity(create);

        await _context.Clinics.AddAsync(entity);
        await _context.SaveChangesAsync();
        var response = _mapper.EntityToResponse(entity);
        return response;
    }

    public async Task<ClinicResponseDTO> FindClinicByIdAsync(int id)
    {
        var cid = await _context.Clinics.FindAsync(id)
            ?? throw new Exception($"Không có Id {id} tồn tại!");
        
        var response = _mapper.EntityToResponse(cid);
        return response;
    }

    public async Task<IEnumerable<ClinicResponseDTO>> GetAllClinicAsync()
    {
        var cid = await _context.Clinics.ToListAsync();
        if(cid == null)
        {
            throw new Exception("Không có bản ghi nào");
        }
        var response = _mapper.ListEntityToResponse(cid);
        return response;
    }

    /*Người dùng requestDTO -> Controller -> Service -> Database
                                Database -> service -> Controller -> Répsonse
    */
    public async Task<bool> HardDeleteClinicAsync(int id)
    {
        var cid = await _context.Clinics.FindAsync(id)       
          ??  throw new KeyNotFoundException($"Không có Id {id} tồn tại!");
        
         _context.Clinics.Remove(cid);
        await _context.SaveChangesAsync();
        return true;
        
    }

    public async Task<ClinicResponseDTO> SoftDeleteClinicAsync(int id, Status.ClinicStatus newStatus)
    {
        var coId = await _context.Clinics.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không có Id {id} tồn tại");

        coId.Status = newStatus;

        await _context.SaveChangesAsync();

        var response = _mapper.EntityToResponse(coId);
        return response;
    }

    public async Task<IEnumerable<ClinicResponseDTO>> SearchClinicByKeyAsync(string name)
    {
        var cid = await _context.Clinics.FromSqlRaw("Select * from Clinics where Name like {0}", "%" + name + "%").ToListAsync();
        if (cid == null)
        {
            throw new Exception($"Không có tên {name} nào tồn tại!");
        }
        var response = _mapper.ListEntityToResponse(cid);
        return response;
    }

    public async Task<ClinicResponseDTO> UpdateClinicAsync(int id, ClinicUpdate update)
    {

        var coId = await _context.Clinics.FindAsync(id)
            ?? throw new KeyNotFoundException($"Không có ID {id} tồn tại");

        var result = _mapper.UpdateToEntity(update);
        coId.Code = result.Code;
        coId.Name = result.Name;
        coId.Status = result.Status;
        coId.CreateDate = result.CreateDate;
        coId.UpdateDate = result.UpdateDate;
        coId.CreateBy = result.CreateBy;
        coId.UpdateBy = result.UpdateBy;
        await _context.SaveChangesAsync();

        var response =  _mapper.EntityToResponse(coId);
        return response;
    }


   
}
