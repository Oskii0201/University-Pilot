using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class CourseDetails : IModelBase
    {
        public CourseDetails()
        {
            Instructors = new HashSet<Instructor>();
            CourseGroups = new HashSet<CourseGroup>();
            CourseSchedules = new HashSet<CourseSchedule>();
        }

        public int Id { get; set; }

        public CourseTypes CourseType { get; set; }
        public int Hours { get; set; }
        public string AssessmentType { get; set; }
        public int ECTS { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public int? SharedCourseGroupId { get; set; }
        public virtual SharedCourseGroup SharedCourseGroup { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<CourseGroup> CourseGroups { get; set; }
        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
    }
}