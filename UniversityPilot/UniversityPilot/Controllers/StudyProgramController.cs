using Microsoft.AspNetCore.Mvc;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Schedule.Models;

namespace UniversityPilot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyProgramController : ControllerBase
    {
        private readonly IGroupsScheduleService _groupsScheduleService;

        public StudyProgramController(
            IGroupsScheduleService groupsScheduleService)
        {
            _groupsScheduleService = groupsScheduleService;
        }

        [HttpGet]
        [Route("GetUpcomingSemesters")]
        public async Task<IActionResult> GetUpcomingSemesters([FromQuery] int count = 3)
        {
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0.");
            }

            return Ok(await _groupsScheduleService.GetUpcomingSemestersAsync(count));
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
    }
}