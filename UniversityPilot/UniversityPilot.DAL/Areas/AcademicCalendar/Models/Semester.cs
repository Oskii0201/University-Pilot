using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.AcademicCalendar.Models
{
    public class Semester : IModelBase
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}