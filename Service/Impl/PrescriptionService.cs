using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly ApplicationDBContext _context;
        public PrescriptionService(ApplicationDBContext context) { _context = context; }

        public bool CreatePrescription(CreatePrescriptionDTO dto)
        {
            var prescription = new Prescription
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                Diagnosis = dto.Diagnosis,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateBy = "System", // hoặc DoctorId.ToString()
                UpdateBy = "System"
            };
            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();

            foreach (var item in dto.Items)
            {
                _context.PrescriptionDetails.Add(new PrescriptionDetail
                {
                    PrescriptionId = prescription.Id,
                    MedicineId = item.MedicineId,
                    Usage = item.Usage,
                    Quantity = item.Quantity
                });
            }

            _context.SaveChanges();
            return true;
        }
    }
}
