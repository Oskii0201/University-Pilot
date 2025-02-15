using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Repositories
{
    internal class ScheduleClassDayRepository : Repository<Course>, IScheduleClassDayRepository
    {
        public ScheduleClassDayRepository(UniversityPilotContext context) : base(context)
        {
        }

        public async Task<List<ScheduleClassDay>> GetBySemesterIdAsync(int semesterId)
        {
            return await _context.ScheduleClassDays
                .Include(scd => scd.StudyPrograms)
                    .ThenInclude(sp => sp.FieldOfStudy)
                .Where(scd => scd.SemesterId == semesterId)
                .ToListAsync();
        }
    }
}