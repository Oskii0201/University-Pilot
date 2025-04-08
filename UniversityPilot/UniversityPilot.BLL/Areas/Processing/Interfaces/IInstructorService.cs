using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Processing.Services
{
    internal interface IInstructorService
    {
        Task<Result> SaveFromCsv(List<InstructorCsv> instructors);
    }
}