using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Repositories
{
    internal class SharedCourseGroupRepository : Repository<SharedCourseGroup>, ISharedCourseGroupRepository
    {
        public SharedCourseGroupRepository(UniversityPilotContext context) : base(context)
        {
        }

        public async Task<SharedCourseGroup?> GetByNameWithCourseDetailsAsync(string name, CourseTypes courseType, string courseName)
        {
            return await _context.SharedCourseGroups
                .Include(sg => sg.CoursesDetails)
                    .ThenInclude(cd => cd.Course)
                .Where(scg => scg.CoursesDetails.Any(cd => cd.CourseType == courseType) &&
                              scg.CoursesDetails.Any(cd => cd.Course.Name == courseName))
                .FirstOrDefaultAsync(sg => sg.Name == name);
        }
    }
}