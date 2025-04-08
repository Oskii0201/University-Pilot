using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class ClassroomCsv
    {
        [CsvColumn(0, "ClassroomId")]
        public int ClassroomId { get; set; }

        [CsvColumn(1, "Number")]
        public string Number { get; set; }
    }
}