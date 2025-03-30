using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Models
{
    public class ScheduleClassDay : IModelBase
    {
        public ScheduleClassDay()
        {
            StudyPrograms = new HashSet<StudyProgram>();
            ClassDays = new HashSet<ClassDay>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public int SemesterId { get; set; }
        public virtual Semester Semester { get; set; }

        public virtual ICollection<StudyProgram> StudyPrograms { get; set; }
        public virtual ICollection<ClassDay> ClassDays { get; set; }
    }
}