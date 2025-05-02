using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Repositories
{
    internal class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(UniversityPilotContext context) : base(context)
        {
        }

        public async Task<List<StudyProgram>> GetStudyProgramsBySemesterIdAsync(int semesterId)
        {
            return await _context.Courses
                .Where(c => c.SemesterId == semesterId)
                .Select(c => c.StudyProgram)
                .Distinct()
                .Include(sp => sp.FieldOfStudy)
                .ToListAsync();
        }

        public async Task<Dictionary<(int ProgramId, int SemesterNumber), List<CourseGroup>>> GetCourseGroupsByProgramAndSemesterAsync(List<(int ProgramId, int SemesterNumber)> keys)
        {
            var programIds = keys.Select(k => k.ProgramId).Distinct().ToList();
            var semesterNumbers = keys.Select(k => k.SemesterNumber).Distinct().ToList();

            var courses = await _context.Courses
                .Where(c => programIds.Contains(c.StudyProgramId) && semesterNumbers.Contains(c.SemesterNumber))
                .Include(c => c.CoursesDetails)
                    .ThenInclude(cd => cd.CourseGroups)
                .ToListAsync();

            return keys.ToDictionary(
                key => key,
                key => courses
                    .Where(c => c.StudyProgramId == key.ProgramId && c.SemesterNumber == key.SemesterNumber)
                    .SelectMany(c => c.CoursesDetails)
                    .SelectMany(cd => cd.CourseGroups)
                    .DistinctBy(g => g.Id)
                    .ToList()
            );
        }

        public async Task<List<Course>> GetBySemesterIdAsync(int semesterId)
        {
            return await _context.Courses
                .Where(c => c.SemesterId == semesterId)
                .Include(c => c.StudyProgram)
                    .ThenInclude(sp => sp.FieldOfStudy)
                .ToListAsync();
        }
    }
}