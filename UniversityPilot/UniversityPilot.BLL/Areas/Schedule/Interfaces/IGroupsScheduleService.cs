using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Schedule.Models;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Schedule.Interfaces
{
    public interface IGroupsScheduleService
    {
        public Task<FieldsOfStudyAssignmentDto> GetFieldsOfStudyAssignmentsToGroupAsync(int semesterId);

        public Task UpdateFieldsOfStudyAssignmentsToGroupAsync(FieldsOfStudyAssignmentDto model);

        public Task<WeekendAvailabilityDto> GetWeekendAvailabilityAsync(int semesterId);

        public Task SaveWeekendAvailabilityAsync(WeekendAvailabilityDto model);

        public Task<Result> AcceptWeekendAvailabilityAsync(int semesterId);

        public Task<List<ScheduleGroupsDaysCsv>> GetScheduleGroupsDaysCsvAsync(int semesterId);
    }
}