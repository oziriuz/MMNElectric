
namespace MMNElectric
{
    public class StructurePota3
    {
        public const string tableName = "plc_data_pota_3";

        public struct TableColumns
        {
            public const string id = "id"; //unique
            public const string dateStamp = "date_stamp";
            public const string date = "date";
            public const string time = "time";
            public const string pota3T = "pota3_t";
            public const string pota3Rpm = "pota3_rpm";
        }

        public struct ColumnsToCreate
        {
            public static readonly string[] colNameVar =
        {
            $"{TableColumns.id} bigserial",
            $"{TableColumns.dateStamp} date",
            $"{TableColumns.date} text",
            $"{TableColumns.time} text",
            $"{TableColumns.pota3T} text",
            $"{TableColumns.pota3Rpm} text"
        };
        }
    }
}
