using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Service;
using Microsoft.EntityFrameworkCore;

public class MedicalRecordAdminService : IMedicalRecordAdminService
{
    private readonly ApplicationDBContext _context;

    public MedicalRecordAdminService(ApplicationDBContext context)
    {
        _context = context;
    }

    public List<MedicalRecordAdminResponse> GetAllMedicalRecords(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Medical_Records
            .Include(mr => mr.Doctor)
            .Include(mr => mr.Patient)
            .Include(mr => mr.Disease)
            .Include(mr => mr.Appointment)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(mr => mr.CreateDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(mr => mr.CreateDate <= endDate.Value);

        var records = query
            .Select(mr => new MedicalRecordAdminResponse
            {
                Id = mr.Id,
                Status = (int)mr.Status,
                Diagnosis = mr.Diagnosis ?? "",
                TestResults = mr.TestResults ?? "",
                Notes = mr.Notes ?? "",
                AppointmentId = mr.AppointmentId,
                PatientId = mr.PatientId,
                DoctorId = mr.DoctorId,
                PrescriptionId = (int)mr.PrescriptionId,
                DiseaseId = (int)mr.DiseaseId,
                Name = mr.Name ?? "",
                Code = mr.Code ?? "",
                CreateDate = mr.CreateDate,
                UpdateDate = mr.UpdateDate,
                CreateBy = mr.CreateBy ?? "",
                UpdateBy = mr.UpdateBy ?? "",
                DoctorName = mr.Doctor != null ? mr.Doctor.Name : null,
                PatientName = mr.Patient != null ? mr.Patient.Name : null,
                DiseaseName = mr.Disease != null ? mr.Disease.Name : null,
                AppointmentName = mr.Appointment != null ? mr.Appointment.Name : null
            })
            .OrderByDescending(mr => mr.CreateDate)
            .ToList();

        return records;
    }

    // *** API lấy theo tên bác sĩ ***
    public List<MedicalRecordAdminResponse> GetMedicalRecordsByDoctorName(string doctorName)
    {
        var query = _context.Medical_Records
            .Include(mr => mr.Doctor)
            .Include(mr => mr.Patient)
            .Include(mr => mr.Disease)
            .Include(mr => mr.Appointment)
            .Where(mr => mr.Doctor != null && mr.Doctor.Name.Contains(doctorName))
            .Select(mr => new MedicalRecordAdminResponse
            {
                Id = mr.Id,
                Status = (int)mr.Status,
                Diagnosis = mr.Diagnosis ?? "",
                TestResults = mr.TestResults ?? "",
                Notes = mr.Notes ?? "",
                AppointmentId = mr.AppointmentId,
                PatientId = mr.PatientId,
                DoctorId = mr.DoctorId,
                PrescriptionId = (int)mr.PrescriptionId,
                DiseaseId = (int)mr.DiseaseId,
                Name = mr.Name ?? "",
                Code = mr.Code ?? "",
                CreateDate = mr.CreateDate,
                UpdateDate = mr.UpdateDate,
                CreateBy = mr.CreateBy ?? "",
                UpdateBy = mr.UpdateBy ?? "",
                DoctorName = mr.Doctor != null ? mr.Doctor.Name : null,
                PatientName = mr.Patient != null ? mr.Patient.Name : null,
                DiseaseName = mr.Disease != null ? mr.Disease.Name : null,
                AppointmentName = mr.Appointment != null ? mr.Appointment.Name : null
            })
            .OrderByDescending(mr => mr.CreateDate)
            .ToList();

        return query;
    }
}
