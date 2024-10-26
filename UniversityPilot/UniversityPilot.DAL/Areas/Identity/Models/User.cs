using UniversityPilot.DAL.Areas.Students.Models;
using UniversityPilot.DAL.Areas.UniversityAndScheduling.Models;

namespace UniversityPilot.DAL.Areas.Identity.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
        public Instructor Instructor { get; set; }
        public Student Student { get; set; }
    }
}