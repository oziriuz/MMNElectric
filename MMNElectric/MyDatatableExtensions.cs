using System;
using System.Data;
using System.Windows;
using MMNElectric.ViewModels;

namespace MMNElectric
{
    public static class MyDatatableExtensions
    {
        /// <summary>
        /// Export DataTable to Excel file
        /// </summary>
        /// <param name="DataTable">Source DataTable</param>
        /// <param name="ExcelFilePath">Path to result file name</param>

        public static void ExportManyTablesToExcel(this object[] ListOfDataTables, string ExcelFilePath = null)
        {
            ShellViewModel._tableDataPota1.TableName = "Пота_1";
            ShellViewModel._tableDataPota2.TableName = "Пота_2";
            ShellViewModel._tableDataPota3.TableName = "Пота_3";
            ShellViewModel._tableDataPota4.TableName = "Пота_4";
            ShellViewModel._tableDataBurner.TableName = "Горелка";
            ShellViewModel._tableDataFurnace.TableName = "Пещ";
            ShellViewModel._tableDataFilter.TableName = "Филтър";
            ShellViewModel._tableDataScale.TableName = "Кантар";

            try
            {
                
                //int ColumnsCount;

                //if (DataTable == null || (ColumnsCount = DataTable.Columns.Count) == 0)
                //    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook
                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbooks.Add();
                Excel.Visible = true;

                if (Excel == null)
                {
                    MessageBox.Show("Excel не е инсталиран!");
                    return;
                }

                // single worksheet
                foreach (DataTable dataTable in ListOfDataTables)
                {
                    int ColumnsCount;

                    if (dataTable == null || (ColumnsCount = dataTable.Columns.Count) == 0)
                        throw new Exception("Има празна или невалидна таблица!\n");
                    //Microsoft.Office.Interop.Excel._Worksheet Worksheet = Excel.ActiveSheet();

                    Microsoft.Office.Interop.Excel._Worksheet Worksheet = Excel.Sheets.Add();
                    Worksheet.Name = dataTable.TableName.ToString();

                    object[] Header = new object[ColumnsCount];
                    // table header

                    Worksheet.Cells[1, 1] = Worksheet.Name;
                    // column headings
                    Header[0] = dataTable.Columns[0].ColumnName;
                    for (int i = 2; i < ColumnsCount; i++)
                        Header[i - 1] = dataTable.Columns[i].ColumnName;

                    Microsoft.Office.Interop.Excel.Range HeaderRange = Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[2, 1]),
                        (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[2, ColumnsCount-1]));
                    HeaderRange.Value = Header;
                    //HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    HeaderRange.Font.Bold = true;

                    // DataCells
                    int RowsCount = dataTable.Rows.Count;
                    object[,] Cells = new object[RowsCount, ColumnsCount];

                    for (int j = 0; j < RowsCount; j++)
                    {
                        Cells[j, 0] = dataTable.Rows[j][0];
                        for (int i = 2; i < ColumnsCount; i++)
                        {
                            Cells[j, i - 1] = dataTable.Rows[j][i].ToString();
                        }
                            
                    }
                    Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[3, 1]),
                        (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[RowsCount + 2, ColumnsCount])).Value = Cells;
                }
                // check filepath
                //if (ExcelFilePath != null && ExcelFilePath != "")
                //{
                //    try
                //    {
                //        //Worksheet.SaveAs(ExcelFilePath);
                //        Excel.Quit();
                //        MessageBox.Show("Файлът е записан!");
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new Exception("Файлът не може да бъде записан.\n"
                //            + ex.Message);
                //    }
                //}
                //else    // no filepath is given
                //{
                //    Excel.Visible = true;
                //}
            }
            catch (Exception ex)
            {
                //throw new Exception("ExportToExcel: \n" + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }


    }
}
