namespace UniversityPilot.BLL.Areas.Schedule.Models
{
    public class StudyProgramWithSemestersDto
    {
        public string Name { get; set; }
        public List<int> Semesters { get; set; }
    }
}