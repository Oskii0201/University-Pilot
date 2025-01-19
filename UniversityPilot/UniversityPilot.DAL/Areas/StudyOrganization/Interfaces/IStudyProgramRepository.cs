using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Interfaces
{
    public interface IStudyProgramRepository : IRepository<StudyProgram>
    {
        StudyProgram? GetExistingStudyProgramWithIncludes(StudyProgram studyProgram);
    }
}