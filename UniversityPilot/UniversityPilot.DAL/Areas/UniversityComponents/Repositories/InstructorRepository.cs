using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.UniversityComponents.Interfaces;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.DAL.Areas.UniversityComponents.Repositories
{
    internal class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}