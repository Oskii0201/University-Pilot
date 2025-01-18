using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.BLL.Areas.Processing.Services
{
    internal class StudyProgramService : IStudyProgramService
    {
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;

        public StudyProgramService(IFieldOfStudyRepository fieldOfStudyRepository)
        {
            _fieldOfStudyRepository = fieldOfStudyRepository;
        }

        public Result SaveFromCsv(List<StudyProgramCsv> studyPrograms)
        {
            var FieldsOfStudyToSave = GetListOfUniqueFieldOfStudy(studyPrograms);
            return Result.Success();
        }

        private IEnumerable<FieldOfStudy> GetListOfUniqueFieldOfStudy(List<StudyProgramCsv> studyPrograms)
        {
            var fieldOfStudies = _fieldOfStudyRepository.GetAll();

            foreach (var studyProgram in studyPrograms)
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

            return fieldOfStudies;
        }
    }
}