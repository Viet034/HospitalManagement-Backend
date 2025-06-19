using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_SE1914_ManageHospital.Models
{
	public class LabRequest
	{
		public int Id { get; set; } // Primary key
		public int DoctorId { get; set; } // Id bác sĩ chỉ định
		public int PatientId { get; set; } // Id bệnh nhân
		public int AppointmentId { get; set; } // Mã cuộc hẹn
		public int MedicalRecordId { get; set; } // Mã hồ sơ y tế
		public string TestType { get; set; } // Loại xét nghiệm (Enum hoặc string)
		public string Description { get; set; } // Mô tả về lý do, yêu cầu xét nghiệm
		public DateTime RequestDate { get; set; } // Ngày yêu cầu xét nghiệm
		public string Status { get; set; } // Trạng thái phiếu (Chưa thực hiện, Đang thực hiện, Hoàn thành)
		public string ResultText { get; set; } // Kết quả xét nghiệm dưới dạng text
		public string ResultFileUrl { get; set; } // Đường dẫn file kết quả (PDF hoặc ảnh)
		public DateTime ResultDate { get; set; } // Ngày nhận kết quả xét nghiệm
		public DateTime CreateDate { get; set; } // Ngày tạo phiếu chỉ định
		public DateTime UpdateDate { get; set; } // Ngày cập nhật phiếu chỉ định
		public string CreateBy { get; set; } // Người tạo phiếu
		public string UpdateBy { get; set; } // Người cập nhật phiếu

		// Quan hệ
		[ForeignKey("PatientId")]
		public virtual Patient Patient { get; set; }

		[ForeignKey("DoctorId")]
		public virtual User Doctor { get; set; }
	}
}
