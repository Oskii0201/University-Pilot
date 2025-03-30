using UniversityPilot.DAL.Areas.SemesterPlanning.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces
{
    public interface IScheduleClassDayStudyProgramRepository
    {
        Task<List<ScheduleClassDayStudyProgram>> GetByScheduleClassDayIdAsync(int scheduleClassDayId);

        Task<List<ScheduleClassDayStudyProgram>> GetAllBySemesterIdAsync(int semesterId);

        Task UpdateAssignmentsAsync(int scheduleClassDayId, List<int> newStudyProgramIds);
    }
}