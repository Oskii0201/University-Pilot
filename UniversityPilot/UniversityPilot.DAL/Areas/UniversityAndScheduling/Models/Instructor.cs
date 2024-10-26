using UniversityPilot.DAL.Areas.Identity.Models;

namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
{
    public class Instructor
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string ContractType { get; set; }

        public User User { get; set; }
        public ICollection<CourseGroup> CourseGroups { get; set; }
    }
}