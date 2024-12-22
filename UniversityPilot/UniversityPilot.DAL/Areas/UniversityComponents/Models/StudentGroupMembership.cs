using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.UniversityComponents.Models
{
    public class StudentGroupMembership : IModelBase
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int CourseGroupId { get; set; }
        public virtual CourseGroup CourseGroup { get; set; }
    }
}