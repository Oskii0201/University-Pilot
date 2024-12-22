using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Models
{
    public class ClassDay : IModelBase
    {
        public ClassDay()
        {
            ScheduleClassDays = new HashSet<ScheduleClassDay>();
        }

        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual ICollection<ScheduleClassDay> ScheduleClassDays { get; set; }
    }
}