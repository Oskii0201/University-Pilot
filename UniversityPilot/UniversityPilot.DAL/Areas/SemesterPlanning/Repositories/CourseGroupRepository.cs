using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Repositories
{
    internal class CourseGroupRepository : Repository<CourseGroup>, ICourseGroupRepository
    {
        public CourseGroupRepository(UniversityPilotContext context) : base(context)
        {
        }

        public async Task<List<CourseGroup>> GetByNamesAndTypesAsync(IEnumerable<(string Name, CourseTypes Type)> descriptors)
        {
            var nameList = descriptors.Select(d => d.Name).Distinct().ToList();
            var typeList = descriptors.Select(d => d.Type).Distinct().ToList();

            return await _context.CourseGroups
                .Where(g => nameList.Contains(g.GroupName) && typeList.Contains(g.CourseType))
                .ToListAsync();
        }
    }
}