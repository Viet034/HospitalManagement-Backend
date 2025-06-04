using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMedicalRecordMapper _mapper;

        public MedicalRecordService(ApplicationDBContext context, IMedicalRecordMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<MedicalRecordResponse> GetMedicalRecordsByPatientId(int patientId)
        {
            var records = _context.Medical_Records
                .Include(mr => mr.Doctor)
                .Include(mr => mr.Patient)
                .Include(mr => mr.Disease)
                .Where(mr => mr.PatientId == patientId)
                .OrderByDescending(mr => mr.CreateDate) // Lần khám gần nhất lên đầu
                .ToList();

            return _mapper.ListEntityToResponse(records);
        }

        public MedicalRecordResponse? GetMedicalRecordDetail(int medicalRecordId)
        {
            var record = _context.Medical_Records
                .Include(mr => mr.Doctor)
                .Include(mr => mr.Patient)
                .Include(mr => mr.Disease)
                .FirstOrDefault(mr => mr.Id == medicalRecordId);

            return record == null ? null : _mapper.EntityToResponse(record);
        }
    }
}