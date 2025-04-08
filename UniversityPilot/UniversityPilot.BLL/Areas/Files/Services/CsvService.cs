using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Files.Interfaces;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Processing.Services;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.Shared.Utilities;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;

namespace UniversityPilot.BLL.Areas.Files.Services
{
    internal class CsvService : ICsvService
    {
        private readonly IClassroomService _classroomService;
        private readonly IInstructorService _instructorService;
        private readonly IStudyProgramService _studyProgramService;
        private readonly IHolidayService _holidayService;
        private readonly ICourseDetailsRepository _courseDetailsRepository;
        private readonly ICourseDetailsService _courseDetailsService;
        private readonly IGroupsScheduleService _groupsScheduleService;

        public CsvService(
            IClassroomService classroomService,
            IInstructorService instructorService,
            IStudyProgramService studyProgramService,
            IHolidayService holidayService,
            ICourseDetailsRepository courseDetailsRepository,
            ICourseDetailsService courseDetailsService,
            IGroupsScheduleService groupsScheduleService)
        {
            _classroomService = classroomService;
            _instructorService = instructorService;
            _studyProgramService = studyProgramService;
            _holidayService = holidayService;
            _courseDetailsRepository = courseDetailsRepository;
            _courseDetailsService = courseDetailsService;
            _groupsScheduleService = groupsScheduleService;
        }

        public async Task<Result> UploadAsync(UploadDatasetDto data)
        {
            try
            {
                if (data.File == null || data.File.Length == 0)
                    return Result.Failure("File is empty.", "File_Empty", new[] { "No file content provided." });

                if (!Path.GetExtension(data.File.FileName).Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
                    return Result.Failure("CSV file contains invalid data.", "Invalid_Extension", new[] { "Bad file extension." });

                return await SaveToDatabase(data);
            }
            catch (Exception ex)
            {
                return Result.Failure("An error occurred during CSV processing.", "500", new[] { ex.Message });
            }
        }

        private async Task<Result> SaveToDatabase(UploadDatasetDto data)
        {
            switch (data.Dataset)
            {
                case FileType.StudyProgram:
                    var studyProgramsCsv = CsvHandler.ReadCsvFileToObject<StudyProgramCsv>(data.File);
                    return _studyProgramService.SaveFromCsv(studyProgramsCsv);

                case FileType.Classrooms:
                    var classroomsCsv = CsvHandler.ReadCsvFileToObject<ClassroomCsv>(data.File);
                    return await _classroomService.SaveFromCsv(classroomsCsv);

                case FileType.Holidays:
                    var holidaysCsv = CsvHandler.ReadCsvFileToObject<HolidaysCsv>(data.File);
                    return await _holidayService.SaveFromCsv(holidaysCsv);

                case FileType.Instructors:
                    var instructorsCsv = CsvHandler.ReadCsvFileToObject<InstructorCsv>(data.File);
                    return await _instructorService.SaveFromCsv(instructorsCsv);

                case FileType.CourseAssignment:
                    var courseDetailsCsv = CsvHandler.ReadCsvFileToObject<CourseDetailsCsv>(data.File);
                    return await _courseDetailsService.UpdateFromCsv(courseDetailsCsv);

                default:
                    return Result.Failure($"Unsupported file type: {data.Dataset}");
            }
        }

        public async Task<string> GetCourseDetailsCsv(int semesterId)
        {
            var courseDetails = await _courseDetailsRepository.GetCourseDetailsExport(semesterId);
            var courseDetailsCsv = courseDetails.Select(cd =>
                    new CourseDetailsCsv
                    {
                        CourseDetailsId = cd.Id,
                        EnrollmentYear = cd.Course.StudyProgram.EnrollmentYear,
                        SemesterNumber = cd.Course.SemesterNumber,
                        FieldOfStudy = cd.Course.StudyProgram.FieldOfStudy.Name,
                        SummerRecruitment = cd.Course.StudyProgram.SummerRecruitment ? "Tak" : "Nie",
                        StudyForm = EnumHelper.GetEnumDescription(cd.Course.StudyProgram.StudyForm),
                        StudyDegree = cd.Course.StudyProgram.StudyDegree.ToString(),
                        Specialization = cd.Course.Specialization?.Name ?? "",
                        CourseName = cd.Course.Name,
                        CourseType = EnumHelper.GetEnumDescription(cd.CourseType),
                        TitleScheduleClassDay = cd.Course.StudyProgram.ScheduleClassDays.Select(d => d.Title).FirstOrDefault() ?? "",
                        CourseGroups = string.Join("|", cd.CourseGroups.Select(g => g.Id)),
                        GroupsName = string.Join("|", cd.CourseGroups.Select(g => g.GroupName)),
                        SharedCourseGroup = cd.SharedCourseGroup?.Name ?? "",
                        Instructors = string.Join("|", cd.Instructors.Select(i => i.Id)),
                        InstructorsTitle = string.Join("|", cd.Instructors.Select(i => i.Title)),
                        InstructorsFirstName = string.Join("|", cd.Instructors.Select(i => i.FirstName)),
                        InstructorsLastName = string.Join("|", cd.Instructors.Select(i => i.LastName))
                    }).ToList();

            return CsvHandler.Build(courseDetailsCsv);
        }

        public async Task<string> GetScheduleGroupsDaysCsv(int semesterId)
        {
            List<ScheduleGroupsDaysCsv> result = await _groupsScheduleService.GetScheduleGroupsDaysCsvAsync(semesterId);
            return CsvHandler.Build(result);
        }

        public async Task<string> GetClassroomsCsv()
        {
            var result = await _classroomService.GetAllClassroomsCsv();
            return CsvHandler.Build(result);
        }

        public async Task<string> GetPreliminaryCoursesScheduleCsv(int id)
        {
            List<PreliminaryCoursesScheduleCsv> result = new();
            return CsvHandler.Build(result);
        }
    }
}