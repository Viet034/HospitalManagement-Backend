using AutoMapper;
using SWP391_SE1914_ManageHospital.DTOs;  // Đảm bảo sử dụng đúng namespace
using SWP391_SE1914_ManageHospital.Models; // Đảm bảo sử dụng đúng namespace

namespace YourProject.Mapping
{
	public class YourMappingProfile : Profile
	{
		public YourMappingProfile()
		{
			// Cấu hình ánh xạ giữa LabRequest và LabRequestDTO
			CreateMap<LabRequest, LabRequestDto>()
				.ForMember(dest => dest.ResultFileUrl, opt => opt.MapFrom(src => src.ResultFileUrl))
				.ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.RequestDate))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
			// Thêm các trường ánh xạ khác nếu cần

			CreateMap<LabRequestDto, LabRequest>()
				.ForMember(dest => dest.ResultFileUrl, opt => opt.MapFrom(src => src.ResultFileUrl))
				.ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.RequestDate))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
			// Thêm các trường ánh xạ ngược lại nếu cần
		}
	}
}
