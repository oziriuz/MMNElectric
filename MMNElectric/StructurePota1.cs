
namespace MMNElectric
{
    public class StructurePota1
    {
        public const string tableName = "plc_data_pota_1";

        public struct TableColumns
        {
            public const string id = "id"; //unique
            public const string dateStamp = "date_stamp";
            public const string date = "date";
            public const string time = "time";
            public const string pota1T = "pota1_t";
            public const string pota1Rpm = "pota1_rpm";
        }

        public struct ColumnsToCreate
        {
            public static readonly string[] colNameVar =
        {
            $"{TableColumns.id} bigserial",
            $"{TableColumns.dateStamp} date",
            $"{TableColumns.date} text",
            $"{TableColumns.time} text",
            $"{TableColumns.pota1T} text",
            $"{TableColumns.pota1Rpm} text"
        };
        }
    }
}
