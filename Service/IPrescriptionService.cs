﻿using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Prescription;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service
{
    public interface IPrescriptionService
    {
        // Lấy tất cả đơn thuốc
        Task<IEnumerable<PrescriptionResponseDTO>> GetAllAsync();

        // Lấy đơn thuốc theo ID
        Task<PrescriptionResponseDTO> GetByIdAsync(int id);

        // Tạo đơn thuốc mới
        Task<PrescriptionResponseDTO> CreateAsync(PrescriptionRequest request);

        // Cập nhật đơn thuốc
        Task<PrescriptionResponseDTO> UpdateAsync(int id, PrescriptionRequest request);

        // Xóa đơn thuốc theo ID
        Task<bool> DeleteAsync(int id);

        // Lấy đơn thuốc theo DoctorId (dành cho bác sĩ và Admin)
        Task<IEnumerable<PrescriptionResponseDTO>> GetByDoctorIdAsync(int doctorId);

        // Lấy đơn thuốc theo PatientId (dành cho bác sĩ và Admin)
        Task<IEnumerable<PrescriptionResponseDTO>> GetByPatientIdAsync(int patientId);

        // Lấy đơn thuốc của người dùng hiện tại (bác sĩ hoặc bệnh nhân) dựa trên UserId và role
        Task<IEnumerable<PrescriptionResponseDTO>> GetMineAsync(int userId, string role);

        // Cập nhật trạng thái của đơn thuốc (dành cho bác sĩ và Admin)
        Task<PrescriptionResponseDTO> UpdateStatusAsync(int prescriptionId, PrescriptionStatus newStatus, string updatedBy);

        // Lấy đơn thuốc của bác sĩ hiện tại dựa trên UserId
        Task<IEnumerable<PrescriptionResponseDTO>> GetByUserIdAsync(int userId);
    }
}
