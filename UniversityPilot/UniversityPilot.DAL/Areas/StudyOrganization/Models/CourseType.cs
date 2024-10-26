namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class CourseType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}