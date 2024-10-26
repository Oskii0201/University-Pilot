namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
{
    public class Classroom
    {
        public int ID { get; set; }
        public string RoomNumber { get; set; }
        public int Floor { get; set; }
        public int SeatCount { get; set; }
        public int ClassroomCategoryID { get; set; }

        public ICollection<CourseGroup> CourseGroups { get; set; }
    }
}