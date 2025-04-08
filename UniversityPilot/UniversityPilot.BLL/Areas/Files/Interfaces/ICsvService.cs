using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.Interfaces
{
    public interface ICsvService
    {
        public Task<Result> UploadAsync(UploadDatasetDto data);

        public Task<string> GetCourseDetailsCsv(int semesterId);

        public Task<string> GetScheduleGroupsDaysCsv(int id);

        public Task<string> GetClassroomsCsv(int id);

        public Task<string> GetPreliminaryCoursesScheduleCsv(int id);
    }
}