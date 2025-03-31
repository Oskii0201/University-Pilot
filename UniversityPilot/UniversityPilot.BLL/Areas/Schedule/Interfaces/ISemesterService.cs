using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.BLL.Areas.Schedule.Interfaces
{
    public interface ISemesterService
    {
        Task<IEnumerable<Semester>> GetUpcomingSemestersAsync(int count = 3);

        Task<List<Semester>> GetAllExceptNewAsync();

        Task<List<Semester>> GetByStatusAsync(ScheduleCreationStage stage);

        Task<string> GetStatusBySemesterIdAsync(int semesterId);
    }
}