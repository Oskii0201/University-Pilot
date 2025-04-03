using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class HolidaysCsv
    {
        [CsvColumn(0, "Nazwa")]
        public string Name { get; set; }

        [CsvColumn(1, "Data")]
        public DateTime Date { get; set; }

        [CsvColumn(2, "Opis")]
        public string? Description { get; set; }
    }
}