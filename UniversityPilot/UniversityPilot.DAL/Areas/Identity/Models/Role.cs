using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.Identity.Models
{
    public class Role : IModelBase
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}