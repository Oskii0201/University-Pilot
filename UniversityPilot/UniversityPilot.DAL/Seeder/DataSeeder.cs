using Microsoft.AspNetCore.Identity;
using UniversityPilot.DAL.Areas.Identity.Models;

namespace UniversityPilot.DAL.Seeder
{
    public class DataSeeder
    {
        private readonly UniversityPilotContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public DataSeeder(UniversityPilotContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public void SeedDatabase()
        {
            if (_context.Database.CanConnect())
            {
                if (!_context.Roles.Any())
                {
                    var roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }

                if (!_context.Users.Any())
                {
                    var users = GetUsers();
                    _context.Users.AddRange(users);
                    _context.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            return new List<Role>()
            {
                new Role()
                {
                    Name = "Admin"
                },
                new Role()
                {
                    Name = "User"
                }
            };
        }

        private IEnumerable<User> GetUsers()
        {
            var admin = new User()
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                EmailIsConfirmed = true,
                RoleId = 1,
            };
            admin.PasswordHash = _passwordHasher.HashPassword(admin, "Admin123!");

            var user = new User()
            {
                FirstName = "User",
                LastName = "User",
                Email = "user@user.com",
                EmailIsConfirmed = true,
                RoleId = 2,
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, "User123!");

            var users = new List<User>() { admin, user };

            return users;
        }
    }
}