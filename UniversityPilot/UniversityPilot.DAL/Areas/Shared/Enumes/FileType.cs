using System.ComponentModel;

namespace UniversityPilot.DAL.Areas.Shared.Enumes
{
    public enum FileType
    {
        [Description("Program studiów")]
        StudyProgram = 0,

        [Description("Sale lekcyjne/wykładowe")]
        Rooms = 1,
    }
}