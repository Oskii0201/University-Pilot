namespace UniversityPilot.BLL.Areas.Schedule.Models
{
    public class WeekendDto
    {
        public DateTime Date { get; set; }
        public Dictionary<int, bool> Availability { get; set; } // key: GroupId
    }
}