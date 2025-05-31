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

    public async Task<ClinicResponse> CreateClinicAsync(ClinicCreate create)
    {
        //requestDTO -> response
        Clinic entity =  _mapper.CreateToEntity(create);

        await _context.Clinics.AddAsync(entity);
        _context.SaveChangesAsync();
        var response = _mapper.EntityToResponse(entity);
        return response;
    }

    public async Task<ClinicResponse> FindClinicByIdAsync(int id)
    {
        var cid = await _context.Clinics.FindAsync(id);
        if(cid == null)
        {
            throw new Exception($"Không có Id {id} tồn tại!");
        }
        var response = _mapper.EntityToResponse(cid);
        return response;
    }

    public async Task<IEnumerable<ClinicResponse>> GetAllClinicAsync()
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
        var cid = await _context.Clinics.FindAsync(id);
        if (cid == null)
        {
            throw new Exception($"Không có Id {id} tồn tại!");
        }
        _context.Clinics.Remove(cid);
        _context.SaveChangesAsync();
        return true;
        
    }

    public async Task<ClinicResponse> SoftDeleteClinicColorAsync(int id, Status.ClinicStatus newStatus)
    {
        var coId = await _context.Clinics.FindAsync(id);
        if (coId == null) throw new Exception($"Không có Id {id} tồn tại");

        coId.Status = newStatus;

        await _context.SaveChangesAsync();

        var response = _mapper.EntityToResponse(coId);
        return response;
    }

    public async Task<IEnumerable<ClinicResponse>> SearchClinicByKeyAsync(string key, string code)
    {
        var cid = await _context.Clinics.Where(x => x.Name == key || x.Code == code).ToListAsync();
        if (cid == null)
        {
            throw new Exception($"Không có tên {key} nào tồn tại!");
        }
        var response = _mapper.ListEntityToResponse(cid);
        return response;
    }

    public Task<ClinicResponse> UpdateClinicAsync(int id, ClinicUpdate update)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ClinicResponse>> SearchClinicByKeyAsync(string key)
    {
        throw new NotImplementedException();
    }

   
}
