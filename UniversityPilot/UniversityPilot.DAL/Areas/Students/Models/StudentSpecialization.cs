using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.Students.Models
{
    public class StudentSpecialization
    {
        public int StudentID { get; set; }
        public int SpecializationID { get; set; }

        public Student Student { get; set; }
        public Specialization Specialization { get; set; }
    }
}