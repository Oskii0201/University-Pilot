using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.DTO
{
    public class StudyProgramCsv
    {
        // ==FieldOfStudy==

        [CsvColumn(1, "Kierunek")]
        public string FieldOfStudy { get; set; }

        // ==StudyProgram==
        [CsvColumn(0, "Rok immatrykulacji")]
        public string EnrollmentYear { get; set; }

        [CsvColumn(2, "Forma")]
        public string StudyForm { get; set; }

        // ==Course==
        [CsvColumn(4, "Nazwa przedmiotu")]
        public string CourseName { get; set; }

        [CsvColumn(5, "Semestr")]
        public int SemesterNumber { get; set; }

        // ==Specialization==
        [CsvColumn(3, "Specjalność")]
        public string Specialization { get; set; }

        // ==CourseDetails==
        [CsvColumn(6, "Rodzaj zajęć")]
        public string CourseType { get; set; }

        [CsvColumn(7, "Liczba godzin")]
        public int Hours { get; set; }

        [CsvColumn(8, "Forma zaliczenia")]
        public string AssessmentType { get; set; }

        [CsvColumn(9, "ECTS")]
        public int ECTS { get; set; }
    }
}