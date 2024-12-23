using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Course : IModelBase
    {
        public Course()
        {
            Instructors = new HashSet<Instructor>();
            CoursesDetails = new HashSet<CourseDetails>();
            StudyPrograms = new HashSet<StudyProgram>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SemesterNumber { get; set; }

        public int? SemesterId { get; set; }
        public virtual Semester? Semester { get; set; }

        public int? SpecializationId { get; set; }
        public virtual Specialization? Specialization { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<CourseDetails> CoursesDetails { get; set; }
        public virtual ICollection<StudyProgram> StudyPrograms { get; set; }
    }
}