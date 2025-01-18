using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class StudyProgram : IModelBase
    {
        public StudyProgram()
        {
            Semesters = new HashSet<Semester>();
            Courses = new HashSet<Course>();
            ScheduleClassDays = new HashSet<ScheduleClassDay>();
        }

        public int Id { get; set; }

        public string EnrollmentYear { get; set; }
        public string StudyDegree { get; set; }

        public StudyForms StudyForm { get; set; }
        public bool SummerRecruitment { get; set; }

        public int FieldOfStudyId { get; set; }
        public virtual FieldOfStudy FieldOfStudy { get; set; }

        public virtual ICollection<Semester> Semesters { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<ScheduleClassDay> ScheduleClassDays { get; set; }
    }
}