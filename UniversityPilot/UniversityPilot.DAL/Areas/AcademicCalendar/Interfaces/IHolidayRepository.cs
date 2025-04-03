using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces
{
    public interface IHolidayRepository : IRepository<Holiday>
    {
        Task<List<Holiday>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}