using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Repositories
{
    internal class CourseScheduleRepository : Repository<CourseSchedule>, ICourseScheduleRepository
    {
        public CourseScheduleRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}