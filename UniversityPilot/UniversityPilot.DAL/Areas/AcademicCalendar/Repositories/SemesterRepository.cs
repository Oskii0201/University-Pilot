using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.AcademicCalendar.Repositories
{
    internal class SemesterRepository : Repository<Semester>, ISemesterRepository
    {
        public SemesterRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}