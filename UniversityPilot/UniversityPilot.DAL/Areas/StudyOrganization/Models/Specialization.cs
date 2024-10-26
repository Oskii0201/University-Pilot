using UniversityPilot.DAL.Areas.Students.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Specialization
    {
        public int ID { get; set; }
        public int FieldOfStudyID { get; set; }
        public string Name { get; set; }

        public FieldOfStudy FieldOfStudy { get; set; }
        public ICollection<StudentSpecialization> StudentSpecializations { get; set; }
        public ICollection<CourseSpecialization> CourseSpecializations { get; set; }
    }
}