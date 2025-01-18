using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Repositories
{
    internal class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}