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
        private readonly ISemesterRepository _semesterRepository;

        public StudyProgramService(
            IFieldOfStudyRepository fieldOfStudyRepository,
            IStudyProgramRepository studyProgramRepository,
            ISpecializationRepository specializationRepository,
            ISemesterRepository semesterRepository)
        {
            _fieldOfStudyRepository = fieldOfStudyRepository;
            _studyProgramRepository = studyProgramRepository;
            _specializationRepository = specializationRepository;
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

            foreach (var studyProgram in studyProgramsCsv)
            {
                if (!existingFieldOfStudies.Contains(studyProgram.FieldOfStudy))
                {
                    var newFieldOfStudy = new FieldOfStudy()
                    {
                        Name = studyProgram.FieldOfStudy
                    };
                    _fieldOfStudyRepository.Add(newFieldOfStudy);
                    existingFieldOfStudies.Add(studyProgram.FieldOfStudy);
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
                .GroupBy(sp => new { sp.EnrollmentYear, sp.StudyForm })
                .ToList();

            var tempStudyProgramsCsv = new HashSet<(string EnrollmentYear, string StudyForm)>();

            foreach (var group in groupedStudyPrograms)
            {
                var studyProgram = group.First();
                if (!tempStudyProgramsCsv.Contains((studyProgram.EnrollmentYear, studyProgram.StudyForm)))
                {
                    tempStudyProgramsCsv.Add((studyProgram.EnrollmentYear, studyProgram.StudyForm));

                    var dbStudyProgram = GetDbStudyProgram(fieldsOfStudy, studyProgram);

                    CreateUniqueSemesters(dbStudyProgram, group.ToList());
                    CreateUniqueCoursesInStudyProgram(dbStudyProgram, group.ToList());
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

                bool isWinterSemester = (studyProgram.SummerRecruitment && i % 2 == 0) ||
                                        (!studyProgram.SummerRecruitment && i % 2 == 1);

                var (startYear, endYear) = ParseSemesterYears(semesterName);

                var newSemester = new Semester
                {
                    AcademicYear = $"{startYear}/{endYear}",
                    Name = semesterName,
                    StartDate = GenerateSemestrStartDate(startYear, isWinterSemester),
                    EndDate = GenerateSemestrEndDate(isWinterSemester ? endYear : startYear, isWinterSemester)
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

        private void CreateUniqueCoursesInStudyProgram(StudyProgram dbStudyProgram, List<StudyProgramCsv> studyProgramWithCoursesCsv)
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
                    int courseCountInProgram = studyProgramWithCoursesCsv.Count(c =>
                        c.CourseName == courseCsv.CourseName &&
                        c.SemesterNumber == courseCsv.SemesterNumber);

                    if (numberOfSpecialization <= 1)
                    {
                        // TODO
                    }
                    else if (courseCountInProgram == numberOfSpecialization)
                    {
                        // TODO
                    }
                    else
                    {
                        // TODO
                    }

                    coursesDictionary[courseKey] = courseCsv;
                }
            }
        }

        private string CreateSemesterName(string enrollmentYear, bool summerRecruitment, int semesterNumber)
        {
            var years = enrollmentYear.Split('/');
            int startYear = int.Parse(years[0]);

            int yearAdjustment = summerRecruitment ? 0 : 1;
            int semesterShift = (semesterNumber - 1) / 2;

            int resultYear = startYear + semesterShift + yearAdjustment;

            string semesterName = semesterNumber % 2 == 1 ? "Semestr zimowy" : "Semestr letni";

            return $"{resultYear}/{resultYear + 1} - {semesterName}";
        }
    }
}