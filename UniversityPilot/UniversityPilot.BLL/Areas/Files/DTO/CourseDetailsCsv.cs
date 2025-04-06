using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class CourseDetailsCsv
    {
        [CsvColumn(0, "CourseDetailsId")]
        public int CourseDetailsId { get; set; }

        [CsvColumn(1, "StudyProgramDescription")]
        public string StudyProgramDescription { get; set; }

        [CsvColumn(2, "Specialization")]
        public string Specialization { get; set; }

        [CsvColumn(3, "CourseName")]
        public string CourseName { get; set; }

        [CsvColumn(4, "CourseType")]
        public string CourseType { get; set; }

        [CsvColumn(5, "CourseGroups")]
        public string CourseGroups { get; set; }

        [CsvColumn(6, "GroupsName")]
        public string GroupsName { get; set; }

        [CsvColumn(7, "SharedCourseGroup")]
        public string SharedCourseGroup { get; set; }

        [CsvColumn(8, "Instructors")]
        public string Instructors { get; set; }

        [CsvColumn(9, "InstructorsTitle")]
        public string InstructorsTitle { get; set; }

        [CsvColumn(10, "InstructorsFirstName")]
        public string InstructorsFirstName { get; set; }

        [CsvColumn(11, "InstructorsLastName")]
        public string InstructorsLastName { get; set; }
    }
}