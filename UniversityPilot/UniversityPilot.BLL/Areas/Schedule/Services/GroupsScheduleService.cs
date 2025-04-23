using Microsoft.Extensions.DependencyInjection;
using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Schedule.Models;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    public class GroupsScheduleService : IGroupsScheduleService
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IScheduleClassDayRepository _scheduleClassDayRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IClassDayRepository _classDayRepository;
        private readonly IHolidayRepository _holidayRepository;
        private readonly IServiceScopeFactory _scopeFactory;

        public GroupsScheduleService(
            ISemesterRepository semesterRepository,
            IScheduleClassDayRepository scheduleClassDayRepository,
            ICourseRepository courseRepository,
            IClassDayRepository classDayRepository,
            IHolidayRepository holidayRepository,
            IServiceScopeFactory scopeFactory)
        {
            _semesterRepository = semesterRepository;
            _scheduleClassDayRepository = scheduleClassDayRepository;
            _courseRepository = courseRepository;
            _classDayRepository = classDayRepository;
            _holidayRepository = holidayRepository;
            _scopeFactory = scopeFactory;
        }

        public async Task<FieldsOfStudyAssignmentDto> GetFieldsOfStudyAssignmentsToGroupAsync(int semesterId)
        {
            var scheduleClassDays = await _scheduleClassDayRepository.GetBySemesterIdAsync(semesterId);
            var semester = await _semesterRepository.GetAsync(semesterId);

            var assignedPrograms = scheduleClassDays
                .SelectMany(scd => scd.StudyPrograms)
                .Where(sp =>
                    sp.StudyForm == StudyForms.PartTimeWeekend ||
                    sp.StudyForm == StudyForms.PartTimeWeekendOnline)
                .DistinctBy(sp => sp.Id)
                .ToList();

            var allProgramsFromCourses = await _courseRepository.GetStudyProgramsBySemesterIdAsync(semesterId);

            var relevantPrograms = allProgramsFromCourses
                .Where(sp => sp.StudyForm == StudyForms.PartTimeWeekend || sp.StudyForm == StudyForms.PartTimeWeekendOnline)
                .ToList();

            var unassignedPrograms = relevantPrograms
                .Where(sp => !assignedPrograms.Any(ap => ap.Id == sp.Id))
                .ToList();

            var unassignedFieldsOfStudy = unassignedPrograms
                .Select(sp => new
                {
                    Program = sp,
                    Formatted = FormatFieldOfStudy(sp),
                    Rank = GetStudyDegreeRank(sp.StudyDegree),
                    NameOnly = sp.FieldOfStudy.Name
                })
                .DistinctBy(x => x.Formatted)
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.NameOnly)
                .Select(x => x.Formatted)
                .ToList();

            var assignedFieldOfStudyGroups = scheduleClassDays
                .Select(scd => new FieldOfStudyGroupDto
                {
                    GroupId = scd.Id,
                    GroupName = scd.Title,
                    AssignedFieldsOfStudy = scd.StudyPrograms
                        .Where(sp =>
                            sp.StudyForm == StudyForms.PartTimeWeekend ||
                            sp.StudyForm == StudyForms.PartTimeWeekendOnline)
                        .Select(sp => new
                        {
                            Program = sp,
                            Formatted = FormatFieldOfStudy(sp),
                            Rank = GetStudyDegreeRank(sp.StudyDegree),
                            NameOnly = sp.FieldOfStudy.Name
                        })
                        .DistinctBy(x => x.Formatted)
                        .OrderBy(x => x.Rank)
                        .ThenBy(x => x.NameOnly)
                        .Select(x => x.Formatted)
                        .ToList()
                })
                .ToList();

            return new FieldsOfStudyAssignmentDto
            {
                SemesterId = semesterId,
                Name = semester.Name,
                UnassignedFieldsOfStudy = unassignedFieldsOfStudy,
                AssignedFieldOfStudyGroups = assignedFieldOfStudyGroups
            };
        }

        public async Task UpdateFieldsOfStudyAssignmentsToGroupAsync(FieldsOfStudyAssignmentDto model)
        {
            var scheduleClassDaysInDb = await _scheduleClassDayRepository.GetBySemesterIdAsync(model.SemesterId);
            var relevantPrograms = await _courseRepository.GetStudyProgramsBySemesterIdAsync(model.SemesterId);

            var relevantProgramsFiltered = relevantPrograms
                .Where(sp => sp.StudyForm == StudyForms.PartTimeWeekend
                          || sp.StudyForm == StudyForms.PartTimeWeekendOnline)
                .ToList();

            var stringToPrograms = new Dictionary<string, List<StudyProgram>>(StringComparer.OrdinalIgnoreCase);

            foreach (var sp in relevantProgramsFiltered)
            {
                var formatted = FormatFieldOfStudy(sp);

                if (!stringToPrograms.TryGetValue(formatted, out var programList))
                {
                    programList = new List<StudyProgram>();
                    stringToPrograms[formatted] = programList;
                }

                programList.Add(sp);
            }

            var frontExistingGroupIds = model.AssignedFieldOfStudyGroups
                .Where(g => g.GroupId != 0)
                .Select(g => g.GroupId)
                .ToList();

            var groupsToRemove = scheduleClassDaysInDb
                .Where(scd => !frontExistingGroupIds.Contains(scd.Id))
                .ToList();

            foreach (var groupToRemove in groupsToRemove)
            {
                await _scheduleClassDayRepository.DeleteAsync(groupToRemove);
            }

            foreach (var groupDto in model.AssignedFieldOfStudyGroups)
            {
                ScheduleClassDay scd;
                if (groupDto.GroupId == 0)
                {
                    scd = new ScheduleClassDay
                    {
                        Title = groupDto.GroupName,
                        SemesterId = model.SemesterId
                    };
                    await _scheduleClassDayRepository.AddAsync(scd);
                }
                else
                {
                    scd = scheduleClassDaysInDb
                        .First(x => x.Id == groupDto.GroupId);

                    if (scd == null) continue;

                    scd.Title = groupDto.GroupName;
                }

                var studyProgramIds = groupDto.AssignedFieldsOfStudy
                    .SelectMany(x => stringToPrograms[x])
                    .Select(p => p.Id)
                    .Distinct()
                    .ToList();

                await _scheduleClassDayRepository.UpdateAssignmentsAsync(scd.Id, studyProgramIds);
            }

            var semester = await _semesterRepository.GetAsync(model.SemesterId);
            semester.CreationStage = ScheduleCreationStage.GroupsScheduleCreating;
            semester.CreateDate = DateTime.UtcNow;
            semester.UpdateDate = DateTime.UtcNow;
            await _semesterRepository.UpdateAsync(semester);
        }

        public async Task<WeekendAvailabilityDto> GetWeekendAvailabilityAsync(int semesterId)
        {
            var semester = await _semesterRepository.GetAsync(semesterId);
            var groups = await _scheduleClassDayRepository.GetBySemesterIdAsync(semesterId);
            var classDays = await _classDayRepository.GetBySemesterDatesAsync(semester.StartDate, semester.EndDate);
            var holidays = await _holidayRepository.GetByDateRangeAsync(semester.StartDate, semester.EndDate);

            var holidayDates = holidays.Select(h => h.Date.Date).ToHashSet();
            var weekends = new List<WeekendDto>();

            foreach (var date in Utilities.EachDay(semester.StartDate.Date, semester.EndDate.Date))
            {
                if ((date.DayOfWeek == DayOfWeek.Friday ||
                     date.DayOfWeek == DayOfWeek.Saturday ||
                     date.DayOfWeek == DayOfWeek.Sunday)
                    && !holidayDates.Contains(date))
                {
                    var dateClassDays = classDays.Where(cd => cd.StartDateTime.Date == date).ToList();
                    Dictionary<int, bool> availability = new();

                    foreach (var group in groups)
                    {
                        var isAvailable = dateClassDays.Any(cd => cd.ScheduleClassDays.Any(scd => scd.Id == group.Id));
                        availability[group.Id] = isAvailable;
                    }

                    weekends.Add(new WeekendDto
                    {
                        Date = date,
                        Availability = availability
                    });
                }
            }

            return new WeekendAvailabilityDto
            {
                SemesterId = semesterId,
                Name = semester.Name,
                Groups = groups.Select(g => new GroupDto
                {
                    GroupId = g.Id,
                    GroupName = g.Title
                }).ToList(),
                Weekends = weekends
            };
        }

        public async Task SaveWeekendAvailabilityAsync(WeekendAvailabilityDto model)
        {
            var semester = await _semesterRepository.GetAsync(model.SemesterId);
            var existingClassDays = await _classDayRepository.GetBySemesterDatesAsync(semester.StartDate, semester.EndDate);
            var holidays = await _holidayRepository.GetByDateRangeAsync(semester.StartDate, semester.EndDate);
            var holidayDates = holidays.Select(h => h.Date.Date).ToHashSet();

            foreach (var weekend in model.Weekends)
            {
                if (holidayDates.Contains(weekend.Date.Date))
                    continue;

                DateTime classStart = weekend.Date.Date;
                classStart = weekend.Date.DayOfWeek == DayOfWeek.Friday ?
                    classStart.AddHours(17) : classStart.AddHours(7);

                var classEnd = weekend.Date.Date.AddHours(22);

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

                foreach (var entry in weekend.Availability)
                {
                    if (entry.Value)
                    {
                        await _classDayRepository.AssignToScheduleClassDayAsync(classDay.Id, entry.Key);
                    }
                    else
                    {
                        await _classDayRepository.UnassignFromScheduleClassDayAsync(classDay.Id, entry.Key);
                    }
                }
            }

            semester.CreationStage = ScheduleCreationStage.GroupsScheduleCreated;
            semester.UpdateDate = DateTime.UtcNow;
            await _semesterRepository.UpdateAsync(semester);
        }

        public async Task<Result> AcceptWeekendAvailabilityAsync(int semesterId)
        {
            var semester = await _semesterRepository.GetAsync(semesterId);

            if (semester == null)
                return Result.Failure($"Semester with ID {semesterId} not found.", "SEMESTER_NOT_FOUND");

            semester.CreationStage = ScheduleCreationStage.GeneratingPreliminarySchedule;
            semester.UpdateDate = DateTime.UtcNow;

            await _semesterRepository.UpdateAsync(semester);

            _ = Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var scheduleGenerator = scope.ServiceProvider.GetRequiredService<IScheduleGenerator>();

                try
                {
                    await scheduleGenerator.GeneratePreliminaryScheduleAsync(semesterId);
                }
                catch (Exception ex)
                {
                    // logowanie błędu, ale nie przerywamy działania głównej metody
                    //_logger.LogError(ex, "Error while generating preliminary schedule for semester {SemesterId}", semesterId);
                }
            });

            return Result.Success("Weekend availability accepted. Schedule generation started.");
        }

        public async Task<List<ScheduleGroupsDaysCsv>> GetScheduleGroupsDaysCsvAsync(int semesterId)
        {
            var scheduleClassDays = await _scheduleClassDayRepository.GetWithClassDaysBySemesterAsync(semesterId);

            var result = scheduleClassDays
                .SelectMany(scd => scd.ClassDays.Select(cd => new ScheduleGroupsDaysCsv
                {
                    ScheduleGroupId = scd.Id,
                    ScheduleGroupName = scd.Title,
                    DateTimeStart = cd.StartDateTime.ToString("yyyy-MM-dd HH:mm"),
                    DateTimeEnd = cd.EndDateTime.ToString("yyyy-MM-dd HH:mm")
                }))
                .ToList();

            return result;
        }

        private static string FormatFieldOfStudy(StudyProgram sp)
        {
            var baseName = sp.FieldOfStudy.Name;
            var suffix = sp.StudyDegree switch
            {
                StudyDegree.Inz => " - I Stopień",
                StudyDegree.Lic => " - I Stopień",
                StudyDegree.Mgr => " - II Stopień",
                StudyDegree.USM => " - II Stopień",
                StudyDegree.USM3Sem => " - II Stopień",
                StudyDegree.USMSp => " - II Stopień",
                _ => " - Nieznany Stopień"
            };

            return baseName + suffix;
        }

        private static int GetStudyDegreeRank(StudyDegree sd)
        {
            return sd switch
            {
                StudyDegree.Inz => 0,
                StudyDegree.Lic => 0,
                StudyDegree.Mgr => 1,
                StudyDegree.USM => 1,
                StudyDegree.USM3Sem => 1,
                StudyDegree.USMSp => 1,
                _ => 2
            };
        }
    }
}