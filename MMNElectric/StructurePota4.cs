
namespace MMNElectric
{
    public class StructurePota4
    {
        public const string tableName = "plc_data_pota_4";

        public struct TableColumns
        {
            public const string id = "id"; //unique
            public const string dateStamp = "date_stamp";
            public const string date = "date";
            public const string time = "time";
            public const string pota4T = "pota4_t";
            public const string pota4Rpm = "pota4_rpm";
        }

        public struct ColumnsToCreate
        {
            public static readonly string[] colNameVar =
        {
            $"{TableColumns.id} bigserial",
            $"{TableColumns.dateStamp} date",
            $"{TableColumns.date} text",
            $"{TableColumns.time} text",
            $"{TableColumns.pota4T} text",
            $"{TableColumns.pota4Rpm} text"
        };
        }
    }
}
