namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class StudyProgram
    {
        public StudyProgram()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public int EnrollmentYear { get; set; }
        public string StudyType { get; set; }
        public string FieldOfStudy { get; set; }
        public string DegreeType { get; set; }
        public bool SummerRecruitment { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}