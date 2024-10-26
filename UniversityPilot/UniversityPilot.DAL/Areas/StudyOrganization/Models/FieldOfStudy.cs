using UniversityPilot.DAL.Areas.Students.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class FieldOfStudy
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FormOfStudy { get; set; }
        public string LevelOfEducation { get; set; }

        public ICollection<Specialization> Specializations { get; set; }
        public ICollection<StudentFieldOfStudy> StudentFieldsOfStudy { get; set; }
        public ICollection<CourseFieldOfStudy> CourseFieldsOfStudy { get; set; }
        public ICollection<FieldOfStudySchedule> FieldOfStudySchedules { get; set; }
    }
}