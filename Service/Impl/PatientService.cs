using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Patient;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDBContext _context;
        private readonly IpatientMapper _mapper;

        public PatientService(ApplicationDBContext context, IpatientMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            do
            {
                newCode = "PAT" + Guid.NewGuid().ToString("N")[..5].ToUpper();
            } while (await _context.Patients.AnyAsync(p => p.Code == newCode));
            return newCode;
        }

        public async Task<PatientRespone> CreatePatientAsync(PatientCreate create)
        {
            if (create.UserId > 0)
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == create.UserId);
                if (!userExists)
                {
                    throw new Exception($"User with Id {create.UserId} does not exist.");
                }
            }
            Patient entity = _mapper.CreateToEntity(create);

            await _context.Patients.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.EntityToRespone(entity);
        }


        public async Task<PatientRespone> FindPatientByIdAsync(int id)
        {
            var patient =  await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                throw new Exception($"Can not find patient id: {id}");
            }
            return _mapper.EntityToRespone(patient);
        }

        public async Task<IEnumerable<PatientRespone>> GetAllPatientAsync()
        {
            var patients = await _context.Patients.ToListAsync();
            if (!patients.Any())
            {
                throw new Exception("No Patient");
            }

            return _mapper.ListEntityToRespone(patients);
        }

        public async Task<bool> HardDeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                throw new Exception($"Can not find patient ID: {id}");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PatientRespone>> SearchPatientByKeyAsync(string key)
        {
            var result = await _context.Patients
                .Where(p => p.Name.Contains(key) || p.Phone.Contains(key) || p.CCCD.Contains(key) || p.Code.Contains(key))
                .ToListAsync();

            if (!result.Any())
            {
                throw new Exception($"Can not find patient with key: \"{key}\".");
            }

            return _mapper.ListEntityToRespone(result);
        }

        public async Task<PatientRespone> SoftDeletePatientColorAsync(int id, Status.PatientStatus newStatus)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                throw new Exception($"Can not fint patient with ID: {id}");
            }

            patient.Status = newStatus;
            patient.UpdateDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return _mapper.EntityToRespone(patient);
        }

        public async Task<PatientRespone> UpdatePatientAsync(int id, PatientUpdate update)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                throw new Exception($"Can not fint patient with ID: {id}");
            }

            
            patient.Name = update.Name;
            patient.Gender = update.Gender;
            patient.Dob = update.Dob;
            patient.CCCD = update.CCCD;
            patient.Phone = update.Phone;
            patient.EmergencyContact = update.EmergencyContact;
            patient.Address = update.Address;
            patient.InsuranceNumber = update.InsuranceNumber;
            patient.Allergies = update.Allergies;
            patient.BloodType = update.BloodType;
            patient.ImageURL = update.ImageURL;
            patient.Status = update.Status;
            patient.UpdateBy = update.UpdateBy;
            patient.UpdateDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return _mapper.EntityToRespone(patient);
        }

        public async Task<List<MedicalRecordHistoryDTO>> GetMedicalHistoryByPatientId(int patientId)
        {
            var records = await _context.Medical_Records
                .Include(m => m.Doctor)
                .Include(m => m.Appointment)
                    .ThenInclude(a => a.Clinic)
                .Where(m => m.PatientId == patientId)
                .Select(m => new MedicalRecordHistoryDTO
                {
                    AppointmentCode = m.Appointment.Code,
                    Diagnosis = m.Diagnosis,
                    TestResults = m.TestResults,
                    DoctorName = m.Doctor.Name,
                    ClinicName = m.Appointment.Clinic.Name,
                    AppointmentDate = m.Appointment.AppointmentDate
                }).ToListAsync();

            return records;
        }

        public async Task<List<PrescriptionDTO>> GetPrescriptionsByPatientId(int patientId)
        {
            var result = await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.PrescriptionDetails)
                    .ThenInclude(d => d.Medicine)
                .Where(p => p.PatientId == patientId)
                .Select(p => new PrescriptionDTO
                {
                    PrescriptionCode = p.Code,
                    CreateDate = p.CreateDate,
                    DoctorName = p.Doctor.Name,
                    Medicines = p.PrescriptionDetails.Select(d => new MedicineDetailDTO
                    {
                        MedicineName = d.Medicine.Name,
                        Quantity = d.Quantity,
                        Usage = d.Usage
                    }).ToList()
                }).ToListAsync();

            return result;
        }
    }

}
