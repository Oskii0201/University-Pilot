using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Course : IModelBase
    {
        public Course()
        {
            CourseGroups = new List<CourseGroup>();
            StudyProgram = new HashSet<StudyProgram>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SemesterNumber { get; set; }
        public CourseTypes CourseType { get; set; }

        public int Hours { get; set; }
        public string AssessmentType { get; set; }
        public int ECTS { get; set; }

        public int? SemesterId { get; set; }
        public virtual Semester? Semester { get; set; }

        public int? SpecializationId { get; set; }
        public virtual Specialization? Specialization { get; set; }

        public int? SharedCourseGroupId { get; set; }
        public virtual SharedCourseGroup SharedCourseGroup { get; set; }

        public virtual ICollection<CourseGroup> CourseGroups { get; set; }
        public virtual ICollection<StudyProgram> StudyProgram { get; set; }
    }
}