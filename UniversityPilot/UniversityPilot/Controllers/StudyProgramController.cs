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
        public IActionResult UpdateFieldsOfStudyAssignmentsToGroup([FromBody] FieldsOfStudyAssignmentDto model)
        {
            // TODO: Do implementacji w serwisie (zapis zmian w bazie, walidacja itp.)
            return Ok("Fields of study assignments updated successfully.");
        }
    }
}