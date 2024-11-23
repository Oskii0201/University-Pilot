using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
{
    public class StudySchedule : IModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}