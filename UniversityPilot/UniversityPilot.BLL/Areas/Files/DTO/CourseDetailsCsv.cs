using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class CourseDetailsCsv
    {
        [CsvColumn(0, "CourseDetailsId")]
        public int CourseDetailsId { get; set; }

        [CsvColumn(1, "StudyProgramDescription")]
        public string StudyProgramDescription { get; set; }

        [CsvColumn(2, "CourseName")]
        public string CourseName { get; set; }

        [CsvColumn(3, "CourseType")]
        public string CourseType { get; set; }

        [CsvColumn(4, "Instructors")]
        public string Instructors { get; set; }

        [CsvColumn(5, "CourseGroups")]
        public string CourseGroups { get; set; }

        [CsvColumn(6, "GroupsName")]
        public string GroupsName { get; set; }
    }
}