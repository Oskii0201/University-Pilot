using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.DAL.Areas.UniversityComponents.Interfaces
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        public Task<List<Instructor>> GetByIdsAsync(IEnumerable<int> ids);
    }
}