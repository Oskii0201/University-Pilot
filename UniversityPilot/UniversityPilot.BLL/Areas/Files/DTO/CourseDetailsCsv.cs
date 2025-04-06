using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class CourseDetailsCsv
    {
        [CsvColumn(0, "CourseDetailsId")]
        public int CourseDetailsId { get; set; }

        [CsvColumn(1, "EnrollmentYear")]
        public string EnrollmentYear { get; set; }

        [CsvColumn(2, "SemesterNumber")]
        public int SemesterNumber { get; set; }

        [CsvColumn(3, "FieldOfStudy")]
        public string FieldOfStudy { get; set; }

        [CsvColumn(4, "SummerRecruitment")]
        public string SummerRecruitment { get; set; }

        [CsvColumn(5, "StudyForm")]
        public string StudyForm { get; set; }

        [CsvColumn(6, "StudyDegree")]
        public string StudyDegree { get; set; }

        [CsvColumn(7, "Specialization")]
        public string Specialization { get; set; }

        [CsvColumn(8, "CourseName")]
        public string CourseName { get; set; }

        [CsvColumn(9, "CourseType")]
        public string CourseType { get; set; }

        [CsvColumn(10, "TitleScheduleClassDay")]
        public string TitleScheduleClassDay { get; set; }

        [CsvColumn(11, "CourseGroups")]
        public string CourseGroups { get; set; }

        [CsvColumn(12, "GroupsName")]
        public string GroupsName { get; set; }

        [CsvColumn(13, "SharedCourseGroup")]
        public string SharedCourseGroup { get; set; }

        [CsvColumn(14, "Instructors")]
        public string Instructors { get; set; }

        [CsvColumn(15, "InstructorsTitle")]
        public string InstructorsTitle { get; set; }

        [CsvColumn(16, "InstructorsFirstName")]
        public string InstructorsFirstName { get; set; }

        [CsvColumn(17, "InstructorsLastName")]
        public string InstructorsLastName { get; set; }
    }
}