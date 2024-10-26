using UniversityPilot.DAL.Areas.UniversityAndScheduling.Models;

namespace UniversityPilot.DAL.Areas.Students.Models
{
    public class StudentGroup
    {
        public int StudentID { get; set; }
        public int GroupID { get; set; }

        public Student Student { get; set; }
        public CourseGroup CourseGroup { get; set; }
    }
}