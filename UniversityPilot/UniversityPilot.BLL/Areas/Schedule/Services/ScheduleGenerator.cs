﻿using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    internal class ScheduleGenerator : IScheduleGenerator
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassDayRepository _classDayRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IScheduleClassDayRepository _scheduleClassDayRepository;
        private readonly ICourseScheduleRepository _courseScheduleRepository;
        private readonly IHolidayRepository _holidayRepository;

        public ScheduleGenerator(
            ISemesterRepository semesterRepository,
            IClassDayRepository classDayRepository,
            ICourseRepository courseRepository,
            IScheduleClassDayRepository scheduleClassDayRepository,
            ICourseScheduleRepository courseScheduleRepository,
            IHolidayRepository holidayRepository)
        {
            _semesterRepository = semesterRepository;
            _classDayRepository = classDayRepository;
            _courseRepository = courseRepository;
            _scheduleClassDayRepository = scheduleClassDayRepository;
            _courseScheduleRepository = courseScheduleRepository;
            _holidayRepository = holidayRepository;
        }

        public async Task GeneratePreliminaryScheduleAsync(int semesterId)
        {
            var semester = await _semesterRepository.GetAsync(semesterId);
            if (semester == null)
            {
                //_logger.LogWarning("Semester with ID {SemesterId} not found.", semesterId);
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
                    //_logger.LogError(ex, "Error during AI-based schedule generation");
                }
            });
        }

        private async Task GenerateClassDaysAndScheduleClassDaysAsync(Semester semester)
        {
            var existingClassDays = await _classDayRepository.GetBySemesterDatesAsync(semester.StartDate, semester.EndDate);
            var holidays = await _holidayRepository.GetByDateRangeAsync(semester.StartDate, semester.EndDate);
            var holidayDates = holidays.Select(h => h.Date.Date).ToHashSet();

            var allPrograms = await _courseRepository.GetStudyProgramsBySemesterIdAsync(semester.Id);
            var fullTimePrograms = allPrograms
                .Where(sp => sp.StudyForm != StudyForms.PartTimeWeekend &&
                             sp.StudyForm != StudyForms.PartTimeWeekendOnline)
                .ToList();

            if (!fullTimePrograms.Any())
                return;

            var scheduleClassDay = new ScheduleClassDay
            {
                Title = "Stacjonarna grupa zjazdowa",
                SemesterId = semester.Id
            };

            await _scheduleClassDayRepository.AddAsync(scheduleClassDay);
            await _scheduleClassDayRepository.UpdateAssignmentsAsync(scheduleClassDay.Id, fullTimePrograms.Select(p => p.Id).ToList());

            foreach (var date in Utilities.EachDay(semester.StartDate.Date, semester.EndDate.Date))
            {
                if (holidayDates.Contains(date))
                    continue;

                if (date.DayOfWeek < DayOfWeek.Monday || date.DayOfWeek > DayOfWeek.Friday)
                    continue;

                var classStart = DateTime.SpecifyKind(date.AddHours(7), DateTimeKind.Utc);
                var classEnd = DateTime.SpecifyKind(date.AddHours(22), DateTimeKind.Utc);

                var classDay = existingClassDays.FirstOrDefault(cd =>
                    cd.StartDateTime == classStart &&
                    cd.EndDateTime == classEnd);

                if (classDay == null)
                {
                    classDay = new ClassDay
                    {
                        StartDateTime = classStart,
                        EndDateTime = classEnd
                    };

                    await _classDayRepository.AddAsync(classDay);
                    existingClassDays.Add(classDay);
                }

                await _classDayRepository.AssignToScheduleClassDayAsync(classDay.Id, scheduleClassDay.Id);
            }
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