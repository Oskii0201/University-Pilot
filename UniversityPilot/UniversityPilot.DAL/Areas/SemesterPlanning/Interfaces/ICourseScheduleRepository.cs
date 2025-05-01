using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces
{
    public interface ICourseScheduleRepository : IRepository<CourseSchedule>
    {
        Task AssignCourseDetailsAsync(int scheduleId, int courseDetailsId);

        Task AssignCourseGroupAsync(int scheduleId, int courseGroupId);

        Task<List<CourseSchedule>> GetAllWithDetailsBySemesterIdAsync(int semesterId);

        Task UpdateStartEndDateAsync(int courseScheduleId, DateTime start, DateTime end);

        Task<List<CourseSchedule>> GetWithDetailsAsync(int semesterNumber, DateTime from, DateTime to);
    }
}