namespace MMNElectric
{
    public class SQLColumnModel
    {
        public string ColumnName
        {
            get; set;
        }

        public string ColumnDataType
        {
            get; set;
        }

        public bool IsPrimary
        {
            get; set;
        }

        public bool IsUnique
        {
            get; set;
        }

    }
}
