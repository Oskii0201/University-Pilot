using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Course : IModelBase
    {
        public Course()
        {
            StudyProgram = new HashSet<StudyProgram>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }
        public CourseTypes CourseType { get; set; }

        public int Hours { get; set; }
        public string AssessmentType { get; set; }
        public int ECTS { get; set; }

        public string? SpecializationId { get; set; }
        public virtual Specialization? Specialization { get; set; }

        public virtual IEnumerable<StudyProgram> StudyProgram { get; set; }
    }
}