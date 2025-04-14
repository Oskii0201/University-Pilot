using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces
{
    public interface ISharedCourseGroupRepository : IRepository<SharedCourseGroup>
    {
        public Task<SharedCourseGroup?> GetByNameWithCourseDetailsAsync(string name, CourseTypes courseTypes, string courseName);
    }
}