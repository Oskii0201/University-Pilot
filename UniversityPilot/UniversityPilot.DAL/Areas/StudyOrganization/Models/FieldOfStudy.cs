using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Students.Models;
using UniversityPilot.DAL.Areas.UniversityAndScheduling.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class FieldOfStudy : IModelBase
    {
        public FieldOfStudy()
        {
            Specializations = new HashSet<Specialization>();
            Students = new HashSet<Student>();
            Courses = new HashSet<Course>();
            StudySchedules = new HashSet<StudySchedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FormOfStudy { get; set; }
        public string LevelOfEducation { get; set; }

        public virtual ICollection<Specialization> Specializations { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<StudySchedule> StudySchedules { get; set; }
    }
}