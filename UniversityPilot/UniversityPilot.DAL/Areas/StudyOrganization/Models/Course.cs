using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Course : IModelBase
    {
        public Course()
        {
            CoursesDetails = new HashSet<CourseDetails>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SemesterNumber { get; set; }

        public int StudyProgramId { get; set; }
        public virtual StudyProgram StudyProgram { get; set; }

        public int? SemesterId { get; set; }
        public virtual Semester? Semester { get; set; }

        public int? SpecializationId { get; set; }
        public virtual Specialization? Specialization { get; set; }

        public virtual ICollection<CourseDetails> CoursesDetails { get; set; }
    }
}