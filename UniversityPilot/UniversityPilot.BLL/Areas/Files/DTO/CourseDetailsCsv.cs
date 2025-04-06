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

        [CsvColumn(4, "CourseGroups")]
        public string CourseGroups { get; set; }

        [CsvColumn(5, "GroupsName")]
        public string GroupsName { get; set; }

        [CsvColumn(6, "SharedCourseGroup")]
        public string SharedCourseGroup { get; set; }

        [CsvColumn(7, "Instructors")]
        public string Instructors { get; set; }

        [CsvColumn(8, "InstructorsTitle")]
        public string InstructorsTitle { get; set; }

        [CsvColumn(9, "InstructorsFirstName")]
        public string InstructorsFirstName { get; set; }

        [CsvColumn(10, "InstructorsLastName")]
        public string InstructorsLastName { get; set; }
    }
}