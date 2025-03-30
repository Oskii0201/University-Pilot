using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces
{
    public interface IClassDayRepository : IRepository<ClassDay>
    {
        Task<ClassDay?> GetByDateAsync(DateTime date);

        Task AssignToScheduleClassDayAsync(int classDayId, int scheduleClassDayId);

        Task<List<ClassDay>> GetBySemesterDatesAsync(DateTime start, DateTime end);

        Task UnassignFromScheduleClassDayAsync(int classDayId, int scheduleClassDayId);
    }
}