using UniversityPilot.DAL.Areas.Identity.Models;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;
using UniversityPilot.DAL.Areas.UniversityAndScheduling.Models;

namespace UniversityPilot.DAL.Areas.Students.Models
{
    public class Student : User
    {
        public Student() : base()
        {
            CourseGroups = new HashSet<CourseGroup>();
            FieldOfStudies = new HashSet<FieldOfStudy>();
            Specializations = new HashSet<Specialization>();
        }

        public int Indeks { get; set; }

        public virtual ICollection<CourseGroup> CourseGroups { get; set; }
        public virtual ICollection<FieldOfStudy> FieldOfStudies { get; set; }
        public virtual ICollection<Specialization> Specializations { get; set; }
    }
}