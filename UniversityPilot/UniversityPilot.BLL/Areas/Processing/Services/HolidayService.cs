using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;

namespace UniversityPilot.BLL.Areas.Processing.Services
{
    internal class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;

        public HolidayService(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public async Task<Result> SaveFromCsv(List<HolidaysCsv> csvHolidays)
        {
            var holidays = csvHolidays.Select(h => new Holiday
            {
                Name = h.Name,
                Date = h.Date,
                Description = h.Description
            }).ToList();

            await _holidayRepository.AddRangeAsync(holidays);

            return Result.Success($"{holidays.Count} holidays imported successfully.");
        }
    }
}