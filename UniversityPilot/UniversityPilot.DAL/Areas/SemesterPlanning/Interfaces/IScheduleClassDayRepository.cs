using UniversityPilot.DAL.Areas.SemesterPlanning.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces
{
    public interface IScheduleClassDayRepository
    {
        Task<List<ScheduleClassDay>> GetBySemesterIdAsync(int semesterId);
    }
}