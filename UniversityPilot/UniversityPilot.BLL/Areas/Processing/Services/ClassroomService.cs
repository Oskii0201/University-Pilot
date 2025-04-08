using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.UniversityComponents.Interfaces;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.BLL.Areas.Processing.Services
{
    internal class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;

        public ClassroomService(IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
        }

        public async Task<Result> SaveFromCsv(List<ClassroomCsv> csvData)
        {
            if (csvData == null || !csvData.Any())
            {
                return Result.Failure("CSV is empty or invalid.", "EMPTY_CSV");
            }

            var existingClassroomNames = (await _classroomRepository.GetAllAsync())
                .Select(c => c.RoomNumber.Trim().ToLower())
                .ToHashSet();

            var classroomsToAdd = csvData
                .Where(x => !string.IsNullOrWhiteSpace(x.Number))
                .Select(x => x.Number.Trim())
                .Where(name => !existingClassroomNames.Contains(name.ToLower()))
                .Select(name => new Classroom { RoomNumber = name })
                .ToList();

            if (!classroomsToAdd.Any())
            {
                return Result.Failure("All classrooms from the file already exist in the system.", "ALL_DUPLICATES");
            }

            foreach (var classroom in classroomsToAdd)
            {
                await _classroomRepository.AddAsync(classroom);
            }
            return Result.Success($"{classroomsToAdd.Count} new classrooms imported successfully.");
        }
    }
}