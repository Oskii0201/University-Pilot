﻿using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Files.Interfaces;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Processing.Services;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
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
        private readonly ICourseScheduleRepository _courseScheduleRepository;
        private readonly ICourseRepository _courseRepository;

        public CsvService(
            IClassroomService classroomService,
            IInstructorService instructorService,
            IStudyProgramService studyProgramService,
            IHolidayService holidayService,
            ICourseDetailsRepository courseDetailsRepository,
            ICourseDetailsService courseDetailsService,
            IGroupsScheduleService groupsScheduleService,
            ICourseScheduleRepository courseScheduleRepository,
            ICourseRepository courseRepository)
        {
            _classroomService = classroomService;
            _instructorService = instructorService;
            _studyProgramService = studyProgramService;
            _holidayService = holidayService;
            _courseDetailsRepository = courseDetailsRepository;
            _courseDetailsService = courseDetailsService;
            _groupsScheduleService = groupsScheduleService;
            _courseScheduleRepository = courseScheduleRepository;
            _courseRepository = courseRepository;
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
                        Online = cd.Online ? "Tak" : "Nie",
                        CourseGroups = string.Join(",", cd.CourseGroups.Select(g => g.Id)),
                        GroupsName = string.Join(",", cd.CourseGroups.Select(g => g.GroupName)),
                        SharedCourseGroup = cd.SharedCourseGroup?.Name ?? "",
                        Instructors = string.Join(",", cd.Instructors.Select(i => i.Id)),
                        InstructorsTitle = string.Join(",", cd.Instructors.Select(i => i.Title)),
                        InstructorsFirstName = string.Join(",", cd.Instructors.Select(i => i.FirstName)),
                        InstructorsLastName = string.Join(",", cd.Instructors.Select(i => i.LastName))
                    }).ToList();

            return CsvHandler.Build(courseDetailsCsv);
        }

        public async Task<string> GetInstructorsCsv()
        {
            var instructors = await _instructorService.GetAllInstructorsCsv();
            return CsvHandler.Build(instructors);
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

        public async Task<string> GetPreliminaryCoursesScheduleCsv(int semesterId)
        {
            var coursesSchedule = await _courseScheduleRepository.GetAllWithDetailsBySemesterIdAsync(semesterId);
            var programSemesters = coursesSchedule
                .SelectMany(cs => cs.CoursesDetails)
                .Select(cd => (ProgramId: cd.Course.StudyProgram.Id, SemesterNumber: cd.Course.SemesterNumber))
                .Distinct()
                .ToList();

            var programGroupsMap = await _courseRepository.GetCourseGroupsByProgramAndSemesterAsync(programSemesters);

            var result = coursesSchedule.Select(cs =>
            {
                var cd = cs.CoursesDetails.First();
                var scheduleGroup = cd.Course.StudyProgram.ScheduleClassDays
                    .FirstOrDefault(x => x.SemesterId == semesterId);

                var programId = cd.Course.StudyProgram.Id;
                var semesterNumber = cd.Course.SemesterNumber;
                var isLecture = cd.CourseType == CourseTypes.Lecture;

                var activeGroups = cs.CoursesGroups.ToList();

                var allGroupsInProgramSemester = programGroupsMap
                    .GetValueOrDefault((programId, semesterNumber)) ?? new List<CourseGroup>();

                List<CourseGroup> dependentGroups;

                if (isLecture)
                {
                    dependentGroups = allGroupsInProgramSemester
                        .Where(g => !activeGroups.Any(ag => ag.Id == g.Id))
                        .ToList();
                }
                else
                {
                    dependentGroups = allGroupsInProgramSemester
                        .Where(g =>
                            g.CourseType != cd.CourseType &&
                            !activeGroups.Any(ag => ag.Id == g.Id))
                        .ToList();
                }

                return new PreliminaryCoursesScheduleCsv
                {
                    CourseScheduleId = cs.Id,
                    CourseName = cd.Course?.Name ?? string.Empty,
                    CourseDetailsId = string.Join(",", cs.CoursesDetails.Select(d => d.Id)),
                    CourseType = cd.CourseType.ToString(),
                    Online = cd.Online ? "Yes" : "No",
                    GroupsId = string.Join(",", activeGroups.Select(g => g.Id)),
                    GroupsName = string.Join(",", activeGroups.Select(g => g.GroupName)),
                    DependentGroupsIds = string.Join(",", dependentGroups.Select(g => g.Id)),
                    DependentGroupsNames = string.Join(",", dependentGroups.Select(g => g.GroupName)),
                    ScheduleGroupId = scheduleGroup?.Id ?? 0,
                    ScheduleGroupName = scheduleGroup?.Title ?? "Brak przypisania",
                    InstructorId = cs.Instructor?.Id ?? 0,
                    DateTimeStart = cs.StartDateTime.ToString("yyyy-MM-dd HH:mm"),
                    DateTimeEnd = cs.EndDateTime.ToString("yyyy-MM-dd HH:mm"),
                    Duration = (int)(cs.EndDateTime - cs.StartDateTime).TotalMinutes,
                    ClassroomId = cs.ClassroomId ?? 0
                };
            }).ToList();

            return CsvHandler.Build(result);
        }
    }
}