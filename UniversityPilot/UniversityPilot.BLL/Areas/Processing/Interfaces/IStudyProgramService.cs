using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Processing.Interfaces
{
    public interface IStudyProgramService
    {
        Result SaveFromCsv(List<StudyProgramCsv> studyProgramsCsv);
    }
}