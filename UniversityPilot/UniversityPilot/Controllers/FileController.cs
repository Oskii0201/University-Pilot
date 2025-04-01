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
        private readonly IFileService _fileService;

        public FileController(
            ICsvService csvService,
            IFileService fileService)
        {
            _csvService = csvService;
            _fileService = fileService;
        }

        [HttpGet("GetFileTypes")]
        public IActionResult GetFileTypes()
        {
            var result = _fileService.GetFileTypeDictionary();
            return Ok(result);
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] string dataset, IFormFile file)
        {
            var result = await _csvService.UploadAsync(new UploadDatasetDto(dataset, file));

            if (result.IsSuccess)
                return Ok(new { message = result.Message });

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }
    }
}