using Helpers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace MMNElectric
{
    public class DataAccess
    {
        static readonly string connectionString = Helper.CnnVal("MMNDatabase");
        static NpgsqlDataAdapter adapter;
        static string _strSQL;

        public static bool Check(out string info)
        {
            if(!String.IsNullOrWhiteSpace(connectionString))
                try
                {
                    using(var con = new NpgsqlConnection(connectionString))
                    {
                        if(con != null)
                        {
                            if(!DataAccess.HaveTable("information_schema"))
                            {
                                info = "No connection to database";
                                return false;
                            }
                            else
                            {
                                info = "Connection to database working";
                                return true;
                            }
                        }
                        else
                        {
                            info = "No connection to database";
                            return false;
                        }
                    }
                }
                catch(Exception)
                {
                    throw;
                }
            else
                throw new ArgumentNullException();
        }

        public static bool Check()
        {
            return Check(out _);
        }

        public static string CheckAndCreateStructure()
        {
            string info;

            try
            {
                PopulateDatabaseStructure DatabaseStructure = new PopulateDatabaseStructure();

                List<SQLTableModel> SQLTables = new List<SQLTableModel>(DatabaseStructure.GetAllSQLTables());

                foreach(var item in SQLTables)
                {
                    if(!DataAccess.HaveTable(item.TableName, out info))
                    {
                        DataAccess.CreateTable(item.TableName, out info);

                        foreach(var col in item.TableColumns)
                        {
                            if(!DataAccess.HaveColumn(item.TableName, col.ColumnName, out info))
                            {
                                DataAccess.CreateColumn(item.TableName, col.ColumnName, col.ColumnDataType, out info);
                                //make every id column primary and unique
                                if(col.ColumnName == "id")
                                {
                                    col.IsPrimary = true;
                                    col.IsUnique = true;
                                }
                                if(col.IsPrimary)
                                    DataAccess.AlterPrimaryKey(item.TableName, col.ColumnName);

                                if(col.IsUnique)
                                    DataAccess.AlterUniqueKey(item.TableName, col.ColumnName);
                            }
                            else
                            {
                                //show info maybe
                            }
                        }
                    }
                    else
                    {
                        foreach(var col in item.TableColumns)
                        {
                            if(!DataAccess.HaveColumn(item.TableName, col.ColumnName, out info))
                            {
                                DataAccess.CreateColumn(item.TableName, col.ColumnName, col.ColumnDataType, out info);

                                if(col.IsPrimary)
                                    DataAccess.AlterPrimaryKey(item.TableName, col.ColumnName);

                                if(col.IsUnique)
                                    DataAccess.AlterUniqueKey(item.TableName, col.ColumnName);
                            }
                            else
                            {
                                //show info maybe
                            }
                        }
                    }
                }

                info = "Database structure checked and updated";

                return info;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public static bool HaveTable(string tableName, out string info)
        {
            string _strSQL;

            if(!String.IsNullOrWhiteSpace(tableName))
                _strSQL = TranslateToPG.StrCheckTableFromSchema(tableName);
            else
                throw new ArgumentNullException();

            try
            {
                using(var con = new NpgsqlConnection(connectionString))
                {
                    using(var cmd = new NpgsqlCommand(_strSQL))
                    {
                        if(cmd.Connection == null)
                            cmd.Connection = con;
                        if(cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        lock(cmd)
                        {
                            using(NpgsqlDataReader rdr = cmd.ExecuteReader())
                            {
                                try
                                {
                                    if(rdr != null && rdr.HasRows)
                                    {
                                        info = $"Table '{tableName}' present";
                                        return true;
                                    }
                                    info = $"Table '{tableName}' not found";
                                    return false;
                                }
                                catch(Exception)
                                {
                                    info = $"Table '{tableName}' not found";
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public static bool HaveTable(string tableName)
        {
            return HaveTable(tableName, out _);
        }

        public static void CreateTable(string tableName, out string info)
        {
            string _strSQL;

            if(!String.IsNullOrWhiteSpace(tableName))
                _strSQL = TranslateToPG.StrCreateTableStr(tableName);
            else
                throw new ArgumentNullException();

            try
            {
                using(var con = new NpgsqlConnection(connectionString))
                {
                    using(var cmd = new NpgsqlCommand(_strSQL))
                    {
                        if(cmd.Connection == null)
                            cmd.Connection = con;
                        if(cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        info = $"Table '{tableName}' created";
                    }
                }

            }
            catch(Exception)
            {
                throw;
            }
        }

        public static void CreateTable(string tableName)
        {
            CreateTable(tableName, out _);
        }

        public static bool HaveColumn(string tableName, string columnName, out string info)
        {
            string _strSQL;

            if(!String.IsNullOrWhiteSpace(tableName) && !String.IsNullOrWhiteSpace(columnName))
                _strSQL = TranslateToPG.StrCheckColumnFromSchema(tableName, columnName);
            else
                throw new ArgumentNullException();

            try
            {
                using(var con = new NpgsqlConnection(connectionString))
                {
                    using(var cmd = new NpgsqlCommand(_strSQL))
                    {
                        if(cmd.Connection == null)
                            cmd.Connection = con;
                        if(cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        lock(cmd)
                        {
                            using(NpgsqlDataReader rdr = cmd.ExecuteReader())
                            {
                                try
                                {
                                    if(rdr != null && rdr.HasRows)
                                    {
                                        info = $"Column '{columnName}' present";
                                        return true;
                                    }
                                    info = $"Column '{columnName}' not found";
                                    return false;
                                }
                                catch(Exception)
                                {
                                    info = $"Column '{columnName}' not found";
                                    return false;
                                }
                            }
                        }
                    }
                }

            }
            catch(Exception)
            {
                throw;
            }
        }

        public static bool HaveColumn(string tableName, string columnName)
        {
            return HaveColumn(tableName, columnName, out _);
        }

        public static void CreateColumn(string tableName, string columnName, string dataType, out string info)
        {
            string _strSQL;

            if(!String.IsNullOrWhiteSpace(tableName) && !String.IsNullOrWhiteSpace(columnName) && !String.IsNullOrWhiteSpace(dataType))
                _strSQL = TranslateToPG.StrAddColumnToTable(tableName, columnName, dataType);
            else
                throw new ArgumentNullException();

            try
            {
                using(var con = new NpgsqlConnection(connectionString))
                {
                    using(var cmd = new NpgsqlCommand(_strSQL))
                    {
                        if(cmd.Connection == null)
                            cmd.Connection = con;
                        if(cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        info = $"Column '{ columnName }' in '{ tableName }' created";
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public static void CreateColumn(string tableName, string columnName, string dataType)
        {
            CreateColumn(tableName, columnName, dataType, out _);
        }

        public static void AlterPrimaryKey(string tableName, string columnName)
        {
            string _strSQL;

            if(!String.IsNullOrWhiteSpace(tableName) && !String.IsNullOrWhiteSpace(columnName))
                _strSQL = TranslateToPG.StrAlterColumnPrimaryKey(tableName, columnName);
            else
                throw new ArgumentNullException();

            try
            {
                using(var con = new NpgsqlConnection(connectionString))
                {
                    using(var cmd = new NpgsqlCommand(_strSQL))
                    {
                        if(cmd.Connection == null)
                            cmd.Connection = con;
                        if(cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch(Exception)
            {
                throw;
            }
        }

        public static void AlterUniqueKey(string tableName, string columnName)
        {
            string alterColumn;

            if(!String.IsNullOrWhiteSpace(tableName) && !String.IsNullOrWhiteSpace(columnName))
                alterColumn = TranslateToPG.StrAlterColumnUniqueKey(tableName, columnName);
            else
                throw new ArgumentNullException();

            try
            {
                using(var con = new NpgsqlConnection(connectionString))
                {
                    using(var cmd = new NpgsqlCommand(alterColumn))
                    {
                        if(cmd.Connection == null)
                            cmd.Connection = con;
                        if(cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch(Exception)
            {
                throw;
            }
        }

        public static DataTable LoadFromDB(string tableOrString, string code, string columnDate, string startDate, string endDate)
        {
            if(startDate == "")
            {
                startDate = DateTime.Now.AddDays(-31).ToString(Helper.dateStampPattern);
            }

            if(endDate == "")
            {
                endDate = DateTime.Now.ToString(Helper.dateStampPattern);
            }

            DataTable _table = new DataTable();

            _strSQL = $@"SELECT * FROM {tableOrString} 
                    WHERE {columnDate} >= '{startDate}' 
                    AND {columnDate} <= '{endDate}'
                    ORDER BY {code} DESC;";

            try
            {
                using(var con = new NpgsqlConnection(connectionString))
                {
                    using(var cmd = new NpgsqlCommand(_strSQL))
                    {
                        if(cmd.Connection == null)
                            cmd.Connection = con;
                        if(cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        //cmd.ExecuteNonQuery();
                        adapter = new NpgsqlDataAdapter(_strSQL, con);
                        adapter.Fill(_table);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Source}\n\n{ex.Message + ex.ToString()}", $"Cannot connect!", MessageBoxButton.OK);

                throw;
            }

            finally
            {
                adapter.Dispose();
            }
            return _table;
        }

        public static bool SaveToDB(string tableName, string columnDateStamp, string columnDate, string columnTime, 
            string columnData1, string valueData1,
            string columnData2 = null, string valueData2 = null,
            string columnData3 = null, string valueData3 = null,
            string columnData4 = null, string valueData4 = null,
            string columnData5 = null, string valueData5 = null, 
            string columnData6 = null, string valueData6 = null,
            string columnData7 = null, string valueData7 = null,
            string columnData8 = null, string valueData8 = null,
            string columnData9 = null, string valueData9 = null,
            string columnData10 = null, string valueData10 = null)
        {
            string dateStamp = DateTime.Now.ToString(Helper.dateStampPattern);
            string dateSave = DateTime.Now.ToString(Helper.datePattern);
            string timeSave = DateTime.Now.ToString(Helper.timePattern);

            string _strSQL = $"INSERT INTO {tableName} (";
            _strSQL += $"{columnDateStamp}";
            _strSQL += $", {columnDate}";
            _strSQL += $", {columnTime}";
            _strSQL += $", {columnData1}";
            if(columnData2 != null)
                _strSQL += $", {columnData2}";
            if(columnData3 != null)
                _strSQL += $", {columnData3}";
            if (columnData4 != null)
                _strSQL += $", {columnData4}";
            if (columnData5 != null)
                _strSQL += $", {columnData5}";
            if (columnData6 != null)
                _strSQL += $", {columnData6}";
            if (columnData7 != null)
                _strSQL += $", {columnData7}";
            if (columnData8 != null)
                _strSQL += $", {columnData8}";
            if (columnData9 != null)
                _strSQL += $", {columnData9}";
            if (columnData10 != null)
                _strSQL += $", {columnData10}";
            _strSQL += ") ";

            _strSQL += $"VALUES ('{dateStamp}'";
            _strSQL += $", '{dateSave}'";
            _strSQL += $", '{timeSave}'";
            _strSQL += $", '{valueData1}'";
            if(columnData2 != null)
                _strSQL += $", '{valueData2}'";
            if(columnData3 != null)
                _strSQL += $", '{valueData3}'";
            if (columnData4 != null)
                _strSQL += $", '{valueData4}'";
            if (columnData5 != null)
                _strSQL += $", '{valueData5}'";
            if (columnData6 != null)
                _strSQL += $", '{valueData6}'";
            if (columnData7 != null)
                _strSQL += $", '{valueData7}'";
            if (columnData8 != null)
                _strSQL += $", '{valueData8}'";
            if (columnData9 != null)
                _strSQL += $", '{valueData9}'";
            if (columnData10 != null)
                _strSQL += $", '{valueData10}'";
            _strSQL += ") ";

            try
            {
                using(var con = new NpgsqlConnection(connectionString))
                {
                    using(var cmd = new NpgsqlCommand(_strSQL))
                    {
                        if(cmd.Connection == null)
                            cmd.Connection = con;
                        if(cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Source}\n\n{ex.Message + ex.ToString()}", $"Cannot connect!", MessageBoxButton.OK);

                throw;
            }
            return true;
        }
    }
}
