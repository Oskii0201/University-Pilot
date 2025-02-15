using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class FieldOfStudy : IModelBase
    {
        public FieldOfStudy()
        {
            Programs = new HashSet<StudyProgram>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StudyProgram> Programs { get; set; }
    }
}