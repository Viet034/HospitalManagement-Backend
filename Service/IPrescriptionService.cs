using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Prescription;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;

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
    }

}
