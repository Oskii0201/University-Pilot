using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.UniversityComponents.Interfaces;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.DAL.Areas.UniversityComponents.Repositories
{
    internal class ClassroomRepository : Repository<Classroom>, IClassroomRepository
    {
        public ClassroomRepository(UniversityPilotContext context) : base(context)
        {
        }
    }
}