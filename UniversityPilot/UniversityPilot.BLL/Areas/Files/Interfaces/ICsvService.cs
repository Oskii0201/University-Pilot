using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.Interfaces
{
    public interface ICsvService
    {
        public Task<Result> UploadAsync(UploadDatasetDto data);

        public Task<string> GetCourseDetailsCsv(int semesterId);

        public Task<string> GetInstructorsCsv();

        public Task<string> GetScheduleGroupsDaysCsv(int semesterId);

        public Task<string> GetClassroomsCsv();

        public Task<string> GetPreliminaryCoursesScheduleCsv(int id);
    }
}