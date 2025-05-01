using Microsoft.EntityFrameworkCore;
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