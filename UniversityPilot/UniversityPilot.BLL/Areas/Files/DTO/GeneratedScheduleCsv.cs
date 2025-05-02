using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class GeneratedScheduleCsv
    {
        [CsvColumn(0, "CourseScheduleId")]
        public int CourseScheduleId { get; set; }

        [CsvColumn(1, "ClassroomId")]
        public string ClassroomId { get; set; }

        [CsvColumn(2, "NewDateTimeStart")]
        public DateTime NewDateTimeStart { get; set; }

        [CsvColumn(3, "NewDateTimeEnd")]
        public DateTime NewDateTimeEnd { get; set; }
    }
}