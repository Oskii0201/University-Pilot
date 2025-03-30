namespace UniversityPilot.BLL.Areas.Schedule.Models
{
    public class WeekendAvailabilityDto
    {
        public int SemesterId { get; set; }
        public List<GroupDto> Groups { get; set; }
        public List<WeekendDto> Weekends { get; set; }
    }
}