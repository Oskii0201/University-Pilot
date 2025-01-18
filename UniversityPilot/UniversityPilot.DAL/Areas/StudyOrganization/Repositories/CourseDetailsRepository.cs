using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Repositories
{
    internal class CourseDetailsRepository : Repository<CourseDetails>, ICourseDetailsRepository
    {
        public CourseDetailsRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}