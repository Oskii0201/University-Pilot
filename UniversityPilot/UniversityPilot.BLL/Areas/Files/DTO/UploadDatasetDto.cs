using Microsoft.AspNetCore.Http;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class UploadDatasetDto
    {
        public FileType Dataset { get; set; }
        public IFormFile File { get; set; }

        public UploadDatasetDto(FileType dataset, IFormFile file)
        {
            Dataset = dataset;
            File = file;
        }
    }
}