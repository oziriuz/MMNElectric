using System.Collections.Generic;

namespace MMNElectric
{
    public class SQLTableModel
    {
        public string TableName
        {
            get; set;
        }

        public int NumberOfColumns
        {
            get; set;
        }

        public List<SQLColumnModel> TableColumns { get; set; } = new List<SQLColumnModel>();

        public string PrimaryKey
        {
            get; set;
        }

    }
}
