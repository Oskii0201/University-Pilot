using UniversityPilot.DAL.Areas.Students.Models;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
{
    public class CourseGroup
    {
        public int ID { get; set; }
        public string GroupName { get; set; }
        public int CourseID { get; set; }
        public int InstructorID { get; set; }
        public int? PreferedClassroomID { get; set; }

        public Course Course { get; set; }
        public Instructor Instructor { get; set; }
        public ICollection<StudentGroup> StudentGroups { get; set; }
    }
}