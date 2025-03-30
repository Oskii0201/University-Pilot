using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Repositories
{
    public class ScheduleClassDayStudyProgramRepository : IScheduleClassDayStudyProgramRepository
    {
        private readonly UniversityPilotContext _context;

        public ScheduleClassDayStudyProgramRepository(UniversityPilotContext context)
        {
            _context = context;
        }

        public async Task<List<ScheduleClassDayStudyProgram>> GetByScheduleClassDayIdAsync(int scheduleClassDayId)
        {
            return await _context.Set<ScheduleClassDayStudyProgram>()
                .Where(x => x.ScheduleClassDayId == scheduleClassDayId)
                .ToListAsync();
        }

        public async Task<List<ScheduleClassDayStudyProgram>> GetAllBySemesterIdAsync(int semesterId)
        {
            return await _context.ScheduleClassDayStudyProgram
                .Include(x => x.StudyProgram)
                    .ThenInclude(sp => sp.FieldOfStudy)
                .Include(x => x.ScheduleClassDay)
                .Where(x => x.ScheduleClassDay.SemesterId == semesterId)
                .ToListAsync();
        }

        public async Task UpdateAssignmentsAsync(int scheduleClassDayId, List<int> newStudyProgramIds)
        {
            var existing = await GetByScheduleClassDayIdAsync(scheduleClassDayId);

            var toRemove = existing
                .Where(x => !newStudyProgramIds.Contains(x.StudyProgramId))
                .ToList();

            var existingIds = existing.Select(x => x.StudyProgramId).ToHashSet();
            var toAdd = newStudyProgramIds
                .Where(id => !existingIds.Contains(id))
                .Select(id => new ScheduleClassDayStudyProgram
                {
                    ScheduleClassDayId = scheduleClassDayId,
                    StudyProgramId = id
                })
                .ToList();

            _context.RemoveRange(toRemove);
            await _context.AddRangeAsync(toAdd);

            await _context.SaveChangesAsync();
        }
    }
}