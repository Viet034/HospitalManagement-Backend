using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Prescription;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionResponseDTO>> GetAllAsync();
        Task<PrescriptionResponseDTO> GetByIdAsync(int id);
        Task<PrescriptionResponseDTO> CreateAsync(PrescriptionRequest request);
        Task<PrescriptionResponseDTO> UpdateAsync(int id, PrescriptionRequest request);
        Task<bool> DeleteAsync(int id);

        // Thêm phương thức để lấy đơn thuốc theo DoctorId
        Task<IEnumerable<PrescriptionResponseDTO>> GetByDoctorIdAsync(int doctorId);

        // Thêm phương thức để lấy đơn thuốc theo PatientId
        Task<IEnumerable<PrescriptionResponseDTO>> GetByPatientIdAsync(int patientId);

        // Thêm phương thức để lấy đơn thuốc của người dùng hiện tại (bác sĩ hoặc bệnh nhân)
        Task<IEnumerable<PrescriptionResponseDTO>> GetMineAsync(int userId, string role);

        Task<PrescriptionResponseDTO> UpdateStatusAsync(int prescriptionId, PrescriptionStatus newStatus, string updatedBy);
    }

}
