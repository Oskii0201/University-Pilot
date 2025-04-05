using System.ComponentModel;

namespace UniversityPilot.DAL.Areas.Shared.Enumes
{
    public enum FileType
    {
        [Description("Program studiów")]
        StudyProgram = 0,

        [Description("Sale lekcyjne/wykładowe")]
        Classrooms = 1,

        [Description("Dni dziekańskie, wolne i święta")]
        Holidays = 2,

        [Description("Prowadzący przedmiot")]
        Instructors = 3,
    }
}