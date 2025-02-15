namespace UniversityPilot.BLL.Areas.Shared
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CsvColumnAttribute : Attribute
    {
        public int ColumnIndex { get; }
        public string ColumnName { get; }

        public CsvColumnAttribute(int columnIndex, string columnName)
        {
            ColumnIndex = columnIndex;
            ColumnName = columnName;
        }
    }
}