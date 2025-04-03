using System.ComponentModel;

namespace UniversityPilot.DAL.Areas.Shared.Enumes
{
    public enum StudyDegree
    {
        [Description("inż.")]
        Inz = 0,

        [Description("Lic")]
        Lic = 1,

        [Description("mgr")]
        Mgr = 2,

        [Description("USM")]
        USM = 3,

        [Description("USM 3sem")]
        USM3Sem = 4,

        [Description("USM + SP")]
        USMSp = 5,

        [Description("Nieznany")]
        Unknown = 6
    }
}