using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Files.Interfaces;
using UniversityPilot.BLL.Areas.Shared;

namespace UniversityPilot.BLL.Areas.Files.Services
{
    internal class CsvService : ICsvService
    {
        public async Task<Result> UploadCsvAsync(UploadDatasetDto data)
        {
            try
            {
                if (data.File == null || data.File.Length == 0)
                    return Result.Failure("File is empty.", "File_Empty", new[] { "No file content provided." });

                if (!Path.GetExtension(data.File.FileName).Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
                    return Result.Failure("CSV file contains invalid data.", "Invalid_Extension", new[] { "Bad file extension." });

                if (!await SaveToDatabase(data))
                    return Result.Failure("Failed to save data to the database.", "Database_Error");

                return Result.Success("CSV file processed and data saved successfully.");
            }
            catch (Exception ex)
            {
                // TODO: logowanie błędów
                return Result.Failure("An error occurred during CSV processing.", "500", new[] { ex.Message });
            }
        }

        private async Task<bool> SaveToDatabase(UploadDatasetDto data)
        {
            throw new NotImplementedException();
        }
    }
}