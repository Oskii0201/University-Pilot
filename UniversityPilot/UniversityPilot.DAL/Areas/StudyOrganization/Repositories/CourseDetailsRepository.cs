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
                    .ThenInclude(c => c.StudyProgram)
                        .ThenInclude(sp => sp.FieldOfStudy)
                .Include(cd => cd.Instructors)
                .Include(cd => cd.CourseGroups)
                .Include(cd => cd.SharedCourseGroup)
                .Where(cd => cd.Course.SemesterId == semesterId)
                .ToListAsync();
        }

        public async Task<List<CourseDetails>> GetByIdsWithIncludesAsync(IEnumerable<int> ids)
        {
            return await _context.CoursesDetails
                .Where(cd => ids.Contains(cd.Id))
                .Include(cd => cd.Instructors)
                .Include(cd => cd.CourseGroups)
                .ToListAsync();
        }
    }
}