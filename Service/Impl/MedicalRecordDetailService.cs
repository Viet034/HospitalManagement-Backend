using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicalRecord;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class MedicalRecordDetailService : IMedicalRecordDetailService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMedicalRecordDetailMapper _detailMapper;

        public MedicalRecordDetailService(
            ApplicationDBContext context,
            IMedicalRecordDetailMapper detailMapper)
        {
            _context = context;
            _detailMapper = detailMapper;
        }

        public MedicalRecordDetailResponse? GetMedicalRecordDetail(int medicalRecordId)
        {
            try
            {
                var record = _context.Medical_Records
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Disease)
                    .FirstOrDefault(mr => mr.Id == medicalRecordId);

                return record == null ? null : _detailMapper.EntityToDetailResponse(record);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chi tiết Medical Record ID: {medicalRecordId}", ex);
            }
        }

        public MedicalRecordDetailResponse CreateMedicalRecord(MedicalRecordCreateRequest request)
        {
            try
            {
                var entity = _detailMapper.CreateRequestToEntity(request);
                _context.Medical_Records.Add(entity);
                _context.SaveChanges();

                // Load navigation properties for response
                _context.Entry(entity).Reference(e => e.Doctor).Load();
                _context.Entry(entity).Reference(e => e.Patient).Load();
                _context.Entry(entity).Reference(e => e.Disease).Load();

                return _detailMapper.EntityToDetailResponse(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo mới Medical Record", ex);
            }
        }

        public MedicalRecordDetailResponse UpdateMedicalRecord(int id, MedicalRecordUpdateRequest request)
        {
            try
            {
                var entity = _context.Medical_Records
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Disease)
                    .FirstOrDefault(mr => mr.Id == id);

                if (entity == null)
                    throw new Exception("Medical record not found");

                _detailMapper.UpdateEntityFromRequest(entity, request);
                _context.SaveChanges();

                // Reload navigation properties in case of change
                _context.Entry(entity).Reference(e => e.Doctor).Load();
                _context.Entry(entity).Reference(e => e.Patient).Load();
                _context.Entry(entity).Reference(e => e.Disease).Load();

                return _detailMapper.EntityToDetailResponse(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật Medical Record ID: {id}", ex);
            }
        }

        public bool DeleteMedicalRecord(int id)
        {
            try
            {
                var entity = _context.Medical_Records.FirstOrDefault(mr => mr.Id == id);
                if (entity == null)
                    return false;

                _context.Medical_Records.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xoá Medical Record ID: {id}", ex);
            }
        }
    }
}