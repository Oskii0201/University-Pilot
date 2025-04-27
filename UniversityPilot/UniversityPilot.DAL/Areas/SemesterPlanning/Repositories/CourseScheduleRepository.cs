using Microsoft.EntityFrameworkCore;
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

        public async Task<List<CourseSchedule>> GetAllWithDetailsBySemesterIdAsync(int semesterId)
        {
            return await _context.CourseSchedules
                .Include(cs => cs.CoursesDetails)
                    .ThenInclude(cd => cd.Course)
                        .ThenInclude(c => c.StudyProgram)
                            .ThenInclude(sp => sp.ScheduleClassDays)
                .Include(cs => cs.CoursesDetails)
                    .ThenInclude(cd => cd.SharedCourseGroup)
                .Include(cs => cs.CoursesDetails)
                    .ThenInclude(cd => cd.CourseGroups)
                .Include(cs => cs.Instructor)
                .Include(cs => cs.CoursesGroups)
                .Where(cs => cs.CoursesDetails
                    .Any(cd => cd.Course.SemesterId == semesterId &&
                               cd.CourseGroups.Any()))
                .ToListAsync();
        }

        public async Task AssignCourseDetailsAsync(int scheduleId, int courseDetailsId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                        INSERT INTO ""CourseScheduleCourseDetails"" (""CoursesDetailsId"", ""CourseSchedulesId"")
                        SELECT {courseDetailsId}, {scheduleId}
                        WHERE NOT EXISTS (
                            SELECT 1 FROM ""CourseScheduleCourseDetails""
                            WHERE ""CoursesDetailsId"" = {courseDetailsId}
                              AND ""CourseSchedulesId"" = {scheduleId}
                        )");
        }

        public async Task AssignCourseGroupAsync(int scheduleId, int courseGroupId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                        INSERT INTO ""CourseScheduleCourseGroups"" (""CoursesGroupsId"", ""CourseSchedulesId"")
                        SELECT {courseGroupId}, {scheduleId}
                        WHERE NOT EXISTS (
                            SELECT 1 FROM ""CourseScheduleCourseGroups""
                            WHERE ""CoursesGroupsId"" = {courseGroupId}
                              AND ""CourseSchedulesId"" = {scheduleId}
                        )");
        }

        public async Task UpdateStartEndDateAsync(int courseScheduleId, DateTime startDateTime, DateTime endDateTime)
        {
            var startUtc = DateTime.SpecifyKind(startDateTime, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(endDateTime, DateTimeKind.Utc);

            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                        UPDATE ""CourseSchedules""
                        SET ""StartDateTime"" = {startUtc}, ""EndDateTime"" = {endUtc}
                        WHERE ""Id"" = {courseScheduleId}
                        ");
        }
    }
}