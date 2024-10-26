namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class CourseSpecialization
    {
        public int CourseID { get; set; }
        public int SpecializationID { get; set; }

        public Course Course { get; set; }
        public Specialization Specialization { get; set; }
    }
}