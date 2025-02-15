using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.AcademicCalendar.Repositories
{
    internal class SemesterRepository : Repository<Semester>, ISemesterRepository
    {
        public SemesterRepository(UniversityPilotContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Semester>> GetUpcomingSemestersAsync(int count = 3)
        {
            return await _context.Semesters
                .Where(s => s.StartDate >= DateTime.UtcNow.Date)
                .OrderBy(s => s.StartDate)
                .Take(count)
                .ToListAsync();
        }
    }
}