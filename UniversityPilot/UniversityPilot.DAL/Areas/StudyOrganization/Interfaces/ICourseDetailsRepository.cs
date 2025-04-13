using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Interfaces
{
    public interface ICourseDetailsRepository : IRepository<CourseDetails>
    {
        public Task<List<CourseDetails>> GetCourseDetailsExport(int semesterId);

        public Task<List<CourseDetails>> GetByIdsAsync(IEnumerable<int> ids);

        public Task DetachNavigationPropertiesAsync(CourseDetails course);

        public Task AssignInstructorAsync(int courseDetailsId, int instructorId);

        public Task UnassignInstructorsAsync(int courseDetailsId);

        public Task AssignCourseGroupAsync(int courseDetailsId, int groupId);

        public Task UnassignCourseGroupsAsync(int courseDetailsId);

        public Task<List<CourseDetails>> GetCourseDetailsWithDependenciesAsync(int semesterId);
    }
}