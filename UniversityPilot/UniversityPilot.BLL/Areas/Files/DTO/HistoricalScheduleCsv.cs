using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class HistoricalScheduleCsv
    {
        [CsvColumn(0, "Data")]
        public DateTime Date { get; set; }

        [CsvColumn(1, "Dzień tygodnia")]
        public string DayOfWeek { get; set; }

        [CsvColumn(2, "Godzina rozpoczęcia")]
        public TimeSpan StartTime { get; set; }

        [CsvColumn(3, "Godzina zakończenia")]
        public TimeSpan EndTime { get; set; }

        [CsvColumn(4, "Sala, budynek")]
        public string RoomAndBuilding { get; set; }

        [CsvColumn(5, "Przedmiot")]
        public string CourseName { get; set; }

        [CsvColumn(6, "Rodzaj zajęć")]
        public string CourseType { get; set; }

        [CsvColumn(7, "Rodzaj studiów")]
        public string StudyType { get; set; }

        [CsvColumn(8, "Grupa")]
        public string Group { get; set; }

        [CsvColumn(9, "Kierunek")]
        public string FieldOfStudy { get; set; }

        [CsvColumn(10, "Wydział")]
        public string Faculty { get; set; }

        [CsvColumn(11, "Liczba godzin")]
        public int Hours { get; set; }

        [CsvColumn(12, "Zajęcia online")]
        public bool IsOnline { get; set; }

        [CsvColumn(13, "Uwagi")]
        public string Remarks { get; set; }

        [CsvColumn(14, "Prowadzący")]
        public int InstructorId { get; set; }
    }
}