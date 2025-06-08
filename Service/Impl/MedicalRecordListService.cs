using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class MedicalRecordListService : IMedicalRecordListService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMedicalRecordListMapper _listMapper;

        public MedicalRecordListService(
            ApplicationDBContext context,
            IMedicalRecordListMapper listMapper)
        {
            _context = context;
            _listMapper = listMapper;
        }

        public IEnumerable<MedicalRecordResponse> GetMedicalRecordsByPatientId(int patientId)
        {
            try
            {
                var records = _context.Medical_Records
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Disease)
                    .Where(mr => mr.PatientId == patientId)
                    .OrderByDescending(mr => mr.CreateDate) // Sắp xếp theo ngày tạo mới nhất lên đầu
                    .ToList();

                return _listMapper.ListEntityToResponse(records);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách Medical Records", ex);
            }
        }
    }
}