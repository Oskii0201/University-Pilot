using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Interfaces
{
    public interface ICourseDetailsRepository : IRepository<CourseDetails>
    {
        public Task<List<CourseDetails>> GetCourseDetailsExport(int semesterId);
    }
}