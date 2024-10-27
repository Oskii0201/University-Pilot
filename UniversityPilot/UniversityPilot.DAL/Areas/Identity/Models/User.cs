using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.Identity.Models
{
    public class User : IModelBase
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailIsConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }

        public int RoleID { get; set; }
        public virtual Role Role { get; set; }
    }
}