using UniversityPilot.DAL.Areas.Identity.Models;

namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
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