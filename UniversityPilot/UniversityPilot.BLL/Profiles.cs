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
        }
    }
}