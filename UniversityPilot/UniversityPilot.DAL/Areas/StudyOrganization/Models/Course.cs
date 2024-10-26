using UniversityPilot.DAL.Areas.UniversityAndScheduling.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int CourseTypeID { get; set; }
        public int Credits { get; set; }
        public float CourseDuration { get; set; }
        public bool Online { get; set; }

        public CourseType CourseType { get; set; }
        public ICollection<CourseGroup> CourseGroups { get; set; }
        public ICollection<CourseFieldOfStudy> CourseFieldsOfStudy { get; set; }
        public ICollection<CourseSpecialization> CourseSpecializations { get; set; }
    }
}