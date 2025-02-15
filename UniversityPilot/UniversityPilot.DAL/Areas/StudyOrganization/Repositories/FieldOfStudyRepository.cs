using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Repositories
{
    internal class FieldOfStudyRepository : Repository<FieldOfStudy>, IFieldOfStudyRepository
    {
        public FieldOfStudyRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}