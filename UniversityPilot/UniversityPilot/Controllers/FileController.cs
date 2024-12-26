using Microsoft.AspNetCore.Mvc;
using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Files.Interfaces;

namespace UniversityPilot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ICsvService _csvService;

        public FileController(ICsvService csvService)
        {
            _csvService = csvService;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromBody] UploadDatasetDto data)
        {
            var result = await _csvService.UploadCsvAsync(data);

            if (result.IsSuccess)
                return Ok(new { message = result.Message });

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }
    }
}