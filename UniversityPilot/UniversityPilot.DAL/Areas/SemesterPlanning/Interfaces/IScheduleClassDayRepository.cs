using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces
{
    public interface IScheduleClassDayRepository : IRepository<ScheduleClassDay>
    {
        public Task<List<ScheduleClassDay>> GetBySemesterIdAsync(int semesterId);

        public Task<List<ScheduleClassDay>> GetWithClassDaysBySemesterAsync(int semesterId);

        public Task UpdateAssignmentsAsync(int scheduleClassDayId, List<int> newStudyProgramIds);

        public Task<ScheduleClassDay> GetBySemesterIdAndTitleAsync(int id, string scheduleTitle);
    }
}