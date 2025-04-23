using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System.Text;

namespace UniversityPilot.BLL.Areas.Shared
{
    internal static class CsvHandler
    {
        public static List<T> ReadCsvFileToObject<T>(IFormFile file) where T : new()
        {
            var rows = new List<T>();

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var parser = new TextFieldParser(reader))
            {
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;
                parser.TrimWhiteSpace = true;

                bool isFirstRow = true;
                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();

                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    if (fields != null)
                        rows.Add(MapCsvRowToObject<T>(fields));
                }
            }

            return rows;
        }

        public static T MapCsvRowToObject<T>(string[] csvData) where T : new()
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

        public static string Build<T>(IEnumerable<T> data)
        {
            var sb = new StringBuilder();
            var props = typeof(T).GetProperties();

            sb.AppendLine(string.Join(",", props.Select(p => p.Name)));

            foreach (var item in data)
            {
                var line = string.Join(",", props.Select(p => $"\"{p.GetValue(item)?.ToString()?.Replace("\"", "\"\"")}\""));
                sb.AppendLine(line);
            }

            return sb.ToString();
        }
    }
}