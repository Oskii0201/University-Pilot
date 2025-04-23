using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces
{
    public interface ICourseScheduleRepository : IRepository<CourseSchedule>
    {
        public Task AssignCourseDetailsAsync(int scheduleId, int courseDetailsId);

        public Task AssignCourseGroupAsync(int scheduleId, int courseGroupId);

        public Task<List<CourseSchedule>> GetAllWithDetailsBySemesterIdAsync(int semesterId);
    }
}