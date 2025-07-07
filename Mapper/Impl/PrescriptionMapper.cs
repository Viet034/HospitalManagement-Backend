using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Prescription;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl
{
    public class PrescriptionMapper : IPrescriptionMapper
    {
        // Ánh xạ từ entity Prescription sang DTO PrescriptionResponseDTO
        public PrescriptionResponseDTO MapToResponse(Prescription prescription)
        {
            return new PrescriptionResponseDTO
            {
                Id = prescription.Id,
                Note = prescription.Note,
                Status = (PrescriptionStatus)prescription.Status, // Chuyển từ int sang enum PrescriptionStatus
                PatientId = prescription.PatientId,
                DoctorId = prescription.DoctorId,
                Name = prescription.Name,
                Code = prescription.Code,
                CreateDate = prescription.CreateDate,
                UpdateDate = (DateTime)prescription.UpdateDate,
                CreateBy = prescription.CreateBy ?? "system",  // Nếu CreateBy null thì gán mặc định "system"
                UpdateBy = prescription.UpdateBy ?? "system"   // Nếu UpdateBy null thì gán mặc định "system"
            };
        }

        // Ánh xạ từ DTO PrescriptionRequest sang entity Prescription (dùng khi tạo mới hoặc cập nhật đơn thuốc)
        public Prescription MapToEntity(PrescriptionRequest request)
        {
            return new Prescription
            {
                Note = request.Note,
                Status = (PrescriptionStatus)request.Status,  // Chuyển từ enum PrescriptionStatus sang int khi lưu vào cơ sở dữ liệu
                PatientId = request.PatientId,
                //DoctorId = request.DoctorId,
                Name = request.Name,
                Code = request.Code,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateBy = "system",  // Thêm giá trị mặc định
                UpdateBy = "system"   // Thêm giá trị mặc định
            };
        }

        // Cập nhật entity Prescription với DTO PrescriptionRequest (dùng khi cập nhật đơn thuốc)
        public void MapToExistingEntity(PrescriptionRequest request, Prescription prescription)
        {
            prescription.Note = request.Note;
            prescription.Status = (PrescriptionStatus)request.Status;  // Chuyển từ enum PrescriptionStatus sang int khi lưu vào cơ sở dữ liệu
            prescription.PatientId = request.PatientId;
            //prescription.DoctorId = request.DoctorId;
            prescription.Name = request.Name;
            prescription.Code = request.Code;
            prescription.UpdateDate = DateTime.UtcNow;
            prescription.UpdateBy = "system";  // Thêm giá trị mặc định (hoặc có thể lấy từ người dùng)
        }
    }
}
