using UniversityPilot.BLL.Areas.Schedule.Models;

namespace UniversityPilot.BLL.Areas.Schedule.Interfaces
{
    public interface IGroupsScheduleService
    {
        Task<IEnumerable<SemesterDTO>> GetUpcomingSemestersAsync(int count = 3);

        Task<FieldsOfStudyAssignmentDto> GetFieldsOfStudyAssignmentsToGroupAsync(int semesterId);

        Task UpdateFieldsOfStudyAssignmentsToGroupAsync(FieldsOfStudyAssignmentDto model);
    }
}