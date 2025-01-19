using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Repositories
{
    internal class StudyProgramRepository : Repository<StudyProgram>, IStudyProgramRepository
    {
        public StudyProgramRepository(UniversityPilotContext context) : base(context)
        {
        }

        public StudyProgram? GetExistingStudyProgramWithCourses(StudyProgram studyProgram)
        {
            return _context.Set<StudyProgram>()
                .Include(s => s.Courses)
                    .ThenInclude(c => c.Specialization)
                .Include(s => s.Courses)
                    .ThenInclude(c => c.CoursesDetails)
                .FirstOrDefault(sp =>
                           sp.EnrollmentYear == studyProgram.EnrollmentYear &&
                           sp.StudyDegree == studyProgram.StudyDegree &&
                           sp.StudyForm == studyProgram.StudyForm &&
                           sp.FieldOfStudyId == studyProgram.FieldOfStudyId &&
                           sp.SummerRecruitment == studyProgram.SummerRecruitment);
        }
    }
}