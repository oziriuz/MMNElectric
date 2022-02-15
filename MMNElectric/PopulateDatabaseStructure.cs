using System;
using System.Collections.Generic;
using System.Linq;

namespace MMNElectric
{
    public class PopulateDatabaseStructure
    {
        public List<SQLTableModel> GetAllSQLTables()
        {
            List<SQLTableModel> outputAllTables = new List<SQLTableModel>
            {
                GetTable(StructureBurner.tableName, StructureBurner.ColumnsToCreate.colNameVar),
                GetTable(StructureFilter.tableName, StructureFilter.ColumnsToCreate.colNameVar),
                GetTable(StructureFurnace.tableName, StructureFurnace.ColumnsToCreate.colNameVar),
                GetTable(StructurePota1.tableName, StructurePota1.ColumnsToCreate.colNameVar),
                GetTable(StructurePota2.tableName, StructurePota2.ColumnsToCreate.colNameVar),
                GetTable(StructurePota3.tableName, StructurePota3.ColumnsToCreate.colNameVar),
                GetTable(StructurePota4.tableName, StructurePota4.ColumnsToCreate.colNameVar),
                GetTable(StructureScale.tableName, StructureScale.ColumnsToCreate.colNameVar)
            };

            return outputAllTables;
        }

        private SQLTableModel GetTable(string tableName, string[] tableInfo)
        {
            SQLTableModel outputTable = new SQLTableModel();

            string _name;
            string _dataType;
            string _unique = "";

            if(!String.IsNullOrWhiteSpace(tableName))
            {
                outputTable.TableName = tableName;
                outputTable.NumberOfColumns = tableInfo.Length;

                for(int i = 0; i < outputTable.NumberOfColumns; i++)
                {
                    if(!String.IsNullOrWhiteSpace(tableInfo[i]))
                    {
                        _name = tableInfo[i].Split(' ').First();
                        _dataType = tableInfo[i].Split(' ').ElementAt(1);

                        if(tableInfo[i].Split(' ').Count() > 2)
                            _unique = tableInfo[i].Split(' ').ElementAt(2);

                        if(i == 0)
                            outputTable.TableColumns.Add(GetColumn(_name, _dataType, true));
                        else
                        {
                            if(_unique == "unique")
                            {
                                outputTable.TableColumns.Add(GetColumn(_name, _dataType, false, true));
                                _unique = "";
                            }
                            else
                                outputTable.TableColumns.Add(GetColumn(_name, _dataType));
                        }
                    }
                    else
                        throw new ArgumentNullException();
                }
                outputTable.PrimaryKey = tableInfo[0].Split(' ').First();
            }
            else
                throw new ArgumentNullException();

            return outputTable;
        }

        private SQLColumnModel GetColumn(string Name, string DataType, bool isPrimary = false, bool isUnique = false)
        {
            SQLColumnModel output = new SQLColumnModel();

            if(!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataType))
            {
                output.ColumnName = Name;
                output.ColumnDataType = DataType;
                output.IsPrimary = isPrimary;
                output.IsUnique = isUnique;
            }
            else
                throw new ArgumentNullException();

            return output;
        }
    }
}
