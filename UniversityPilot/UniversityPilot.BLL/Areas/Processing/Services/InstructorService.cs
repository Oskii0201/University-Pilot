using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.Identity.Models;
using UniversityPilot.DAL.Areas.UniversityComponents.Interfaces;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.BLL.Areas.Processing.Services
{
    internal class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public InstructorService(
            IInstructorRepository instructorRepository,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper)
        {
            _instructorRepository = instructorRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<Result> SaveFromCsv(List<InstructorCsv> csvData)
        {
            if (csvData == null || !csvData.Any())
                return Result.Failure("CSV is empty or invalid.", "EMPTY_CSV");

            var existing = await _instructorRepository.GetAllAsync();

            var existingKeys = existing
                .Select(x => $"{x.Title.Trim().ToLower()}|{x.FirstName.Trim().ToLower()}|{x.LastName.Trim().ToLower()}")
                .ToHashSet();

            var instructorsToAdd = csvData
                .Where(x => !string.IsNullOrWhiteSpace(x.FirstName) && !string.IsNullOrWhiteSpace(x.LastName))
                .Select(x => new
                {
                    Key = $"{x.Title.Trim().ToLower()}|{x.FirstName.Trim().ToLower()}|{x.LastName.Trim().ToLower()}",
                    x.Title,
                    x.FirstName,
                    x.LastName
                })
                .Where(x => !existingKeys.Contains(x.Key))
                .Select(x =>
                {
                    var email = $"{x.FirstName.ToLowerInvariant()}.{x.LastName.ToLowerInvariant()}@example.uczelnia.pl";

                    var instructor = new Instructor
                    {
                        FirstName = x.FirstName.Trim(),
                        LastName = x.LastName.Trim(),
                        Title = x.Title.Trim(),
                        Email = email,
                        EmailIsConfirmed = false,
                        RoleId = 3, // rola "Instructor"
                        ContractType = "Umowa o pracę"
                    };

                    instructor.PasswordHash = _passwordHasher.HashPassword(instructor, "Password123!");

                    return instructor;
                })
                .ToList();

            if (!instructorsToAdd.Any())
                return Result.Failure("All instructors from the file already exist in the system.", "ALL_DUPLICATES");

            foreach (var instructor in instructorsToAdd)
            {
                await _instructorRepository.AddAsync(instructor);
            }

            return Result.Success($"{instructorsToAdd.Count()} new instructors imported successfully.");
        }

        public async Task<List<InstructorCsv>> GetAllInstructorsCsv()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            return _mapper.Map<List<InstructorCsv>>(instructors.ToList());
        }
    }
}