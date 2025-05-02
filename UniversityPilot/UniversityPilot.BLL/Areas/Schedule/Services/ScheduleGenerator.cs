using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Files.Interfaces;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    internal class ScheduleGenerator : IScheduleGenerator
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassDayRepository _classDayRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseDetailsRepository _courseDetailsRepository;
        private readonly IScheduleClassDayRepository _scheduleClassDayRepository;
        private readonly ICourseScheduleRepository _courseScheduleRepository;
        private readonly IHolidayRepository _holidayRepository;
        private readonly ICsvService _csvService;

        public ScheduleGenerator(
            ISemesterRepository semesterRepository,
            IClassDayRepository classDayRepository,
            ICourseRepository courseRepository,
            ICourseDetailsRepository courseDetailsRepository,
            IScheduleClassDayRepository scheduleClassDayRepository,
            ICourseScheduleRepository courseScheduleRepository,
            IHolidayRepository holidayRepository,
            ICsvService csvService)
        {
            _semesterRepository = semesterRepository;
            _classDayRepository = classDayRepository;
            _courseRepository = courseRepository;
            _courseDetailsRepository = courseDetailsRepository;
            _scheduleClassDayRepository = scheduleClassDayRepository;
            _courseScheduleRepository = courseScheduleRepository;
            _holidayRepository = holidayRepository;
            _csvService = csvService;
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
            await GenerateFilesCSV(semester);
            await GenerateWithAiAsync();
            await SetSemesterStageToGeneratedScheduleAsync(semester);
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

            const string scheduleTitle = "Stacjonarna grupa";
            var scheduleClassDay = await _scheduleClassDayRepository.GetBySemesterIdAndTitleAsync(semester.Id, scheduleTitle);

            if (scheduleClassDay == null)
            {
                scheduleClassDay = new ScheduleClassDay
                {
                    Title = scheduleTitle,
                    SemesterId = semester.Id
                };

                await _scheduleClassDayRepository.AddAsync(scheduleClassDay);
            }
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

        public async Task GeneratePreliminaryCourseSchedulesAsync(Semester semester)
        {
            var allCourseDetails = await _courseDetailsRepository.GetCourseDetailsWithDependenciesAsync(semester.Id);
            var courseSchedules = new List<CourseSchedule>();
            var scheduleGroupAssignments = new List<(CourseSchedule Schedule, CourseGroup Group)>();
            var scheduleDetailAssignments = new List<(CourseSchedule Schedule, CourseDetails Details)>();

            var groupedByShared = allCourseDetails
                .Where(cd => cd.SharedCourseGroup != null)
                .GroupBy(cd => cd.SharedCourseGroup.Id)
                .ToList();

            var standaloneDetails = allCourseDetails
                .Where(cd => cd.SharedCourseGroup == null)
                .ToList();

            var startDate = semester.StartDate.Date.AddHours(8);
            var blockStart = startDate;

            foreach (var sharedGroup in groupedByShared)
            {
                var firstDetails = sharedGroup.First();
                var allInstructors = sharedGroup.SelectMany(cd => cd.Instructors).Distinct().ToList();
                var instructorQueue = new Queue<int>(allInstructors.Select(i => i.Id));
                int maxGroupCount = sharedGroup.Max(cd => cd.CourseGroups.Count);

                for (int i = 0; i < maxGroupCount; i++)
                {
                    if (instructorQueue.Count == 0)
                        instructorQueue = new Queue<int>(allInstructors.Select(i => i.Id));

                    var instructorId = instructorQueue.Dequeue();
                    var blocks = CalculateTimeBlocks(firstDetails.Hours);

                    var courseGroupsSet = new List<CourseGroup>();
                    var courseDetailsSet = new List<CourseDetails>();

                    foreach (var cd in sharedGroup)
                    {
                        var group = cd.CourseGroups.ElementAtOrDefault(i);
                        if (group != null)
                        {
                            courseGroupsSet.Add(group);
                            courseDetailsSet.Add(cd);
                        }
                    }

                    foreach (var block in blocks)
                    {
                        var newSchedule = new CourseSchedule
                        {
                            StartDateTime = blockStart,
                            EndDateTime = blockStart.AddMinutes(block),
                            Status = "Planning",
                            ClassroomId = null,
                            InstructorId = instructorId
                        };

                        courseSchedules.Add(newSchedule);

                        foreach (var cd in courseDetailsSet)
                            scheduleDetailAssignments.Add((newSchedule, cd));

                        foreach (var cg in courseGroupsSet)
                            scheduleGroupAssignments.Add((newSchedule, cg));
                    }
                }
            }

            foreach (var details in standaloneDetails)
            {
                var blocks = CalculateTimeBlocks(details.Hours);
                var courseGroups = details.CourseGroups;
                var instructorQueue = new Queue<int>(details.Instructors.Select(i => i.Id));

                foreach (var group in courseGroups)
                {
                    if (instructorQueue.Count == 0)
                        instructorQueue = new Queue<int>(details.Instructors.Select(i => i.Id));

                    var instructorId = instructorQueue.Dequeue();

                    foreach (var block in blocks)
                    {
                        var newSchedule = new CourseSchedule
                        {
                            StartDateTime = blockStart,
                            EndDateTime = blockStart.AddMinutes(block),
                            Status = "Planning",
                            ClassroomId = null,
                            InstructorId = instructorId
                        };

                        courseSchedules.Add(newSchedule);
                        scheduleDetailAssignments.Add((newSchedule, details));
                        scheduleGroupAssignments.Add((newSchedule, group));
                    }
                }
            }

            await _courseScheduleRepository.AddRangeAsync(courseSchedules);

            foreach (var (schedule, details) in scheduleDetailAssignments)
            {
                await _courseScheduleRepository.AssignCourseDetailsAsync(schedule.Id, details.Id);
            }

            foreach (var (schedule, group) in scheduleGroupAssignments)
            {
                await _courseScheduleRepository.AssignCourseGroupAsync(schedule.Id, group.Id);
            }
        }

        private List<int> CalculateTimeBlocks(int totalHours)
        {
            var blocks = new List<int>();

            while (totalHours >= 4)
            {
                blocks.Add(4);
                totalHours -= 4;
            }

            if (totalHours == 1)
            {
                if (blocks.Any())
                    blocks[0] += 1;
                else
                    blocks.Add(1);
            }
            else if (totalHours == 2)
            {
                if (blocks.Count >= 1)
                {
                    blocks[blocks.Count - 1] = 3;
                    blocks.Add(3);
                }
                else
                    blocks.Add(2);
            }
            else if (totalHours == 3)
            {
                blocks.Add(3);
            }

            return blocks.Select(h => h * 45 + (h - 1) * 5).ToList();
        }

        private async Task SetSemesterStageToGeneratingScheduleAsync(Semester semester)
        {
            semester.CreationStage = ScheduleCreationStage.GeneratingSchedule;
            semester.UpdateDate = DateTime.UtcNow;
            await _semesterRepository.UpdateAsync(semester);
        }

        private async Task GenerateFilesCSV(Semester semester)
        {
            var basePath = Path.Combine("..", "..", "UniversityPilot-ML", "DataInput");
            Directory.CreateDirectory(basePath);

            var scheduleGroupDaysCsv = await _csvService.GetScheduleGroupsDaysCsv(semester.Id);
            await File.WriteAllTextAsync(Path.Combine(basePath, "ScheduleGroupsDays.csv"), scheduleGroupDaysCsv);

            var classroomsCsv = await _csvService.GetClassroomsCsv();
            await File.WriteAllTextAsync(Path.Combine(basePath, "Classrooms.csv"), classroomsCsv);

            var preliminarySchedulesCsv = await _csvService.GetPreliminaryCoursesScheduleCsv(semester.Id);
            await File.WriteAllTextAsync(Path.Combine(basePath, "PreliminaryCoursesSchedule.csv"), preliminarySchedulesCsv);
        }

        private async Task GenerateWithAiAsync()
        {
            // TODO: obsługa skryptów .py
            await UpdateCourseSchedulesFromCsvAsync();
        }

        private async Task UpdateCourseSchedulesFromCsvAsync()
        {
            var basePath = Path.Combine("..", "..", "UniversityPilot-ML", "DataOutput");
            var filePath = Path.Combine(basePath, "GeneratedSchedule.csv");

            if (!File.Exists(filePath))
                return;

            var updates = new List<GeneratedScheduleCsv>();

            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(reader))
            {
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;
                parser.TrimWhiteSpace = true;

                bool isFirstRow = true;
                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();

                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    if (fields != null)
                        updates.Add(CsvHandler.MapCsvRowToObject<GeneratedScheduleCsv>(fields));
                }
            }

            foreach (var update in updates)
            {
                await _courseScheduleRepository.UpdateStartEndDateAsync(
                    update.CourseScheduleId,
                    update.NewDateTimeStart,
                    update.NewDateTimeEnd
                );
            }
        }

        private async Task SetSemesterStageToGeneratedScheduleAsync(Semester semester)
        {
            semester.CreationStage = ScheduleCreationStage.GeneratedSchedule;
            semester.UpdateDate = DateTime.UtcNow;
            await _semesterRepository.UpdateAsync(semester);
        }
    }
}