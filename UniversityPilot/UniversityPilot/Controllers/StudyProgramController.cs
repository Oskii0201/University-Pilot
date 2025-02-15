using Microsoft.AspNetCore.Mvc;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;

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
    }
}