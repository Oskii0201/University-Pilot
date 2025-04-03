using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class ClassroomCsv
    {
        [CsvColumn(0, "Nazwa sali")]
        public string Name { get; set; }
    }
}