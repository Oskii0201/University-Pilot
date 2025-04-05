using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class InstructorCsv
    {
        [CsvColumn(1, "Tytuł")]
        public string Title { get; set; }

        [CsvColumn(2, "Imię")]
        public string FirstName { get; set; }

        [CsvColumn(3, "Nazwisko")]
        public string LastName { get; set; }
    }
}