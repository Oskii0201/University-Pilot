namespace UniversityPilot.BLL.Areas.Schedule.Interfaces
{
    public interface IScheduleGenerator
    {
        Task GeneratePreliminaryScheduleAsync(int semesterId);
    }
}