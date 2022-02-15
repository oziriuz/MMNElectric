
namespace MMNElectric
{
    public class StructureFilter
    {
        public const string tableName = "plc_data_filter";

        public struct TableColumns
        {
            public const string id = "id"; //unique
            public const string dateStamp = "date_stamp";
            public const string date = "date";
            public const string time = "time";
            public const string filterTemp = "filter_temp";
            public const string filterLoad = "filter_load";
        }

        public struct ColumnsToCreate
        {
            public static readonly string[] colNameVar =
        {
            $"{TableColumns.id} bigserial",
            $"{TableColumns.dateStamp} date",
            $"{TableColumns.date} text",
            $"{TableColumns.time} text",
            $"{TableColumns.filterTemp} text",
            $"{TableColumns.filterLoad} text"
        };
        }
    }
}
