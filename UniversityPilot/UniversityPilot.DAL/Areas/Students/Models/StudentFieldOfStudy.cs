using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.Students.Models
{
    public class StudentFieldOfStudy
    {
        public int StudentID { get; set; }
        public int FieldOfStudyID { get; set; }

        public Student Student { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
    }
}