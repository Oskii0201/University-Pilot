using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Repositories
{
    internal class CourseGroupRepository : Repository<CourseGroup>, ICourseGroupRepository
    {
        public CourseGroupRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}