using UniversityPilot.DAL.Areas.Identity.Models;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;

namespace UniversityPilot.DAL.Areas.UniversityComponents.Models
{
    public class Student : User
    {
        public Student() : base()
        {
            CourseGroups = new HashSet<CourseGroup>();
        }

        public int Indeks { get; set; }

        public virtual ICollection<CourseGroup> CourseGroups { get; set; }
    }
}