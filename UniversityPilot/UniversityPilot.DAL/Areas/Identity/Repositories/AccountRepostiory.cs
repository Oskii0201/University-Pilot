using Microsoft.EntityFrameworkCore;
using UniversityPilot.DAL.Areas.Identity.Interfaces;
using UniversityPilot.DAL.Areas.Identity.Models;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.Identity.Repositories
{
    internal class AccountRepostiory : Repository<User>, IAccountRepostiory
    {
        public AccountRepostiory(UniversityPilotContext context) : base(context)
        {
        }

        public User? GetByEmail(string email)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email);
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}