using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Schedule.Models;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.Shared.Utilities;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ICourseScheduleRepository _courseScheduleRepository;

        public ScheduleService(
            ICourseScheduleRepository courseScheduleRepository)
        {
            _courseScheduleRepository = courseScheduleRepository;
        }

        public async Task<List<ScheduleItemDto>> GetScheduleAsync(ScheduleRequestDto request)
        {
            var (startDate, endDate) = GetDateRange(request.CurrentDate, request.ViewType);

            var schedules = await _courseScheduleRepository
                .GetWithDetailsAsync(request.Semester, startDate, endDate);

            return schedules
                .Where(s => s.CoursesDetails.Any(cd =>
                    FormatStudyProgramFullName(cd.Course.StudyProgram) == request.Name))
                .Select(s => new ScheduleItemDto
                {
                    Id = s.Id,
                    Text = s.CoursesDetails.First().Course.Name,
                    Start = s.StartDateTime,
                    End = s.EndDateTime,
                    GroupName = string.Join(",", s.CoursesGroups?.Select(x => x.GroupName).ToList() ?? new List<string>()),
                    Instructor = s.Instructor != null
                        ? $"{s.Instructor.Title} {s.Instructor.FirstName} {s.Instructor.LastName}"
                        : "-",
                    Room = s.CoursesDetails.First().Online ? "Online" : s.Classroom?.RoomNumber ?? ""
                })
                .ToList();
        }

        private (DateTime Start, DateTime End) GetDateRange(DateTime currentDate, string viewType)
        {
            if (viewType == "week")
            {
                var monday = currentDate.Date.AddDays(-(int)currentDate.DayOfWeek + (currentDate.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
                var sunday = monday.AddDays(6);
                return (monday, sunday);
            }

            var firstOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var start = firstOfMonth.AddDays(-(int)firstOfMonth.DayOfWeek + (firstOfMonth.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));

            var lastOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);
            var end = lastOfMonth.AddDays(7 - (int)lastOfMonth.DayOfWeek);

            return (start.Date, end.Date);
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