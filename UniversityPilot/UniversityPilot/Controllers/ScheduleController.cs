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

        public ScheduleController(
            IGroupsScheduleService groupsScheduleService)
        {
            _groupsScheduleService = groupsScheduleService;
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

        [HttpPost("SaveWeekendAvailability")]
        public async Task<IActionResult> SaveWeekendAvailability([FromBody] WeekendAvailabilityDto model)
        {
            await _groupsScheduleService.SaveWeekendAvailabilityAsync(model);
            return Ok();
        }

        [HttpPost("AcceptWeekendAvailability")]
        public async Task<IActionResult> AcceptWeekendAvailability([FromQuery] int semesterId)
        {
            //var success = await _groupsScheduleService.AcceptWeekendAvailability(semesterId);

            //if (!success)
            //    return NotFound($"Semester with ID {semesterId} not found.");

            return Ok("Weekend availability accepted and status updated.");
        }
    }
}