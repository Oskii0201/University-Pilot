using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.UniversityAndScheduling.Models
{
    public class Holiday : IModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}