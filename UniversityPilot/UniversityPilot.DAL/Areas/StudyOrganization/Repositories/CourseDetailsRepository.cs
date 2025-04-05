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
                .Include(cd => cd.Instructors)
                .Include(cd => cd.CourseGroups)
                .Where(cd => cd.Course.SemesterId == semesterId)
                .ToListAsync();
        }
    }
}