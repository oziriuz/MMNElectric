
namespace MMNElectric
{
    public class StructureFurnace
    {
        public const string tableName = "plc_data_furnace";

        public struct TableColumns
        {
            public const string id = "id"; //unique
            public const string dateStamp = "date_stamp";
            public const string date = "date";
            public const string time = "time";
            public const string furnaceSetpoint = "furnace_setpoint";
            public const string furnaceSpeed = "furnace_speed";
        }

        public struct ColumnsToCreate
        {
            public static readonly string[] colNameVar =
        {
            $"{TableColumns.id} bigserial",
            $"{TableColumns.dateStamp} date",
            $"{TableColumns.date} text",
            $"{TableColumns.time} text",
            $"{TableColumns.furnaceSetpoint} text",
            $"{TableColumns.furnaceSpeed} text"

        };
        }
    }
}
