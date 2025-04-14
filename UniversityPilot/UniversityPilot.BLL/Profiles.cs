using AutoMapper;
using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.BLL
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Classroom, ClassroomCsv>()
                .ForMember(dest => dest.ClassroomId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.RoomNumber));

            CreateMap<Instructor, InstructorCsv>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
        }
    }
}