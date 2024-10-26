using UniversityPilot.DAL.Areas.UniversityAndScheduling.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class FieldOfStudySchedule
    {
        public int FieldOfStudyID { get; set; }
        public int StudyScheduleID { get; set; }

        public FieldOfStudy FieldOfStudy { get; set; }
        public StudySchedule StudySchedule { get; set; }
    }
}