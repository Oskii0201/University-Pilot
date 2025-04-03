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

        public async Task<Result> SaveFromCsv(List<HolidaysCsv> csvRows)
        {
            var existingHolidays = await _holidayRepository.GetAllAsync();
            var newHolidays = csvRows
                .Where(h => !existingHolidays.Any(e =>
                    e.Date.Date == h.Date.Date &&
                    string.Equals(e.Name.Trim(), h.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
                .Select(h => new Holiday
                {
                    Name = h.Name.Trim(),
                    Date = h.Date.Date,
                    Description = string.IsNullOrWhiteSpace(h.Description) ? null : h.Description.Trim()
                })
                .ToList();

            if (!newHolidays.Any())
                return Result.Success("No new holidays to import – all records already exist.");

            await _holidayRepository.AddRangeAsync(newHolidays);

            return Result.Success($"{newHolidays.Count} holidays imported successfully.");
        }
    }
}