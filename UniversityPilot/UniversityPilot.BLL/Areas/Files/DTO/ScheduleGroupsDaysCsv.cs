using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class ScheduleGroupsDaysCsv
    {
        [CsvColumn(0, "ScheduleGroupId")]
        public int ScheduleGroupId { get; set; }

        [CsvColumn(1, "ScheduleGroupName")]
        public string ScheduleGroupName { get; set; }

        [CsvColumn(2, "DateTimeStart")]
        public string DateTimeStart { get; set; }

        [CsvColumn(3, "DateTimeEnd")]
        public string DateTimeEnd { get; set; }
    }
}