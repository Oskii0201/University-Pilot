using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Processing.Interfaces
{
    internal interface ICourseDetailsService
    {
        public Task<Result> UpdateFromCsv(List<CourseDetailsCsv> courseDetailsCsv);
    }
}