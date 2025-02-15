using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.AcademicCalendar.Models
{
    public class Semester : IModelBase
    {
        public Semester()
        {
            Courses = new HashSet<Course>();
            ScheduleClassDays = new HashSet<ScheduleClassDay>();
        }

        public int Id { get; set; }

        public ScheduleCreationStage CreationStage { get; set; }
        public string AcademicYear { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<ScheduleClassDay> ScheduleClassDays { get; set; }
    }
}