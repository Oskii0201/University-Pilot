using Microsoft.Extensions.Logging;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    internal class ScheduleGenerator : IScheduleGenerator
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassDayRepository _classDayRepository;
        private readonly IScheduleClassDayRepository _scheduleClassDayRepository;

        //private readonly ICourseScheduleRepository _courseScheduleRepository;
        private readonly IHolidayRepository _holidayRepository;

        private readonly ILogger<ScheduleGenerator> _logger;

        public ScheduleGenerator(
            ISemesterRepository semesterRepository,
            IClassDayRepository classDayRepository,
            IScheduleClassDayRepository scheduleClassDayRepository,
            //ICourseScheduleRepository courseScheduleRepository,
            IHolidayRepository holidayRepository,
            ILogger<ScheduleGenerator> logger)
        {
            _semesterRepository = semesterRepository;
            _classDayRepository = classDayRepository;
            _scheduleClassDayRepository = scheduleClassDayRepository;
            //_courseScheduleRepository = courseScheduleRepository;
            _holidayRepository = holidayRepository;
            _logger = logger;
        }

        public async Task GeneratePreliminaryScheduleAsync(int semesterId)
        {
            var semester = await _semesterRepository.GetAsync(semesterId);
            if (semester == null)
            {
                _logger.LogWarning("Semester with ID {SemesterId} not found.", semesterId);
                return;
            }

            await GenerateClassDaysAndScheduleClassDaysAsync(semester);
            await GeneratePreliminaryCourseSchedulesAsync(semester);
            await SetSemesterStageToGeneratingScheduleAsync(semester);

            _ = Task.Run(async () =>
            {
                try
                {
                    await GenerateWithAiAsync(semester.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during AI-based schedule generation");
                }
            });
        }

        private Task GenerateClassDaysAndScheduleClassDaysAsync(Semester semester)
        {
            throw new NotImplementedException();
        }

        private Task GeneratePreliminaryCourseSchedulesAsync(Semester semester)
        {
            throw new NotImplementedException();
        }

        private async Task SetSemesterStageToGeneratingScheduleAsync(Semester semester)
        {
            semester.CreationStage = ScheduleCreationStage.GeneratingSchedule;
            semester.UpdateDate = DateTime.UtcNow;
            await _semesterRepository.UpdateAsync(semester);
        }

        private Task GenerateWithAiAsync(int semesterId)
        {
            throw new NotImplementedException();
        }
    }
}