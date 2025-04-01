using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Processing.Interfaces
{
    internal interface IClassroomService
    {
        Task<Result> SaveFromCsv(List<ClassroomCsv> csvData);
    }
}