using Microsoft.AspNetCore.Http;
using UniversityPilot.BLL.Areas.Files.DTO;
using UniversityPilot.BLL.Areas.Files.Interfaces;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Processing.Services;
using UniversityPilot.BLL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.Shared.Utilities;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;

namespace UniversityPilot.BLL.Areas.Files.Services
{
    internal class CsvService : ICsvService
    {
        private readonly IClassroomService _classroomService;
        private readonly IInstructorService _instructorService;
        private readonly IStudyProgramService _studyProgramService;
        private readonly IHolidayService _holidayService;
        private readonly ICourseDetailsRepository _courseDetailsRepository;

        public CsvService(
            IClassroomService classroomService,
            IInstructorService instructorService,
            IStudyProgramService studyProgramService,
            IHolidayService holidayService,
            ICourseDetailsRepository courseDetailsRepository)
        {
            _classroomService = classroomService;
            _instructorService = instructorService;
            _studyProgramService = studyProgramService;
            _holidayService = holidayService;
            _courseDetailsRepository = courseDetailsRepository;
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

                case FileType.Holidays:
                    var holidaysCsv = ReadCsvFileToObject<HolidaysCsv>(data.File);
                    return await _holidayService.SaveFromCsv(holidaysCsv);

                case FileType.Instructors:
                    var instructorsCsv = ReadCsvFileToObject<InstructorCsv>(data.File);
                    return await _instructorService.SaveFromCsv(instructorsCsv);

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

        public async Task<string> GetCourseDetailsExport(int semesterId)
        {
            var courseDetails = await _courseDetailsRepository.GetCourseDetailsExport(semesterId);
            var courseDetailsCsv = courseDetails.Select(cd =>
                    new CourseDetailsCsv
                    {
                        CourseDetailsId = cd.Id,
                        CourseType = EnumHelper.GetEnumDescription(cd.CourseType),
                        CourseName = cd.Course.Name,
                        Instructors = string.Join("|", cd.Instructors.Select(i => i.Id)),
                        CourseGroups = string.Join("|", cd.CourseGroups.Select(g => g.Id)),
                        GroupsName = string.Join("|", cd.CourseGroups.Select(g => g.GroupName))
                    }).ToList();

            return CsvBuilder.Build(courseDetailsCsv);
        }
    }
}