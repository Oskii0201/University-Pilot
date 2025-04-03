using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.DAL.Areas.AcademicCalendar.Repositories
{
    internal class SemesterRepository : Repository<Semester>, ISemesterRepository
    {
        public SemesterRepository(UniversityPilotContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Semester>> GetUpcomingSemestersAsync(int count = 3, int status = 0)
        {
            return await _context.Semesters
                .Where(s => s.StartDate >= DateTime.UtcNow.Date &&
                            s.CreationStage == (ScheduleCreationStage)status)
                .OrderBy(s => s.StartDate)
                .Take(count)
                .ToListAsync();
        }
    }
}