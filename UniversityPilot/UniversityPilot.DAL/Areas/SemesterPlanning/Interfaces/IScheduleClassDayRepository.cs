using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces
{
    public interface IScheduleClassDayRepository : IRepository<ScheduleClassDay>
    {
        Task<List<ScheduleClassDay>> GetBySemesterIdAsync(int semesterId);

        Task UpdateAssignmentsAsync(int scheduleClassDayId, List<int> newStudyProgramIds);
    }
}