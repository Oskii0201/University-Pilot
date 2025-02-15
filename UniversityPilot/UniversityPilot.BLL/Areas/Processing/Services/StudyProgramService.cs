using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.Shared.Utilities;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.BLL.Areas.Processing.Services
{
    internal class StudyProgramService : IStudyProgramService
    {
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;
        private readonly IStudyProgramRepository _studyProgramRepository;
        private readonly ISpecializationRepository _specializationRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISemesterRepository _semesterRepository;

        public StudyProgramService(
            IFieldOfStudyRepository fieldOfStudyRepository,
            IStudyProgramRepository studyProgramRepository,
            ISpecializationRepository specializationRepository,
            ICourseRepository courseRepository,
            ISemesterRepository semesterRepository)
        {
            _fieldOfStudyRepository = fieldOfStudyRepository;
            _studyProgramRepository = studyProgramRepository;
            _specializationRepository = specializationRepository;
            _courseRepository = courseRepository;
            _semesterRepository = semesterRepository;
        }

        public Result SaveFromCsv(List<StudyProgramCsv> studyProgramsCsv)
        {
            try
            {
                CreateUniqueFieldOfStudy(studyProgramsCsv);
                CreateUniqueSpecialization(studyProgramsCsv);
                CreateUniqueStudyProgramsForFieldOfStudy(studyProgramsCsv);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        private void CreateUniqueFieldOfStudy(List<StudyProgramCsv> studyProgramsCsv)
        {
            var fieldOfStudies = _fieldOfStudyRepository.GetAll();
            var existingFieldOfStudies = new HashSet<string>(fieldOfStudies.Select(f => f.Name));

            foreach (var fieldOfStudy in studyProgramsCsv.Select(x => x.FieldOfStudy).Distinct())
            {
                if (!existingFieldOfStudies.Contains(fieldOfStudy))
                {
                    var newFieldOfStudy = new FieldOfStudy()
                    {
                        Name = fieldOfStudy
                    };
                    _fieldOfStudyRepository.Add(newFieldOfStudy);
                    existingFieldOfStudies.Add(fieldOfStudy);
                }
            }
        }

        private void CreateUniqueSpecialization(List<StudyProgramCsv> studyProgramsCsv)
        {
            var specializationsDb = _specializationRepository.GetAll();
            var existingSpecializations = new HashSet<string>(specializationsDb.Select(s => s.Name));

            foreach (var specialization in studyProgramsCsv.Select(s => s.Specialization).Distinct())
            {
                if (!existingSpecializations.Contains(specialization))
                {
                    var newSpecialization = new Specialization()
                    {
                        Name = specialization
                    };
                    _specializationRepository.Add(newSpecialization);
                    existingSpecializations.Add(specialization);
                }
            }
        }

        private void CreateUniqueStudyProgramsForFieldOfStudy(List<StudyProgramCsv> studyProgramsCsv)
        {
            var fieldsOfStudy = _fieldOfStudyRepository.GetAll().ToList();
            var groupedStudyPrograms = studyProgramsCsv
                .GroupBy(sp => new { sp.EnrollmentYear, sp.StudyForm, sp.FieldOfStudy })
                .ToList();

            var tempStudyProgramsCsv = new HashSet<(string EnrollmentYear, string StudyForm, string FieldOfStudy)>();

            foreach (var group in groupedStudyPrograms)
            {
                var studyProgram = group.First();
                if (!tempStudyProgramsCsv.Contains((studyProgram.EnrollmentYear, studyProgram.StudyForm, studyProgram.FieldOfStudy)))
                {
                    tempStudyProgramsCsv.Add((studyProgram.EnrollmentYear, studyProgram.StudyForm, studyProgram.FieldOfStudy));

                    var dbStudyProgram = GetDbStudyProgram(fieldsOfStudy, studyProgram);

                    CreateUniqueSemesters(dbStudyProgram, group.ToList());
                    //CreateUniqueCoursesInStudyProgram(dbStudyProgram, group.ToList()); TODO: do poprawienia wydajność
                }
            }
        }

        private StudyProgram GetDbStudyProgram(List<FieldOfStudy> fieldsOfStudy, StudyProgramCsv studyProgram)
        {
            var newStudyProgram = ParseEnrollmentYear(studyProgram.EnrollmentYear);
            newStudyProgram.StudyForm = EnumHelper.ParseEnumFromDescriptionOrDefault(studyProgram.StudyForm, StudyForms.Unknown);
            newStudyProgram.FieldOfStudyId = fieldsOfStudy.First(f => f.Name == studyProgram.FieldOfStudy).Id;

            var existingStudyProgram = _studyProgramRepository.GetExistingStudyProgramWithIncludes(newStudyProgram);

            if (existingStudyProgram != null)
            {
                return existingStudyProgram;
            }

            _studyProgramRepository.Add(newStudyProgram);
            return newStudyProgram;
        }

        private StudyProgram ParseEnrollmentYear(string enrollmentYearCsv)
        {
            var enrollmentYear = enrollmentYearCsv.Substring(0, 9);
            var summerRecruitment = enrollmentYearCsv.Contains("n. l.");
            var studyDegreePart = enrollmentYearCsv
                                    .Replace(enrollmentYear, "")
                                    .Replace("- Studia", "")
                                    .Replace("n. l.", "")
                                    .Trim();

            return new StudyProgram()
            {
                EnrollmentYear = enrollmentYear,
                StudyDegree = studyDegreePart,
                SummerRecruitment = summerRecruitment
            };
        }

        private void CreateUniqueSemesters(StudyProgram studyProgram, List<StudyProgramCsv> studyProgramCsvs)
        {
            var existingSemesterNames = _semesterRepository.GetAll().Select(s => s.Name).ToHashSet();
            int maxSemester = studyProgramCsvs.Max(x => x.SemesterNumber);

            for (int i = 1; i <= maxSemester; i++)
            {
                var semesterName = CreateSemesterName(studyProgram.EnrollmentYear, studyProgram.SummerRecruitment, i);

                if (existingSemesterNames.Contains(semesterName))
                    continue;

                bool isWinterSemester = semesterName.Contains("Semestr zimowy");

                var (startYear, endYear) = ParseSemesterYears(semesterName);

                var newSemester = new Semester
                {
                    AcademicYear = $"{startYear}/{endYear}",
                    Name = semesterName,
                    StartDate = GenerateSemestrStartDate(isWinterSemester ? startYear : endYear, isWinterSemester),
                    EndDate = GenerateSemestrEndDate(endYear, isWinterSemester)
                };

                _semesterRepository.Add(newSemester);
                existingSemesterNames.Add(semesterName);
            }
        }

        private (int startYear, int endYear) ParseSemesterYears(string semesterName)
        {
            var years = semesterName[..9].Split('/');
            return (int.Parse(years[0]), int.Parse(years[1]));
        }

        public DateTime GenerateSemestrStartDate(int year, bool isWinterSemester)
            => isWinterSemester ?
            new DateTime(year, 10, 1, 0, 0, 1, DateTimeKind.Utc) :
            new DateTime(year, 3, 1, 0, 0, 1, DateTimeKind.Utc);

        public DateTime GenerateSemestrEndDate(int year, bool isWinterSemester)
            => isWinterSemester ?
            new DateTime(year, 2, DateTime.DaysInMonth(year, 2), 23, 59, 59, DateTimeKind.Utc) :
            new DateTime(year, 9, DateTime.DaysInMonth(year, 9), 23, 59, 59, DateTimeKind.Utc);

        private void CreateUniqueCoursesInStudyProgram(StudyProgram studyProgram, List<StudyProgramCsv> studyProgramWithCoursesCsv)
        {
            var specializations = _specializationRepository.GetAll();
            var semesters = _semesterRepository.GetAll();

            int numberOfSpecialization = studyProgramWithCoursesCsv
                .Select(s => s.Specialization)
                .Distinct()
                .Count();

            var coursesDictionary = new Dictionary<string, StudyProgramCsv>();

            foreach (var courseCsv in studyProgramWithCoursesCsv)
            {
                string courseKey = $"{courseCsv.CourseName}_{courseCsv.SemesterNumber}";
                if (!coursesDictionary.ContainsKey(courseKey))
                {
                    int courseCountInProgram = studyProgramWithCoursesCsv.Where(c =>
                            c.CourseName == courseCsv.CourseName &&
                            c.SemesterNumber == courseCsv.SemesterNumber)
                        .Select(c => (c.CourseName, c.Specialization))
                        .Distinct()
                        .Count();

                    var semesterName = CreateSemesterName(studyProgram.EnrollmentYear, studyProgram.SummerRecruitment, courseCsv.SemesterNumber);
                    var semesterId = semesters.First(s => s.Name == semesterName).Id;
                    var courses = studyProgramWithCoursesCsv.Where(c =>
                            c.CourseName == courseCsv.CourseName &&
                            c.SemesterNumber == courseCsv.SemesterNumber &&
                            c.Specialization == courseCsv.Specialization);

                    if (numberOfSpecialization <= 1)
                    {
                        var newCourse = new Course()
                        {
                            Name = courseCsv.CourseName,
                            SpecializationId = string.IsNullOrEmpty(courseCsv.Specialization) ? null : specializations.First(s => s.Name == courseCsv.Specialization).Id,
                            SemesterNumber = courseCsv.SemesterNumber,
                            SemesterId = semesterId,
                            CoursesDetails = GenerateCoursesDetails(studyProgram, courses),
                            StudyProgramId = studyProgram.Id
                        };
                        _courseRepository.Add(newCourse);
                    }
                    else if (courseCountInProgram == numberOfSpecialization)
                    {
                        var newCourse = new Course()
                        {
                            Name = courseCsv.CourseName,
                            SpecializationId = null,
                            SemesterNumber = courseCsv.SemesterNumber,
                            SemesterId = semesterId,
                            CoursesDetails = GenerateCoursesDetails(studyProgram, courses),
                            StudyProgramId = studyProgram.Id
                        };
                        _courseRepository.Add(newCourse);
                    }
                    else
                    {
                        foreach (var courseGroup in courses.GroupBy(c => c.Specialization))
                        {
                            var firstCourse = courseGroup.First();
                            var newCourse = new Course
                            {
                                Name = firstCourse.CourseName,
                                SpecializationId = specializations.First(s => s.Name == firstCourse.Specialization).Id,
                                SemesterNumber = firstCourse.SemesterNumber,
                                SemesterId = semesterId,
                                CoursesDetails = GenerateCoursesDetails(studyProgram, courseGroup.ToList()),
                                StudyProgramId = studyProgram.Id
                            };
                            _courseRepository.Add(newCourse);
                        }
                    }

                    coursesDictionary[courseKey] = courseCsv;
                }
            }
        }

        private ICollection<CourseDetails> GenerateCoursesDetails(StudyProgram dbStudyProgram, IEnumerable<StudyProgramCsv> courses)
        {
            var coresesDetails = new List<CourseDetails>();

            foreach (var course in courses)
            {
                coresesDetails.Add(new CourseDetails()
                {
                    CourseType = EnumHelper.ParseEnumFromDescriptionOrDefault(course.CourseType, CourseTypes.Unknown),
                    Hours = course.Hours,
                    AssessmentType = course.AssessmentType,
                    ECTS = course.ECTS
                });
            }

            return coresesDetails;
        }

        private string CreateSemesterName(string enrollmentYear, bool summerRecruitment, int semesterNumber)
        {
            int startYear = int.Parse(enrollmentYear.Split('/')[0]);

            int yearShift = (semesterNumber + (summerRecruitment ? 1 : 0) - 1) / 2;
            bool isWinterSemester = (semesterNumber % 2 == (summerRecruitment ? 0 : 1));
            string semesterName = isWinterSemester ? "Semestr zimowy" : "Semestr letni";

            int academicYearStart = startYear + yearShift;
            int academicYearEnd = academicYearStart + 1;

            return $"{academicYearStart}/{academicYearEnd} - {semesterName}";
        }
    }
}