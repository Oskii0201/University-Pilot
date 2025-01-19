using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.AcademicCalendar.Repositories
{
    internal class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}