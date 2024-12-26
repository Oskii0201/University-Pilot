using Microsoft.AspNetCore.Http;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class UploadDatasetDto
    {
        public string Dataset { get; set; }
        public IFormFile File { get; set; }
    }
}