using UniversityPilot.BLL.Areas.Schedule.Interfaces;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    internal class ScheduleGenerator : IScheduleGenerator
    {
        public async Task GeneratePreliminaryScheduleAsync(int semesterId)
        {
            // TODO: implement logic later
            await Task.Delay(1000); // symulacja pracy
        }
    }
}