using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
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

        public StudyProgramService(
            IFieldOfStudyRepository fieldOfStudyRepository,
            IStudyProgramRepository studyProgramRepository)
        {
            _fieldOfStudyRepository = fieldOfStudyRepository;
            _studyProgramRepository = studyProgramRepository;
        }

        public Result SaveFromCsv(List<StudyProgramCsv> studyProgramsCsv)
        {
            try
            {
                var fieldsOfStudy = GetListOfUniqueFieldOfStudy(studyProgramsCsv);
                CreateUniqueStudyProgramsForFieldOfStudy(fieldsOfStudy, studyProgramsCsv);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        private List<FieldOfStudy> GetListOfUniqueFieldOfStudy(List<StudyProgramCsv> studyProgramsCsv)
        {
            var fieldOfStudies = _fieldOfStudyRepository.GetAll();

            foreach (var studyProgram in studyProgramsCsv)
            {
                if (fieldOfStudies.Any(f => f.Name == studyProgram.FieldOfStudy))
                    continue;

                var newFieldsOfStudy = new FieldOfStudy()
                {
                    Name = studyProgram.FieldOfStudy
                };
                _fieldOfStudyRepository.Add(newFieldsOfStudy);
                fieldOfStudies.Add(newFieldsOfStudy);
            }

            return fieldOfStudies.ToList();
        }

        private void CreateUniqueStudyProgramsForFieldOfStudy(List<FieldOfStudy> fieldsOfStudy, List<StudyProgramCsv> studyProgramsCsv)
        {
            var tempStudyProgramsCsv = new List<StudyProgramCsv>();

            foreach (var studyProgram in studyProgramsCsv)
            {
                if (!tempStudyProgramsCsv.Exists(s =>
                        s.EnrollmentYear == studyProgram.EnrollmentYear &&
                        s.StudyForm == studyProgram.StudyForm))
                {
                    tempStudyProgramsCsv.Add(studyProgram);

                    var dbStudyProgram = GetDbStudyProgram(fieldsOfStudy, studyProgram);
                }
            }
        }

        private StudyProgram GetDbStudyProgram(List<FieldOfStudy> fieldsOfStudy, StudyProgramCsv studyProgram)
        {
            var newStudyProgram = ParseEnrollmentYear(studyProgram.EnrollmentYear);
            newStudyProgram.StudyForm = EnumHelper.ParseEnumFromDescriptionOrDefault(studyProgram.StudyForm, StudyForms.Unknown);
            newStudyProgram.FieldOfStudyId = fieldsOfStudy.First(f => f.Name == studyProgram.FieldOfStudy).Id;

            var existingStudyProgram = _studyProgramRepository.GetExistingStudyProgramWithCourses(newStudyProgram);

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
    }
}