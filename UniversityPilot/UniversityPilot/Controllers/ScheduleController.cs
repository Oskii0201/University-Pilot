using Microsoft.AspNetCore.Mvc;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Schedule.Models;

namespace UniversityPilot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IGroupsScheduleService _groupsScheduleService;
        private readonly IScheduleService _scheduleService;

        public ScheduleController(
            IGroupsScheduleService groupsScheduleService,
            IScheduleService scheduleService)
        {
            _groupsScheduleService = groupsScheduleService;
            _scheduleService = scheduleService;
        }

        [HttpGet]
        [Route("GetFieldsOfStudyAssignmentsToGroup")]
        public async Task<IActionResult> GetFieldsOfStudyAssignmentsToGroupAsync(int semesterId)
        {
            return Ok(await _groupsScheduleService.GetFieldsOfStudyAssignmentsToGroupAsync(semesterId));
        }

        [HttpPut("UpdateFieldsOfStudyAssignmentsToGroup")]
        public async Task<IActionResult> UpdateFieldsOfStudyAssignmentsToGroup([FromBody] FieldsOfStudyAssignmentDto model)
        {
            await _groupsScheduleService.UpdateFieldsOfStudyAssignmentsToGroupAsync(model);
            return Ok("Fields of study assignments updated successfully.");
        }

        [HttpGet("GetWeekendAvailability")]
        public async Task<IActionResult> GetWeekendAvailability([FromQuery] int semesterId)
        {
            var result = await _groupsScheduleService.GetWeekendAvailabilityAsync(semesterId);
            return Ok(result);
        }

        [HttpPut("SaveWeekendAvailability")]
        public async Task<IActionResult> SaveWeekendAvailability([FromBody] WeekendAvailabilityDto model)
        {
            await _groupsScheduleService.SaveWeekendAvailabilityAsync(model);
            return Ok();
        }

        [HttpPost("AcceptWeekendAvailability")]
        public async Task<IActionResult> AcceptWeekendAvailability([FromQuery] int semesterId)
        {
            var result = await _groupsScheduleService.AcceptWeekendAvailabilityAsync(semesterId);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok("Weekend availability accepted and status updated.");
        }

        [HttpPost("GetCalendar")]
        public async Task<IActionResult> GetCalendar([FromBody] ScheduleRequestDto request)
        {
            var result = await _scheduleService.GetScheduleAsync(request);
            return Ok(result);
        }
    }
}