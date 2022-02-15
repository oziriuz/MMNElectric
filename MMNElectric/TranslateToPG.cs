namespace MMNElectric
{
    public class TranslateToPG
    {
        public static string StrCheckTableFromSchema(string tableName)
        {
            string output = $"SELECT table_name FROM information_schema.tables WHERE table_name = '{ tableName }'";

            return output;
        }

        public static string StrCheckColumnFromSchema(string tableName, string column)
        {
            string output = $"SELECT column_name FROM information_schema.columns WHERE table_name = '{ tableName }' AND column_name = '{ column }'";

            return output;
        }

        public static string StrCreateTableStr(string tableName)
        {
            string output = $"CREATE TABLE public.{ tableName } () WITH (OIDS = FALSE, autovacuum_enabled = true); ALTER TABLE public.{ tableName } OWNER TO postgres;";

            return output;
        }

        public static string StrAddColumnToTable(string tableName, string column, string dataType)
        {
            string output = $"ALTER TABLE { tableName } ADD COLUMN { column } { dataType };";

            return output;
        }

        public static string StrAlterColumnPrimaryKey(string tableName, string primaryKey)
        {
            string output = $"ALTER TABLE { tableName } ADD CONSTRAINT { tableName }_pkey PRIMARY KEY({ primaryKey });";

            return output;
        }

        public static string StrAlterColumnUniqueKey(string tableName, string uniqueKey)
        {
            string output = $"ALTER TABLE { tableName } ADD CONSTRAINT { tableName }_{ uniqueKey }_key UNIQUE ({ uniqueKey });";

            return output;
        }

        public static string StrSelectFromTable(string table, string orderBy, string order)
        {
            string output = $"SELECT * FROM { table } ORDER BY { orderBy } { order };";

            return output;
        } // in use


        public static string StrSelectFromTable(string range, string table, string orderBy, string order)
        {
            string output = $"SELECT { range } FROM { table } ORDER BY { orderBy } { order };";

            return output;
        } //in use

        //public static string StrSelectFromTable(string range, string table, string whereArg, string whereVal)
        //{
        //    string output = $"SELECT { range } FROM { table } WHERE { whereArg } = '{ whereVal }';";

        //    return output;
        //}

        public static string StrSelectFromTable(string table, string whereArg, string whereVal, string orderBy, string order)
        {
            string output = $"SELECT * FROM { table } WHERE { whereArg } = '{ whereVal }' ORDER BY { orderBy } { order };";

            return output;
        } //in use

        public static string StrSelectFromTable(string range, string table, string whereArg, string whereVal, string orderBy, string order = "ASC")
        {
            string output = $"SELECT { range } FROM { table } WHERE { whereArg } = '{ whereVal }' ORDER BY { orderBy } { order };";

            return output;
        }

    }
}
