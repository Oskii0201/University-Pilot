using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
{
    public class CourseSchedule : IModelBase
    {
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Status { get; set; }

        public int CourseGroupId { get; set; }
        public virtual CourseGroup CourseGroup { get; set; }

        public int? ClassroomId { get; set; }
        public virtual Classroom? Classroom { get; set; }

        public int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}