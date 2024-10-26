using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.UniversityAndScheduling.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Course : IModelBase
    {
        public Course()
        {
            CourseGroups = new HashSet<CourseGroup>();
            FieldOfStudies = new HashSet<FieldOfStudy>();
            Specializations = new HashSet<Specialization>();
            Instructors = new HashSet<Instructor>();
        }

        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int CourseTypeId { get; set; }
        public int Credits { get; set; }
        public float CourseDuration { get; set; }
        public bool Online { get; set; }

        public virtual CourseType CourseType { get; set; }
        public virtual ICollection<CourseGroup> CourseGroups { get; set; }
        public virtual ICollection<FieldOfStudy> FieldOfStudies { get; set; }
        public virtual ICollection<Specialization> Specializations { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}