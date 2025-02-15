using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Repositories
{
    internal class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}