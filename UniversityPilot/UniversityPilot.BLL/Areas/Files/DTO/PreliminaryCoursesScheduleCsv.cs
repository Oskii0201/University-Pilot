using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class PreliminaryCoursesScheduleCsv
    {
        [CsvColumn(0, "CourseScheduleId")]
        public int CourseScheduleId { get; set; }

        [CsvColumn(1, "CourseName")]
        public string CourseName { get; set; }

        [CsvColumn(2, "CourseDetailsId")]
        public int CourseDetailsId { get; set; }

        [CsvColumn(3, "CourseType")]
        public string CourseType { get; set; }

        [CsvColumn(4, "Online")]
        public string Online { get; set; }

        [CsvColumn(5, "GroupId")]
        public int GroupId { get; set; }

        [CsvColumn(6, "GroupName")]
        public string GroupName { get; set; }

        [CsvColumn(7, "DependentGroupsIds")]
        public string DependentGroupsIds { get; set; }

        [CsvColumn(8, "DependentGroupsNames")]
        public string DependentGroupsNames { get; set; }

        [CsvColumn(9, "ScheduleGroupId")]
        public int ScheduleGroupId { get; set; }

        [CsvColumn(10, "ScheduleGroupName")]
        public string ScheduleGroupName { get; set; }

        [CsvColumn(11, "InstructorId")]
        public int InstructorId { get; set; }

        [CsvColumn(12, "DateTimeStart")]
        public string DateTimeStart { get; set; }

        [CsvColumn(13, "DateTimeEnd")]
        public string DateTimeEnd { get; set; }

        [CsvColumn(14, "ClassroomId")]
        public int ClassroomId { get; set; }
    }
}