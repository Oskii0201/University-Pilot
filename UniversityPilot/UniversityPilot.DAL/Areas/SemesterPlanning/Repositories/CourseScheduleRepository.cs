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
                .Include(cs => cs.CourseDetails)
                    .ThenInclude(cd => cd.Course)
                .Include(cs => cs.CourseDetails)
                    .ThenInclude(cd => cd.SharedCourseGroup)
                .Include(cs => cs.CourseDetails)
                    .ThenInclude(cd => cd.CourseGroups)
                .Include(cs => cs.Instructor)
                .Include(cs => cs.CourseGroup)
                .Where(cs => cs.CourseDetails.Course.SemesterId == semesterId)
                .ToListAsync();
        }
    }
}