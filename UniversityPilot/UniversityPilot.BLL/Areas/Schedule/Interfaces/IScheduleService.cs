using UniversityPilot.BLL.Areas.Schedule.Models;

namespace UniversityPilot.BLL.Areas.Schedule.Interfaces
{
    public interface IScheduleService
    {
        Task<List<ScheduleItemDto>> GetScheduleAsync(ScheduleRequestDto request);
    }
}