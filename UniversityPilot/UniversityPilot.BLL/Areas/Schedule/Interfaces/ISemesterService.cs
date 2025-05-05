using UniversityPilot.BLL.Areas.Schedule.Models;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.BLL.Areas.Schedule.Interfaces
{
    public interface ISemesterService
    {
        Task<IEnumerable<Semester>> GetUpcomingSemestersAsync(int count = 3, int status = 0);

        Task<List<Semester>> GetSemestersWithStatusAfterGroupScheduleAsync();

        Task<List<Semester>> GetByStatusAsync(ScheduleCreationStage stage);

        Task<int> GetStatusBySemesterIdAsync(int semesterId);

        Task<List<StudyProgramWithSemestersDto>> GetStudyProgramsWithSemestersAsync(int semesterId);
    }
}