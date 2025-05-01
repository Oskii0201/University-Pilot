using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<List<Course>> GetBySemesterIdAsync(int semesterId);

        Task<List<StudyProgram>> GetStudyProgramsBySemesterIdAsync(int semesterId);
    }
}