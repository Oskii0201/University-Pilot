using AutoMapper;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    internal class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;

        public SemesterService(
            ISemesterRepository semesterRepository,
            IMapper mapper)
        {
            _semesterRepository = semesterRepository;
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

        public async Task<string> GetStatusBySemesterIdAsync(int semesterId)
        {
            var semester = await _semesterRepository.GetAsync(semesterId);
            return semester?.CreationStage.ToString();
        }
    }
}