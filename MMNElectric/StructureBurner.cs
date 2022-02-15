using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MMNElectric
{
    public class StructureBurner
    {
        public const string tableName = "plc_data_burner";

        public struct TableColumns
        {
            public const string id = "id"; //unique
            public const string dateStamp = "date_stamp";
            public const string date = "date";
            public const string time = "time";
            public const string burnerExpenseGas = "burner_expense_gas";
            public const string burnerExpenseOx = "burner_expense_ox";
            public const string burnerSumExpenseGas = "burner_sum_expense_gas";
            public const string burnerSumExpenseOx = "burner_sum_expense_ox";
            public const string burnerPressureGas = "burner_pressure_gas";
            public const string burnerPressureOx = "burner_pressure_ox";
            public const string burnerTGas = "burner_t_gas";
            public const string burnerTOx = "burner_t_ox";
        }

        public struct ColumnsToCreate
        {
            public static readonly string[] colNameVar =
        {
            $"{TableColumns.id} bigserial",
            $"{TableColumns.dateStamp} date",
            $"{TableColumns.date} text",
            $"{TableColumns.time} text",
            $"{TableColumns.burnerExpenseGas} text",
            $"{TableColumns.burnerExpenseOx} text",
            $"{TableColumns.burnerSumExpenseGas} text",
            $"{TableColumns.burnerSumExpenseOx} text",
            $"{TableColumns.burnerPressureGas} text",
            $"{TableColumns.burnerPressureOx} text",
            $"{TableColumns.burnerTGas} text",
            $"{TableColumns.burnerTOx} text"

        };
        }
    }
}

