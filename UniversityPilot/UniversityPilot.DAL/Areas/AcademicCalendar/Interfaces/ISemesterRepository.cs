using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces
{
    public interface ISemesterRepository : IRepository<Semester>
    {
        Task<IEnumerable<Semester>> GetUpcomingSemestersAsync(int count = 3);
    }
}