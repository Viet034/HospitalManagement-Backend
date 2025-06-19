using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_SE1914_ManageHospital.Models
{
	public class LabResult
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int LabRequestId { get; set; }

		[Required]
		[MaxLength(1000)]
		public string Conclusion { get; set; }

		// Đường dẫn file PDF hoặc ảnh (1 file duy nhất)
		[MaxLength(255)]
		public string AttachmentUrl { get; set; }

		[Required]
		public DateTime ResultDate { get; set; } = DateTime.Now;

		[ForeignKey("LabRequestId")]
		public virtual LabRequest LabRequest { get; set; }
	}
}