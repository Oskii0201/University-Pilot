using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.UniversityAndScheduling.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Specialization : IModelBase
    {
        public Specialization()
        {
            FieldOfStudies = new HashSet<FieldOfStudy>();
            Students = new HashSet<Student>();
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FieldOfStudy> FieldOfStudies { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}