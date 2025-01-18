using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Repositories
{
    internal class StudyProgramRepository : Repository<StudyProgram>, IStudyProgramRepository
    {
        public StudyProgramRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}