using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class StudyProgram : IModelBase
    {
        public StudyProgram()
        {
            Courses = new HashSet<Course>();
            Specializations = new HashSet<Specialization>();
        }

        public int Id { get; set; }

        public string AcademicYear { get; set; }
        public string StudyDegree { get; set; }
        public string FieldOfStudy { get; set; }
        public StudyForms StudyForm { get; set; }
        public bool SummerRecruitment { get; set; }

        public virtual ICollection<Specialization> Specializations { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}