using MMNElectric.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace MMNElectric.Views
{
    public partial class ShellView : Window
    {

        //public object[] NewObject = new object[8]
        //{
        //    ShellViewModel._tableDataPota1,
        //    ShellViewModel._tableDataPota2,
        //    ShellViewModel._tableDataPota3,
        //    ShellViewModel._tableDataPota4,
        //    ShellViewModel._tableDataBurner,
        //    ShellViewModel._tableDataFurnace,
        //    ShellViewModel._tableDataFilter,
        //    ShellViewModel._tableDataScale
        //};

        private void ExcelExport_Click(object sender, RoutedEventArgs e)
        {
            //List<DataTable> ListOfTables { get; set; }

            object[] NewObject = new object[8]
    {
            ShellViewModel._tableDataPota1,
            ShellViewModel._tableDataPota2,
            ShellViewModel._tableDataPota3,
            ShellViewModel._tableDataPota4,
            ShellViewModel._tableDataBurner,
            ShellViewModel._tableDataFurnace,
            ShellViewModel._tableDataFilter,
            ShellViewModel._tableDataScale
    };

            NewObject.ExportManyTablesToExcel();
        }
        //private void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        //{
        //    //MessageBox.Show("ok");
        //}
    }
}
