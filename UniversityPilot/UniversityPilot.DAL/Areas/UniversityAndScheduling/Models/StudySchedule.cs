using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
{
    public class StudySchedule : IModelBase
    {
        public StudySchedule()
        {
            FieldOfStudies = new HashSet<FieldOfStudy>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<FieldOfStudy> FieldOfStudies { get; set; }
    }
}