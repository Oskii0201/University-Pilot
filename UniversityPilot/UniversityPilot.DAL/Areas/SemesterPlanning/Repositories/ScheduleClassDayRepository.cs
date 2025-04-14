using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Repositories
{
    internal class ScheduleClassDayRepository : Repository<ScheduleClassDay>, IScheduleClassDayRepository
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

        public async Task<List<ScheduleClassDay>> GetWithClassDaysBySemesterAsync(int semesterId)
        {
            return await _context.ScheduleClassDays
                .Include(s => s.ClassDays)
                .Where(s => s.SemesterId == semesterId)
                .ToListAsync();
        }

        public override async Task DeleteAsync(ScheduleClassDay entity)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                    $@"DELETE FROM ""ScheduleClassDayStudyProgram""
                    WHERE ""ScheduleClassDaysId"" = {entity.Id}");

            _context.Entry(entity).Collection(e => e.StudyPrograms).CurrentValue = null;

            _context.Set<ScheduleClassDay>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssignmentsAsync(int scheduleClassDayId, List<int> newStudyProgramIds)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                    $@"DELETE FROM ""ScheduleClassDayStudyProgram""
                    WHERE ""ScheduleClassDaysId"" = {scheduleClassDayId}");

            foreach (var programId in newStudyProgramIds.Distinct())
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $@"INSERT INTO ""ScheduleClassDayStudyProgram"" (""ScheduleClassDaysId"", ""StudyProgramsId"")
                    VALUES ({scheduleClassDayId}, {programId})");
            }
        }

        public async Task<ScheduleClassDay?> GetBySemesterIdAndTitleAsync(int semesterId, string title)
        {
            return await _context.ScheduleClassDays
                .FirstOrDefaultAsync(s => s.SemesterId == semesterId && s.Title == title);
        }
    }
}