using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

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
    }
}