using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Processing.Services
{
    public interface IInstructorService
    {
        Result SaveFromCsv(List<InstructorCsv> instructors);
    }
}