namespace UniversityPilot.DAL.Areas.Identity.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string Type { get; set; }

        public ICollection<User> Users { get; set; }
    }
}