namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class CourseFieldOfStudy
    {
        public int CourseID { get; set; }
        public int FieldOfStudyID { get; set; }

        public Course Course { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
    }
}