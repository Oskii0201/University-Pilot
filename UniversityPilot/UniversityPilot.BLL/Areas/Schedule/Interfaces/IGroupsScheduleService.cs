using UniversityPilot.BLL.Areas.Schedule.Models;

namespace UniversityPilot.BLL.Areas.Schedule.Interfaces
{
    public interface IGroupsScheduleService
    {
        Task<IEnumerable<SemesterDTO>> GetUpcomingSemestersAsync(int count = 3);
    }
}