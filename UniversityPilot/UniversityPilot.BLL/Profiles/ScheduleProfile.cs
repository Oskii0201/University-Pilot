using AutoMapper;
using UniversityPilot.BLL.Areas.Schedule.Models;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;

namespace UniversityPilot.BLL.Profiles
{
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            CreateMap<Semester, SemesterDto>();
        }
    }
}