using Microsoft.AspNetCore.Http;
using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Files.Interfaces;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Processing.Services;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.BLL.Areas.Files.Services
{
    internal class CsvService : ICsvService
    {
        private readonly IClassroomService _classroomService;
        private readonly IGroupService _groupService;
        private readonly IHistoricalScheduleService _historicalScheduleService;
        private readonly IInstructorService _instructorService;
        private readonly IStudyProgramService _studyProgramService;

        public CsvService(
            IClassroomService classroomService,
            IGroupService groupService,
            IHistoricalScheduleService historicalScheduleService,
            IInstructorService instructorService,
            IStudyProgramService studyProgramService)
        {
            _classroomService = classroomService;
            _groupService = groupService;
            _historicalScheduleService = historicalScheduleService;
            _instructorService = instructorService;
            _studyProgramService = studyProgramService;
        }

        public async Task<Result> UploadAsync(UploadDatasetDto data)
        {
            try
            {
                if (data.File == null || data.File.Length == 0)
                    return Result.Failure("File is empty.", "File_Empty", new[] { "No file content provided." });

                if (!Path.GetExtension(data.File.FileName).Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
                    return Result.Failure("CSV file contains invalid data.", "Invalid_Extension", new[] { "Bad file extension." });

                return await SaveToDatabase(data);
            }
            catch (Exception ex)
            {
                // TODO: logowanie błędów
                return Result.Failure("An error occurred during CSV processing.", "500", new[] { ex.Message });
            }
        }

        private async Task<Result> SaveToDatabase(UploadDatasetDto data)
        {
            switch (data.Dataset)
            {
                case FileType.StudyProgram:
                    var studyProgramsCsv = ReadCsvFileToObject<StudyProgramCsv>(data.File);
                    return _studyProgramService.SaveFromCsv(studyProgramsCsv);

                case FileType.Classrooms:
                    var classroomsCsv = ReadCsvFileToObject<ClassroomCsv>(data.File);
                    return await _classroomService.SaveFromCsv(classroomsCsv);

                //case "Instructors":
                //    var instructorsCsv = ReadCsvFileToObject<InstructorCsv>(data.File);
                //    return _instructorService.SaveFromCsv(instructorsCsv);

                ////case "HistoricalSchedule":
                ////    var historicalSchedulesCsv = ReadCsvFileToObject<HistoricalScheduleCsv>(data.File);
                ////    return _historicalScheduleService.SaveFromCsv(historicalSchedulesCsv);

                //case "Group":
                //    var groupsCsv = ReadCsvFileToObject<GroupCsv>(data.File);
                //    return _groupService.SaveFromCsv(groupsCsv);

                default:
                    return Result.Failure($"Unsupported file type: {data.Dataset}");
            }
        }

        private static List<T> ReadCsvFileToObject<T>(IFormFile file) where T : new()
        {
            var rows = new List<T>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                bool isFirstRow = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    var columns = line.Split(new[] { ',' }, StringSplitOptions.None);
                    rows.Add(MapCsvRowToObject<T>(columns));
                }
            }

            return rows;
        }

        private static T MapCsvRowToObject<T>(string[] csvData) where T : new()
        {
            T obj = new T();

            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var attribute = (CsvColumnAttribute)Attribute.GetCustomAttribute(prop, typeof(CsvColumnAttribute));
                if (attribute == null)
                    continue;

                int columnIndex = attribute.ColumnIndex;
                if (columnIndex >= csvData.Length)
                    continue;

                string value = csvData[columnIndex];
                switch (Type.GetTypeCode(prop.PropertyType))
                {
                    case TypeCode.Int32:
                        if (string.IsNullOrEmpty(value))
                            prop.SetValue(obj, null);
                        else if (int.TryParse(value, out var intValue))
                            prop.SetValue(obj, intValue);
                        break;

                    case TypeCode.Decimal:
                        if (decimal.TryParse(value, out var decimalValue))
                            prop.SetValue(obj, decimalValue);
                        break;

                    case TypeCode.Double:
                        if (double.TryParse(value, out var doubleValue))
                            prop.SetValue(obj, doubleValue);
                        break;

                    case TypeCode.String:
                        prop.SetValue(obj, value);
                        break;

                    case TypeCode.Boolean:
                        if (bool.TryParse(value, out var boolValue))
                            prop.SetValue(obj, boolValue);
                        break;

                    case TypeCode.DateTime:
                        if (DateTime.TryParse(value, out var dateTimeValue))
                            prop.SetValue(obj, dateTimeValue);
                        break;

                    default:
                        throw new InvalidOperationException($"Unsupported property type: {prop.PropertyType}");
                }
            }

            return obj;
        }
    }
}