using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.UniversityComponents.Models
{
    public class Classroom : IModelBase
    {
        public Classroom()
        {
            CourseSchedules = new HashSet<CourseSchedule>();
        }

        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int Floor { get; set; }
        public int SeatCount { get; set; }

        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
    }
}