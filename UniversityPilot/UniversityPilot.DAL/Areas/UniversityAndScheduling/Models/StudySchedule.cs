using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
{
    public class StudySchedule
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<FieldOfStudySchedule> FieldOfStudySchedules { get; set; }
    }
}