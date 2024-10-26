using UniversityPilot.DAL.Areas.Identity.Models;

namespace UniversityPilot.DAL.Areas.Students.Models
{
    public class Student
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int Indeks { get; set; }

        public User User { get; set; }
        public ICollection<StudentGroup> StudentGroups { get; set; }
        public ICollection<StudentFieldOfStudy> StudentFieldsOfStudy { get; set; }
        public ICollection<StudentSpecialization> StudentSpecializations { get; set; }
    }
}