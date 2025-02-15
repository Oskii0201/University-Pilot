namespace UniversityPilot.BLL.Areas.Schedule.Models
{
    public class FieldOfStudyGroupDto
    {
        public int IdGroup { get; set; }
        public string GroupName { get; set; }
        public List<string> AssignedFieldsOfStudy { get; set; } = new();
    }
}