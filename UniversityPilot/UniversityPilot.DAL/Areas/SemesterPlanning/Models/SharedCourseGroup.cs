using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Models
{
    public class SharedCourseGroup : IModelBase
    {
        public SharedCourseGroup()
        {
            CoursesDetails = new HashSet<CourseDetails>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CourseDetails> CoursesDetails { get; set; }
    }
}