using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Course : IModelBase
    {
        public Course()
        {
            StudyProgram = new HashSet<StudyProgram>();
        }

        public int Id { get; set; }
        public string CourseName { get; set; }
        public int Semester { get; set; }
        public string CourseType { get; set; }
        public string Specialization { get; set; }
        public int Hours { get; set; }
        public string AssessmentType { get; set; }
        public int ECTS { get; set; }

        public virtual IEnumerable<StudyProgram> StudyProgram { get; set; }
    }
}