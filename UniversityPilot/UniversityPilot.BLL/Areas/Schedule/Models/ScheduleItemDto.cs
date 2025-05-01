namespace UniversityPilot.BLL.Areas.Schedule.Models
{
    public class ScheduleItemDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string GroupName { get; set; }
        public string Instructor { get; set; }
        public string Room { get; set; }
    }
}