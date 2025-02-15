namespace UniversityPilot.BLL.Areas.Schedule.Models
{
    public class FieldsOfStudyAssignmentDto
    {
        public List<string> UnassignedFieldsOfStudy { get; set; } = new();
        public List<FieldOfStudyGroupDto> AssignedFieldOfStudyGroups { get; set; } = new();
    }
}