using System.Text;

namespace UniversityPilot.BLL.Areas.Shared
{
    public static class CsvBuilder
    {
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