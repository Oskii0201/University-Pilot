namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
{
    public class CourseSchedule
    {
        public int ID { get; set; }
        public int CourseGroupID { get; set; }
        public int? ClassroomID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Status { get; set; }

        public CourseGroup CourseGroup { get; set; }
        public Classroom Classroom { get; set; }
    }
}