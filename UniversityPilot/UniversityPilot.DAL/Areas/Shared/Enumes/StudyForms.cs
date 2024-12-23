using System.ComponentModel;

namespace UniversityPilot.DAL.Areas.Shared.Enumes
{
    public enum StudyForms
    {
        [Description("Stacjonarne")]
        FullTime = 0,

        [Description("Niestacjonarne grupa weekendowa")]
        PartTimeWeekend = 1,

        [Description("Niestacjonarne grupa weekendowa on line")]
        PartTimeWeekendOnline = 2,

        [Description("Stacjonarne dualne")]
        FullTimeDual = 3,

        [Description("Nieznany")]
        Unknown = 4
    }
}