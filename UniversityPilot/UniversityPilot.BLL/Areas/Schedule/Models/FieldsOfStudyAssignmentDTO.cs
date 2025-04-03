namespace UniversityPilot.BLL.Areas.Schedule.Models
{
    public class FieldsOfStudyAssignmentDto
    {
        public int SemesterId { get; set; }
        public string? Name { get; set; }
        public List<string> UnassignedFieldsOfStudy { get; set; } = new();
        public List<FieldOfStudyGroupDto> AssignedFieldOfStudyGroups { get; set; } = new();
    }
}