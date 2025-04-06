using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
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

        public CourseDetailsService(
            ICourseDetailsRepository courseDetailsRepo,
            IInstructorRepository instructorRepo,
            ICourseGroupRepository courseGroupRepo)
        {
            _courseDetailsRepo = courseDetailsRepo;
            _instructorRepo = instructorRepo;
            _courseGroupRepo = courseGroupRepo;
        }

        public async Task<Result> UpdateFromCsv(List<CourseDetailsCsv> courseDetailsCsvList)
        {
            throw new NotImplementedException();

            var courseDetailsIds = courseDetailsCsvList.Select(c => c.CourseDetailsId).ToHashSet();
            var allCourseDetails = await _courseDetailsRepo.GetByIdsWithIncludesAsync(courseDetailsIds); // Include CourseGroups, Instructors
            var allInstructors = await _instructorRepo.GetAllAsync();
            var allGroups = await _courseGroupRepo.GetAllAsync();

            foreach (var row in courseDetailsCsvList)
            {
                var courseDetails = allCourseDetails.FirstOrDefault(cd => cd.Id == row.CourseDetailsId);
                if (courseDetails == null)
                    continue;

                UpdateInstructors(courseDetails, row, allInstructors);
                UpdateGroups(courseDetails, row, allGroups);
            }

            await _courseDetailsRepo.SaveChangesAsync();
            return Result.Success("Course details successfully updated from CSV.");
        }

        private void UpdateInstructors(CourseDetails courseDetails, CourseDetailsCsv row, ICollection<Instructor> allInstructors)
        {
            var idsFromCsv = row.Instructors?.Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(idStr => int.TryParse(idStr, out var id) ? id : (int?)null)
                .Where(id => id != null)
                .Select(id => id!.Value)
                .ToHashSet() ?? new HashSet<int>();

            var instructorsToAssign = allInstructors.Where(i => idsFromCsv.Contains(i.Id)).ToList();

            courseDetails.Instructors.Clear();
            foreach (var instructor in instructorsToAssign)
            {
                courseDetails.Instructors.Add(instructor);
            }
        }

        private void UpdateGroups(CourseDetails courseDetails, CourseDetailsCsv row, ICollection<CourseGroup> allGroups)
        {
            var groupNamesFromCsv = row.GroupsName?.Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Trim())
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>();

            // znajdź grupy, które mają taki CourseType i nazwę
            var matchingGroups = allGroups
                .Where(g => g.CourseType == courseDetails.CourseType && groupNamesFromCsv.Contains(g.GroupName))
                .ToList();

            // dodaj nowe grupy jeśli nie istnieją
            foreach (var name in groupNamesFromCsv)
            {
                if (!matchingGroups.Any(g => g.GroupName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    var newGroup = new CourseGroup
                    {
                        GroupName = name,
                        CourseType = courseDetails.CourseType
                    };

                    _courseGroupRepo.Add(newGroup);
                    matchingGroups.Add(newGroup);
                }
            }

            // aktualizacja przypisanych grup
            courseDetails.CourseGroups.Clear();
            foreach (var group in matchingGroups)
            {
                courseDetails.CourseGroups.Add(group);
            }
        }
    }
}