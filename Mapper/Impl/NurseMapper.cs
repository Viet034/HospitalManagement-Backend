using AutoMapper;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using SWP391_SE1914_ManageHospital.Ultility;

namespace SWP391_SE1914_ManageHospital.Mapper
{
    public class NurseMapper : Profile
    {
        public NurseMapper()
        {
            CreateMap<Nurse, NurseDTO>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
                .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateBy))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdateBy));

            CreateMap<NurseDTO, Nurse>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Status.Gender>(src.Gender.ToString())))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<Status.NurseStatus>(src.Status.ToString())))
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => DateTime.Parse(src.Dob)))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
                .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateBy))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdateBy));

            CreateMap<Nurse_Appointment, Nurse_AppointmentDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Nurse_AppointmentDTO, Nurse_Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<Status.NurseAppointmentStatus>(src.Status.ToString())));
        }
    }
}