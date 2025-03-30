using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Repositories
{
    internal class ClassDayRepository : Repository<ClassDay>, IClassDayRepository
    {
        public ClassDayRepository(UniversityPilotContext context) : base(context)
        {
        }

        public async Task<ClassDay?> GetByDateAsync(DateTime date)
        {
            var start = date.Date;
            var end = date.Date.AddDays(1);

            return await _context.ClassDays
                .FirstOrDefaultAsync(cd => cd.StartDateTime >= start && cd.StartDateTime < end);
        }

        public async Task AssignToScheduleClassDayAsync(int classDayId, int scheduleClassDayId)
        {
            var exists = await _context
                .Set<ScheduleClassDay>()
                .Where(scd => scd.Id == scheduleClassDayId)
                .SelectMany(scd => scd.ClassDays)
                .AnyAsync(cd => cd.Id == classDayId);

            if (!exists)
            {
                await _context
                    .Database
                    .ExecuteSqlInterpolatedAsync(
                        $@"INSERT INTO ""ScheduleClassDayClassDay"" (""ScheduleClassDaysId"", ""ClassDaysId"")
                   VALUES ({scheduleClassDayId}, {classDayId})");
            }
        }

        public async Task<List<ClassDay>> GetBySemesterDatesAsync(DateTime start, DateTime end)
        {
            return await _context.ClassDays
                .Where(cd => cd.StartDateTime.Date >= start.Date && cd.StartDateTime.Date <= end.Date)
                .Include(cd => cd.ScheduleClassDays)
                .ToListAsync();
        }
    }
}