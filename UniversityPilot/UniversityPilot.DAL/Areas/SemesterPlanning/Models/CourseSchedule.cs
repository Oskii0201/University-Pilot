using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Models
{
    public class CourseSchedule : IModelBase
    {
        public CourseSchedule()
        {
            CoursesDetails = new HashSet<CourseDetails>();
            CoursesGroups = new HashSet<CourseGroup>();
        }

        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Status { get; set; }

        public int? ClassroomId { get; set; }
        public virtual Classroom? Classroom { get; set; }

        public int? InstructorId { get; set; }
        public virtual Instructor? Instructor { get; set; }

        public virtual ICollection<CourseDetails> CoursesDetails { get; set; }
        public virtual ICollection<CourseGroup> CoursesGroups { get; set; }
    }
}