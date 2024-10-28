using UniversityPilot.DAL.Areas.Identity.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.Identity.Interfaces
{
    public interface IAccountRepostiory : IRepository<User>
    {
        public User? GetByEmail(string email);

        public List<Role> GetRoles();
    }
}