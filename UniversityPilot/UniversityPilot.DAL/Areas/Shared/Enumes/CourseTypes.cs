using System.ComponentModel;

namespace UniversityPilot.DAL.Areas.Shared.Enumes
{
    public enum CourseTypes
    {
        [Description("Wykład")]
        Lecture = 0,

        [Description("Ćwiczenia")]
        Exercises = 1,

        [Description("Konwersatorium")]
        Seminar = 2,

        [Description("Laboratorium")]
        Laboratory = 3,

        [Description("Zajęcia WF")]
        PEClasses = 4,

        [Description("Praktyka zawodowa")]
        Internship = 5,

        [Description("Nieznany")]
        Unknown = 6
    }
}