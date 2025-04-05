namespace UniversityPilot.BLL.Areas.Schedule
{
    internal static class Utilities
    {
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from; day <= to; day = day.AddDays(1))
                yield return day;
        }
    }
}