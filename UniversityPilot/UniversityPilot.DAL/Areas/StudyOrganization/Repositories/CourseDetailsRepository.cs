using Microsoft.EntityFrameworkCore;
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

        public async Task<List<CourseDetails>> GetCourseDetailsExport(int semesterId)
        {
            return await _context.CoursesDetails
                .Include(cd => cd.Course)
                    .ThenInclude(c => c.Specialization)
                .Include(cd => cd.Course)
                    .ThenInclude(c => c.StudyProgram)
                        .ThenInclude(sp => sp.FieldOfStudy)
                .Include(cd => cd.Course)
                    .ThenInclude(c => c.StudyProgram)
                        .ThenInclude(sp => sp.ScheduleClassDays)
                .Include(cd => cd.Instructors)
                .Include(cd => cd.CourseGroups)
                .Include(cd => cd.SharedCourseGroup)
                .Where(cd => cd.Course.SemesterId == semesterId)
                .ToListAsync();
        }

        public async Task<List<CourseDetails>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.CoursesDetails
                .Include(cd => cd.Course)
                    .ThenInclude(c => c.StudyProgram)
                .Include(cd => cd.CourseGroups)
                .Include(cd => cd.Instructors)
                .Include(cd => cd.SharedCourseGroup)
                .Where(cd => ids.Contains(cd.Id))
                .ToListAsync();
        }

        public async Task DetachNavigationPropertiesAsync(CourseDetails course)
        {
            _context.Entry(course).Collection(c => c.Instructors).CurrentValue = null;
            _context.Entry(course).Collection(c => c.CourseGroups).CurrentValue = null;
            _context.Entry(course).Reference(c => c.Course).CurrentValue = null;
            _context.Entry(course).Reference(c => c.SharedCourseGroup).CurrentValue = null;

            await Task.CompletedTask;
        }

        public async Task AssignInstructorAsync(int courseDetailsId, int instructorId)
        {
            var exists = await _context.Database
                .ExecuteSqlInterpolatedAsync($@"
            INSERT INTO ""CourseDetailsInstructor"" (""CoursesDetailsId"", ""InstructorsId"")
            SELECT {courseDetailsId}, {instructorId}
            WHERE NOT EXISTS (
                SELECT 1 FROM ""CourseDetailsInstructor""
                WHERE ""CoursesDetailsId"" = {courseDetailsId}
                  AND ""InstructorsId"" = {instructorId}
            )");
        }

        public async Task UnassignInstructorsAsync(int courseDetailsId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
        DELETE FROM ""CourseDetailsInstructor""
        WHERE ""CoursesDetailsId"" = {courseDetailsId}");
        }

        public async Task AssignCourseGroupAsync(int courseDetailsId, int groupId)
        {
            var exists = await _context.Database
                .ExecuteSqlInterpolatedAsync($@"
            INSERT INTO ""CourseGroupCourseDetails"" (""CourseDetailsId"", ""CourseGroupsId"")
            SELECT {courseDetailsId}, {groupId}
            WHERE NOT EXISTS (
                SELECT 1 FROM ""CourseGroupCourseDetails""
                WHERE ""CourseDetailsId"" = {courseDetailsId}
                  AND ""CourseGroupsId"" = {groupId}
            )");
        }

        public async Task UnassignCourseGroupsAsync(int courseDetailsId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
        DELETE FROM ""CourseGroupCourseDetails""
        WHERE ""CourseDetailsId"" = {courseDetailsId}");
        }

    }
}