using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Models
{
    public class CourseGroup : IModelBase
    {
        public CourseGroup()
        {
            CourseDetails = new HashSet<CourseDetails>();
            Students = new HashSet<Student>();
            CourseSchedules = new HashSet<CourseSchedule>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public CourseTypes CourseType { get; set; }

        public virtual ICollection<CourseDetails> CourseDetails { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
    }
}