using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.Shared.Utilities;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;
using UniversityPilot.DAL.Areas.UniversityComponents.Interfaces;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.BLL.Areas.Processing.Services
{
    internal class CourseDetailsService : ICourseDetailsService
    {
        private readonly ICourseDetailsRepository _courseDetailsRepo;
        private readonly IInstructorRepository _instructorRepo;
        private readonly ICourseGroupRepository _courseGroupRepo;
        private readonly ISharedCourseGroupRepository _sharedCourseGroupRepository;

        public CourseDetailsService(
            ICourseDetailsRepository courseDetailsRepo,
            IInstructorRepository instructorRepo,
            ICourseGroupRepository courseGroupRepo,
            ISharedCourseGroupRepository sharedCourseGroupRepository)
        {
            _courseDetailsRepo = courseDetailsRepo;
            _instructorRepo = instructorRepo;
            _courseGroupRepo = courseGroupRepo;
            _sharedCourseGroupRepository = sharedCourseGroupRepository;
        }

        public async Task<Result> UpdateFromCsv(List<CourseDetailsCsv> courseDetailsCsvList)
        {
            var courseDetails = await LoadCourseDetailsAsync(courseDetailsCsvList);
            var allInstructors = await LoadInstructorsAsync(courseDetailsCsvList);
            var allGroups = await LoadCourseGroupsAsync(courseDetailsCsvList);

            foreach (var csv in courseDetailsCsvList)
            {
                var course = courseDetails.FirstOrDefault(cd => cd.Id == csv.CourseDetailsId);
                if (course == null) continue;

                course.Online = csv.Online?.Trim().ToLower() == "tak";

                await UpdateCourseGroupsAsync(course.Id, csv, allGroups);
                await UpdateInstructorsAsync(course.Id, csv, allInstructors);
            }

            foreach (var course in courseDetails)
            {
                await _courseDetailsRepo.DetachNavigationPropertiesAsync(course);
                await _courseDetailsRepo.UpdateAsync(course);
            }

            foreach (var csv in courseDetailsCsvList)
            {
                var course = courseDetails.FirstOrDefault(cd => cd.Id == csv.CourseDetailsId);
                if (course == null) continue;

                await UpdateSharedGroupAssignment(course, csv, courseDetails);
                await _courseDetailsRepo.UpdateAsync(course);
            }

            return Result.Success($"{courseDetailsCsvList.Count} course details updated.");
        }

        private async Task UpdateCourseGroupsAsync(int courseDetailsId, CourseDetailsCsv csv, ICollection<CourseGroup> allGroups)
        {
            if (string.IsNullOrWhiteSpace(csv.GroupsName)) return;

            var groupNames = csv.GroupsName
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct()
                .ToList();

            var courseType = EnumHelper.ParseEnumFromDescriptionOrDefault<CourseTypes>(csv.CourseType, CourseTypes.Lecture);
            var matchedGroups = new List<CourseGroup>();

            foreach (var name in groupNames)
            {
                var existingGroup = allGroups.FirstOrDefault(g => g.GroupName == name && g.CourseType == courseType);

                if (existingGroup != null)
                {
                    matchedGroups.Add(existingGroup);
                }
                else
                {
                    var newGroup = new CourseGroup { GroupName = name, CourseType = courseType };
                    _courseGroupRepo.Add(newGroup);
                    matchedGroups.Add(newGroup);
                    allGroups.Add(newGroup);
                }
            }

            await _courseGroupRepo.SaveChangesAsync();

            await _courseDetailsRepo.UnassignCourseGroupsAsync(courseDetailsId);
            foreach (var group in matchedGroups)
            {
                await _courseDetailsRepo.AssignCourseGroupAsync(courseDetailsId, group.Id);
            }
        }

        private async Task UpdateInstructorsAsync(int courseDetailsId, CourseDetailsCsv csv, ICollection<Instructor> allInstructors)
        {
            if (string.IsNullOrWhiteSpace(csv.Instructors)) return;

            var ids = csv.Instructors.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var matched = allInstructors.Where(i => ids.Contains(i.Id)).Select(i => i.Id).ToList();

            await _courseDetailsRepo.UnassignInstructorsAsync(courseDetailsId);
            foreach (var instructorId in matched)
                await _courseDetailsRepo.AssignInstructorAsync(courseDetailsId, instructorId);
        }

        private async Task UpdateSharedGroupAssignment(CourseDetails course, CourseDetailsCsv csv, ICollection<CourseDetails> allCourseDetails)
        {
            if (string.IsNullOrWhiteSpace(csv.SharedCourseGroup))
                return;

            var courseType = EnumHelper.ParseEnumFromDescriptionOrDefault<CourseTypes>(csv.CourseType, course.CourseType);
            var courseName = csv.CourseName;

            var existingGroup = await _sharedCourseGroupRepository.GetByNameWithCourseDetailsAsync(csv.SharedCourseGroup, courseType, courseName);
            if (existingGroup != null)
            {
                course.SharedCourseGroupId = existingGroup.Id;
                return;
            }

            var newGroup = new SharedCourseGroup { Name = csv.SharedCourseGroup };
            var created = await _sharedCourseGroupRepository.AddAsync(newGroup);
            course.SharedCourseGroupId = created.Id;
        }

        private async Task<List<CourseDetails>> LoadCourseDetailsAsync(List<CourseDetailsCsv> csvList)
        {
            var ids = csvList.Select(x => x.CourseDetailsId).Distinct().ToList();
            return await _courseDetailsRepo.GetByIdsAsync(ids);
        }

        private async Task<List<Instructor>> LoadInstructorsAsync(List<CourseDetailsCsv> csvList)
        {
            var ids = csvList
                .Where(x => !string.IsNullOrWhiteSpace(x.Instructors))
                .SelectMany(x => x.Instructors.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(int.Parse)
                .Distinct()
                .ToList();

            return await _instructorRepo.GetByIdsAsync(ids);
        }

        private async Task<List<CourseGroup>> LoadCourseGroupsAsync(List<CourseDetailsCsv> csvList)
        {
            var descriptors = csvList
                .Where(x => !string.IsNullOrWhiteSpace(x.GroupsName))
                .SelectMany(csv =>
                {
                    var type = EnumHelper.ParseEnumFromDescriptionOrDefault<CourseTypes>(csv.CourseType, CourseTypes.Lecture);
                    return csv.GroupsName
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(name => (name, type));
                })
                .Distinct()
                .ToList();

            return await _courseGroupRepo.GetByNamesAndTypesAsync(descriptors);
        }
    }
}