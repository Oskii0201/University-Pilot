using Microsoft.AspNetCore.Mvc;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _semesterStatusService;

        public SemesterController(ISemesterService semesterStatusService)
        {
            _semesterStatusService = semesterStatusService;
        }

        [HttpGet]
        [Route("GetUpcomingSemesters")]
        public async Task<IActionResult> GetUpcomingSemesters([FromQuery] int count = 3, [FromQuery] int status = 0)
        {
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0.");
            }

            return Ok(await _semesterStatusService.GetUpcomingSemestersAsync(count, status));
        }

        [HttpGet("GetSemestersWithStatusAfterGroupSchedule")]
        public async Task<IActionResult> GetSemestersWithStatusAfterGroupSchedule()
        {
            var result = await _semesterStatusService.GetSemestersWithStatusAfterGroupScheduleAsync();
            return Ok(result);
        }

        [HttpGet("GetSemestersByStatus/{status}")]
        public async Task<IActionResult> GetSemestersByStatus(ScheduleCreationStage status)
        {
            var result = await _semesterStatusService.GetByStatusAsync(status);
            return Ok(result);
        }

        [HttpGet("{semesterId}/Status")]
        public async Task<IActionResult> GetSemesterStatus(int semesterId)
        {
            var status = await _semesterStatusService.GetStatusBySemesterIdAsync(semesterId);

            if (status == null)
                return NotFound($"Semester with ID {semesterId} was not found.");

            return Ok(status);
        }
    }
}