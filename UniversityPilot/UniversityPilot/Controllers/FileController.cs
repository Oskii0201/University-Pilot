using Microsoft.AspNetCore.Mvc;
using System.Text;
using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Files.Interfaces;
using UniversityPilot.DAL.Areas.Shared.Enumes;

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

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] string dataset, IFormFile file)
        {
            if (!Enum.TryParse<FileType>(dataset, out var parsedType))
                return BadRequest("Invalid file type");

            var result = await _csvService.UploadAsync(new UploadDatasetDto(parsedType, file));

            if (result.IsSuccess)
                return Ok(new { message = result.Message });

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        [HttpGet("ExportCourseDetails/{semesterId}")]
        public async Task<IActionResult> ExportCourseDetailsToCsv(int semesterId)
        {
            var csv = await _csvService.GetCourseDetailsExport(semesterId);
            return File(Encoding.UTF8.GetBytes(csv), "text/csv", $"CourseDetails_Semester_{semesterId}.csv");
        }
    }
}