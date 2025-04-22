using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Instructor>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Instructors
                .Where(i => ids.Contains(i.Id))
                .ToListAsync();
        }
    }
}