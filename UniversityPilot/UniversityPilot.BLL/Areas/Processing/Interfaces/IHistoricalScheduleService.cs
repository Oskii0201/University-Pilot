using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Processing.Interfaces
{
    public interface IHistoricalScheduleService
    {
        Result SaveFromCsv(List<HistoricalScheduleCsv> historicalSchedules);
    }
}