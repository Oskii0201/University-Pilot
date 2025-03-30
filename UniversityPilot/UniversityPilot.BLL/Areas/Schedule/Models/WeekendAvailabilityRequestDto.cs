namespace UniversityPilot.BLL.Areas.Schedule.Models
{
    public class WeekendAvailabilityResponseDto
    {
        public int SemesterId { get; set; }
        public List<GroupDto> Groups { get; set; }
        public List<WeekendDto> Weekends { get; set; }
    }
}