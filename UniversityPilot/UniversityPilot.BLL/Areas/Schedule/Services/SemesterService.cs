using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Schedule.Models;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.Shared.Utilities;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    internal class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly ICourseRepository _courseRepository;

        public SemesterService(
            ISemesterRepository semesterRepository,
            ICourseRepository courseRepository)
        {
            _semesterRepository = semesterRepository;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Semester>> GetUpcomingSemestersAsync(int count = 3, int status = 0)
        {
            return await _semesterRepository.GetUpcomingSemestersAsync(count, status);
        }

        public async Task<List<Semester>> GetSemestersWithStatusAfterGroupScheduleAsync()
        {
            var semesters = await _semesterRepository.GetSemestersWithStatusAfterGroupScheduleAsync();
            return semesters.ToList();
        }

        public async Task<List<Semester>> GetByStatusAsync(ScheduleCreationStage stage)
        {
            var semesters = await _semesterRepository.GetAllAsync();
            return semesters
                .Where(s => s.CreationStage == stage)
                .ToList();
        }

        public async Task<int> GetStatusBySemesterIdAsync(int semesterId)
        {
            var semester = await _semesterRepository.GetAsync(semesterId);
            return semester == null ? 0 : (int)semester.CreationStage;
        }

        public async Task<List<StudyProgramWithSemestersDto>> GetStudyProgramsWithSemestersAsync(int semesterId)
        {
            var courses = await _courseRepository.GetBySemesterIdAsync(semesterId);

            var result = courses
                .Where(c => c.StudyProgram != null)
                .Select(c => new
                {
                    Name = FormatStudyProgramFullName(c.StudyProgram),
                    SemesterNumber = c.SemesterNumber
                })
                .GroupBy(x => x.Name)
                .Select(group => new StudyProgramWithSemestersDto
                {
                    Name = group.Key,
                    Semesters = group.Select(x => x.SemesterNumber).Distinct().OrderBy(n => n).ToList()
                })
                .OrderBy(x => x.Name)
                .ToList();

            return result;
        }

        private static string FormatStudyProgramFullName(StudyProgram sp)
        {
            var baseName = sp.FieldOfStudy.Name;
            var degreeSuffix = sp.StudyDegree switch
            {
                StudyDegree.Inz or StudyDegree.Lic => " - I Stopień",
                StudyDegree.Mgr or StudyDegree.USM or StudyDegree.USM3Sem or StudyDegree.USMSp => " - II Stopień",
                _ => " - Nieznany Stopień"
            };
            var formDesc = EnumHelper.GetEnumDescription(sp.StudyForm);
            return $"{baseName}{degreeSuffix} - {formDesc}";
        }
    }
}