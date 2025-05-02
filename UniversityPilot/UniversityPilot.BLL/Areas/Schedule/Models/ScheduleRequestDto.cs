namespace UniversityPilot.BLL.Areas.Schedule.Models
{
    public class ScheduleRequestDto
    {
        public string Name { get; set; }
        public int Semester { get; set; }
        public DateTime CurrentDate { get; set; }
        public string ViewType { get; set; }
    }
}