using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<List<Course>> GetBySemesterIdAsync(int semesterId);

        Task<Dictionary<(int ProgramId, int SemesterNumber), List<CourseGroup>>> GetCourseGroupsByProgramAndSemesterAsync(List<(int ProgramId, int SemesterNumber)> keys);

        Task<List<StudyProgram>> GetStudyProgramsBySemesterIdAsync(int semesterId);
    }
}