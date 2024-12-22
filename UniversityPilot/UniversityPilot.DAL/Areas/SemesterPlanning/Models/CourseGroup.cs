using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Models
{
    public class CourseGroup : IModelBase
    {
        public CourseGroup()
        {
            Courses = new HashSet<Course>();
            Students = new HashSet<Student>();
            CourseSchedules = new HashSet<CourseSchedule>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
    }
}