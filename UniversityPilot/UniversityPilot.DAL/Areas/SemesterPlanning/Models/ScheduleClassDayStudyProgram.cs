using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Models
{
    public class ScheduleClassDayStudyProgram
    {
        public int ScheduleClassDayId { get; set; }
        public virtual ScheduleClassDay ScheduleClassDay { get; set; }

        public int StudyProgramId { get; set; }
        public virtual StudyProgram StudyProgram { get; set; }
    }
}