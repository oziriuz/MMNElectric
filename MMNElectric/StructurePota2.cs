
namespace MMNElectric
{
    public class StructurePota2
    {
        public const string tableName = "plc_data_pota_2";

        public struct TableColumns
        {
            public const string id = "id"; //unique
            public const string dateStamp = "date_stamp";
            public const string date = "date";
            public const string time = "time";
            public const string pota2T = "pota2_t";
            public const string pota2Rpm = "pota2_rpm";
        }

        public struct ColumnsToCreate
        {
            public static readonly string[] colNameVar =
        {
            $"{TableColumns.id} bigserial",
            $"{TableColumns.dateStamp} date",
            $"{TableColumns.date} text",
            $"{TableColumns.time} text",
            $"{TableColumns.pota2T} text",
            $"{TableColumns.pota2Rpm} text"
        };
        }
    }
}
