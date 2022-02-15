using Caliburn.Micro;
using Helpers;
using System;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace MMNElectric.ViewModels
{
    public class ShellViewModel : Screen
    {
        public ShellViewModel()
        {
            //string Testing;
            try
            {
                DataAccess.Check(out string info);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Няма връзка с база данни \n\n{ex.Source}\n\n{ex.Message}", $"Database Error", MessageBoxButton.OK);
                Environment.Exit(0);
            }

            try
            {
                DataAccess.CheckAndCreateStructure();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при създаване на структурата \n\n{ex.Source}\n\n{ex.Message}", $"Database Error", MessageBoxButton.OK);
                Environment.Exit(0);
            }

            FillTablePota1("", "");
            FillTablePota2("", "");
            FillTablePota3("", "");
            FillTablePota4("", "");
            FillTableBurner("", "");
            FillTableFurnace("", "");
            FillTableFilter("", "");
            FillTableScale("", "");

            LoadOPC();

            //setting the timer for live data from plc
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();
        }

        protected override void OnDeactivate(bool close)
        {
            if (OPCConnection.MyOPCServer.ServerState == 1)
                OPCConnection.MyOPCServer.Disconnect();

            Application.Current.Shutdown();
        }

        public void TimerTick(object sender, EventArgs e)
        {
            //timer for live data
            Testing = MyOPCConnection.signals_Temp[1];
            Pota1ReadTFromPLC = MyOPCConnection.signals_Temp[2];
            Pota1ReadRpmFromPLC = MyOPCConnection.signals_Temp[3];
            Pota2ReadTFromPLC = MyOPCConnection.signals_Temp[4];
            Pota2ReadRpmFromPLC = MyOPCConnection.signals_Temp[5];
            Pota3ReadTFromPLC = MyOPCConnection.signals_Temp[6];
            Pota3ReadRpmFromPLC = MyOPCConnection.signals_Temp[7];
            Pota4ReadTFromPLC = MyOPCConnection.signals_Temp[8];
            Pota4ReadRpmFromPLC = MyOPCConnection.signals_Temp[9];
            BurnerReadFromPLC = MyOPCConnection.signals_Temp[10];
            FurnaceReadFromPLC = MyOPCConnection.signals_Temp[11];
            FilterReadFromPLC = MyOPCConnection.signals_Temp[12];
            ScaleReadFromPLC = MyOPCConnection.signals_Temp[13];

            Pota1TFromPLC = MyOPCConnection.floats_Temp[1].ToString();
            Pota1RpmFromPLC = MyOPCConnection.floats_Temp[2].ToString();

            Pota2TFromPLC = MyOPCConnection.floats_Temp[3].ToString();
            Pota2RpmFromPLC = MyOPCConnection.floats_Temp[4].ToString();

            Pota3TFromPLC = MyOPCConnection.floats_Temp[5].ToString();
            Pota3RpmFromPLC = MyOPCConnection.floats_Temp[6].ToString();

            Pota4TFromPLC = MyOPCConnection.floats_Temp[7].ToString();
            Pota4RpmFromPLC = MyOPCConnection.floats_Temp[8].ToString();

            BurnerExpenseGasFromPLC = MyOPCConnection.floats_Temp[9].ToString("X");
            BurnerExpenseOxFromPLC = MyOPCConnection.floats_Temp[10].ToString("X");

            BurnerSumExpenseGasFromPLC = MyOPCConnection.floats_Temp[11].ToString(); //new
            BurnerSumExpenseOxFromPLC = MyOPCConnection.floats_Temp[12].ToString(); //new
            BurnerPressureGasFromPLC = MyOPCConnection.floats_Temp[13].ToString();
            BurnerPressureOxFromPLC = MyOPCConnection.floats_Temp[14].ToString();
            //BurnerExpenseGasFromPLC = int.Parse(MyOPCConnection.floats_Temp[13].ToString(), System.Globalization.NumberStyles.HexNumber).ToString(); // from hex
            //BurnerExpenseOxFromPLC = int.Parse(MyOPCConnection.floats_Temp[14].ToString(), System.Globalization.NumberStyles.HexNumber).ToString(); // from hex
            BurnerTGasFromPLC = MyOPCConnection.floats_Temp[15].ToString();
            BurnerTOxFromPLC = MyOPCConnection.floats_Temp[16].ToString();

            FurnaceSetpointFromPLC = MyOPCConnection.floats_Temp[17].ToString();
            FurnaceSpeedFromPLC = MyOPCConnection.floats_Temp[18].ToString();

            FilterTFromPLC = MyOPCConnection.floats_Temp[19].ToString();
            FilterLoadFromPLC = MyOPCConnection.floats_Temp[20].ToString();

            ScaleWeightFromPLC = MyOPCConnection.floats_Temp[21].ToString(); //new

            //checking for write permition
            if (MyOPCConnection.signals_Temp[2] || MyOPCConnection.signals_Temp[3])
            {
                if (MyOPCConnection.signals_Temp[2])
                    while (MyOPCConnection.signals_Temp[2])
                        MyOPCConnection.WriteToPLC(2);

                if (MyOPCConnection.signals_Temp[3])
                    while (MyOPCConnection.signals_Temp[3])
                        MyOPCConnection.WriteToPLC(3);

                if (DataAccess.SaveToDB(StructurePota1.tableName, StructurePota1.TableColumns.dateStamp,
                    StructurePota1.TableColumns.date, StructurePota1.TableColumns.time,
                    StructurePota1.TableColumns.pota1T, Pota1TFromPLC,
                    StructurePota1.TableColumns.pota1Rpm, Pota1RpmFromPLC))
                {
                    Pota1ReadTFromPLC = false;
                    FillTablePota1(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                }
            }

            if (MyOPCConnection.signals_Temp[4] || MyOPCConnection.signals_Temp[5])
            {
                if (MyOPCConnection.signals_Temp[4])
                    while (MyOPCConnection.signals_Temp[4])
                        MyOPCConnection.WriteToPLC(4);

                if (MyOPCConnection.signals_Temp[5])
                    while (MyOPCConnection.signals_Temp[5])
                        MyOPCConnection.WriteToPLC(5);

                if (DataAccess.SaveToDB(StructurePota2.tableName, StructurePota2.TableColumns.dateStamp,
                        StructurePota2.TableColumns.date, StructurePota2.TableColumns.time,
                        StructurePota2.TableColumns.pota2T, Pota2TFromPLC,
                        StructurePota2.TableColumns.pota2Rpm, Pota2RpmFromPLC))
                {
                    Pota2ReadTFromPLC = false;
                    FillTablePota2(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                }
            }

            if (MyOPCConnection.signals_Temp[6] || MyOPCConnection.signals_Temp[7])
            {
                if (MyOPCConnection.signals_Temp[6])
                    while (MyOPCConnection.signals_Temp[6])
                        MyOPCConnection.WriteToPLC(6);

                if (MyOPCConnection.signals_Temp[7])
                    while (MyOPCConnection.signals_Temp[7])
                        MyOPCConnection.WriteToPLC(7);

                if (DataAccess.SaveToDB(StructurePota3.tableName, StructurePota3.TableColumns.dateStamp,
                        StructurePota3.TableColumns.date, StructurePota3.TableColumns.time,
                        StructurePota3.TableColumns.pota3T, Pota3TFromPLC,
                        StructurePota3.TableColumns.pota3Rpm, Pota3RpmFromPLC))
                {
                    Pota3ReadTFromPLC = false;
                    FillTablePota3(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                }
            }

            if (MyOPCConnection.signals_Temp[8] || MyOPCConnection.signals_Temp[9])
            {
                if (MyOPCConnection.signals_Temp[8])
                    while (MyOPCConnection.signals_Temp[8])
                        MyOPCConnection.WriteToPLC(8);

                if (MyOPCConnection.signals_Temp[9])
                    while (MyOPCConnection.signals_Temp[9])
                        MyOPCConnection.WriteToPLC(9);

                if (DataAccess.SaveToDB(StructurePota4.tableName, StructurePota4.TableColumns.dateStamp,
                        StructurePota4.TableColumns.date, StructurePota4.TableColumns.time,
                        StructurePota4.TableColumns.pota4T, Pota4TFromPLC,
                        StructurePota4.TableColumns.pota4Rpm, Pota4RpmFromPLC))
                {
                    Pota4ReadTFromPLC = false;
                    FillTablePota4(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                }
            }

            if (MyOPCConnection.signals_Temp[10])
            {
                while (MyOPCConnection.signals_Temp[10])
                    MyOPCConnection.WriteToPLC(10);

                if (DataAccess.SaveToDB(StructureBurner.tableName, StructureBurner.TableColumns.dateStamp,
                    StructureBurner.TableColumns.date, StructureBurner.TableColumns.time,
                    StructureBurner.TableColumns.burnerExpenseGas, BurnerExpenseGasFromPLC,
                    StructureBurner.TableColumns.burnerExpenseOx, BurnerExpenseOxFromPLC,
                    StructureBurner.TableColumns.burnerSumExpenseGas, BurnerSumExpenseGasFromPLC,
                    StructureBurner.TableColumns.burnerSumExpenseOx, BurnerSumExpenseOxFromPLC,
                    StructureBurner.TableColumns.burnerPressureGas, BurnerPressureGasFromPLC,
                    StructureBurner.TableColumns.burnerPressureOx, BurnerPressureOxFromPLC,
                    StructureBurner.TableColumns.burnerTGas, BurnerTGasFromPLC,
                    StructureBurner.TableColumns.burnerTOx, BurnerTOxFromPLC))
                {
                    BurnerReadFromPLC = false;
                    FillTableBurner(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                }
            }

            if (MyOPCConnection.signals_Temp[11])
            {
                while (MyOPCConnection.signals_Temp[11])
                    MyOPCConnection.WriteToPLC(11);

                if (DataAccess.SaveToDB(StructureFurnace.tableName, StructureFurnace.TableColumns.dateStamp,
                    StructureFurnace.TableColumns.date, StructureFurnace.TableColumns.time,
                    StructureFurnace.TableColumns.furnaceSetpoint, FurnaceSetpointFromPLC,
                    StructureFurnace.TableColumns.furnaceSpeed, FurnaceSpeedFromPLC))
                {
                    FurnaceReadFromPLC = false;
                    FillTableFurnace(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                }
            }

            if (MyOPCConnection.signals_Temp[12])
            {
                while (MyOPCConnection.signals_Temp[12])
                    MyOPCConnection.WriteToPLC(12);

                if (DataAccess.SaveToDB(StructureFilter.tableName, StructureFilter.TableColumns.dateStamp,
                    StructureFilter.TableColumns.date, StructureFilter.TableColumns.time,
                    StructureFilter.TableColumns.filterTemp, FilterTFromPLC,
                    StructureFilter.TableColumns.filterLoad, FilterLoadFromPLC))
                {
                    FilterReadFromPLC = false;
                    FillTableFilter(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                }
            }

            if (MyOPCConnection.signals_Temp[13])
            {
                while (MyOPCConnection.signals_Temp[13])
                    MyOPCConnection.WriteToPLC(13);

                if (DataAccess.SaveToDB(StructureScale.tableName, StructureScale.TableColumns.dateStamp,
                    StructureScale.TableColumns.date, StructureFilter.TableColumns.time,
                    StructureScale.TableColumns.scaleWeight, ScaleWeightFromPLC))
                {
                    ScaleReadFromPLC = false;
                    FillTableScale(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                }
            }

        }

        public static string AppName
        {
            get
            {
                return "ММН Електрик - Справка";
            }
        }

        #region date filtering
        public string GrpFilterByDate
        {
            get
            {
                return "Филтри по дата";
            }
        }

        public string DatePattern
        {
            get
            {
                return $"'{Helper.datePattern}'";
            }
        }

        public string LblStartDate
        {
            get
            {
                return "От дата:";
            }
        }

        private static DateTime _startDate
        {
            get; set;
        }

        public object StartDate
        {
            get
            {
                if (_startDate.ToString(Helper.dateStampPattern) == "0001-01-01")
                {
                    _startDate = DateTime.Now.AddDays(-31);
                }

                return _startDate;
            }

            set
            {
                _startDate = (DateTime)value;
                NotifyOfPropertyChange(() => StartDate);
                FillTablePota1(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTablePota2(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTablePota3(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTablePota4(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTableBurner(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTableFurnace(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTableFilter(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTableScale(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
            }
        }

        public string LblEndDate
        {
            get
            {
                return "До дата:";
            }
        }

        private static DateTime _endDate
        {
            get; set;
        }

        public object EndDate
        {
            get
            {
                if (_endDate.ToString(Helper.dateStampPattern) == "0001-01-01")
                {
                    _endDate = DateTime.Now;
                }

                return _endDate;
            }

            set
            {
                _endDate = (DateTime)value;
                NotifyOfPropertyChange(() => EndDate);
                FillTablePota1(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTablePota2(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTablePota3(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTablePota4(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTableBurner(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTableFurnace(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTableFilter(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
                FillTableScale(_startDate.ToString(Helper.dateStampPattern), _endDate.ToString(Helper.dateStampPattern));
            }
        }
        #endregion

        public string GrpLiveData
        {
            get
            {
                return "Живи данни";
            }
        }

        public string GrpData
        {
            get
            {
                return "Данни";
            }
        }

        public string Id
        {
            get
            {
                return "Запис";
            }
        }

        public string Date
        {
            get
            {
                return "Дата";
            }
        }

        public string Time
        {
            get
            {
                return "Час";
            }
        }

        public OPCConnection MyOPCConnection = new OPCConnection();

        private void LoadOPC()
        {
            if (OPCConnection.OPCConnect())
            {
                MyOPCConnection.SignalsConnect();
                MyOPCConnection.FloatsConnect();
            }
        }

        private bool _testing
        {
            get; set;
        }

        public bool Testing
        {
            get
            {
                return _testing;
            }
            set
            {
                if (_testing == value)
                    return;
                _testing = value;
                NotifyOfPropertyChange(() => Testing);
            }
        }

        #region pota1
        public string Pota1
        {
            get
            {
                return "Пота 1";
            }
        }

        public string Temperature
        {
            get
            {
                return "Температура";
            }
        }

        public string Rpm
        {
            get
            {
                return "Обороти";
            }
        }


        public static DataTable _tableDataPota1 = new DataTable();

        public DataTable TableDataPota1
        {
            get
            {
                return _tableDataPota1;
            }
            set
            {
                _tableDataPota1 = value;
                NotifyOfPropertyChange(() => TableDataPota1);
            }
        }


        public void FillTablePota1(string startDate, string endDate)
        {
            _tableDataPota1.Clear();
            _tableDataPota1 = DataAccess.LoadFromDB(StructurePota1.tableName, StructurePota1.TableColumns.id, StructurePota1.TableColumns.dateStamp, startDate, endDate);

            _tableDataPota1.Columns[0].ColumnName = "Запис";
            _tableDataPota1.Columns[1].ColumnName = "Stamp";
            _tableDataPota1.Columns[2].ColumnName = "Дата";
            _tableDataPota1.Columns[3].ColumnName = "Час";
            _tableDataPota1.Columns[4].ColumnName = "Температура";
            _tableDataPota1.Columns[5].ColumnName = "Обороти";

            NotifyOfPropertyChange(() => TableDataPota1);
        }

        private string _pota1TFromPLC
        {
            get; set;
        }

        public string Pota1TFromPLC
        {
            get
            {
                return _pota1TFromPLC;
            }
            set
            {
                if (_pota1TFromPLC == value)
                    return;
                _pota1TFromPLC = value;
                NotifyOfPropertyChange(() => Pota1TFromPLC);
            }
        }

        private bool _pota1ReadTFromPLC
        {
            get; set;
        }

        public bool Pota1ReadTFromPLC
        {
            get
            {
                return _pota1ReadTFromPLC;
            }
            set
            {
                if (_pota1ReadTFromPLC == value)
                    return;
                _pota1ReadTFromPLC = value;
                NotifyOfPropertyChange(() => Pota1ReadTFromPLC);
            }
        }

        private string _pota1RpmFromPLC
        {
            get; set;
        }

        public string Pota1RpmFromPLC
        {
            get
            {
                return _pota1RpmFromPLC;
            }
            set
            {
                if (_pota1RpmFromPLC == value)
                    return;
                _pota1RpmFromPLC = value;
                NotifyOfPropertyChange(() => Pota1RpmFromPLC);
            }
        }

        private bool _pota1ReadRpmFromPLC
        {
            get; set;
        }

        public bool Pota1ReadRpmFromPLC
        {
            get
            {
                return _pota1ReadRpmFromPLC;
            }
            set
            {
                if (_pota1ReadRpmFromPLC == value)
                    return;
                _pota1ReadRpmFromPLC = value;
                NotifyOfPropertyChange(() => Pota1ReadRpmFromPLC);
            }
        }
        #endregion

        #region pota2
        public string Pota2
        {
            get
            {
                return "Пота 2";
            }
        }

        public static DataTable _tableDataPota2 = new DataTable();

        public DataTable TableDataPota2
        {
            get
            {
                return _tableDataPota2;
            }
            set
            {
                _tableDataPota2 = value;
                NotifyOfPropertyChange(() => TableDataPota2);
            }
        }

        private void FillTablePota2(string startDate, string endDate)
        {
            _tableDataPota2.Clear();
            _tableDataPota2 = DataAccess.LoadFromDB(StructurePota2.tableName, StructurePota2.TableColumns.id, StructurePota2.TableColumns.dateStamp, startDate, endDate);

            _tableDataPota2.Columns[0].ColumnName = "Запис";
            _tableDataPota2.Columns[1].ColumnName = "Stamp";
            _tableDataPota2.Columns[2].ColumnName = "Дата";
            _tableDataPota2.Columns[3].ColumnName = "Час";
            _tableDataPota2.Columns[4].ColumnName = "Температура";
            _tableDataPota2.Columns[5].ColumnName = "Обороти";

            NotifyOfPropertyChange(() => TableDataPota2);
        }

        private string _pota2TFromPLC
        {
            get; set;
        }

        public string Pota2TFromPLC
        {
            get
            {
                return _pota2TFromPLC;
            }
            set
            {
                if (_pota2TFromPLC == value)
                    return;
                _pota2TFromPLC = value;
                NotifyOfPropertyChange(() => Pota2TFromPLC);
            }
        }

        private bool _pota2ReadTFromPLC
        {
            get; set;
        }

        public bool Pota2ReadTFromPLC
        {
            get
            {
                return _pota2ReadTFromPLC;
            }
            set
            {
                if (_pota2ReadTFromPLC == value)
                    return;
                _pota2ReadTFromPLC = value;
                NotifyOfPropertyChange(() => Pota2ReadTFromPLC);
            }
        }

        private string _pota2RpmFromPLC
        {
            get; set;
        }

        public string Pota2RpmFromPLC
        {
            get
            {
                return _pota2RpmFromPLC;
            }
            set
            {
                if (_pota2RpmFromPLC == value)
                    return;
                _pota2RpmFromPLC = value;
                NotifyOfPropertyChange(() => Pota2RpmFromPLC);
            }
        }

        private bool _pota2ReadRpmFromPLC
        {
            get; set;
        }

        public bool Pota2ReadRpmFromPLC
        {
            get
            {
                return _pota2ReadRpmFromPLC;
            }
            set
            {
                if (_pota2ReadRpmFromPLC == value)
                    return;
                _pota2ReadRpmFromPLC = value;
                NotifyOfPropertyChange(() => Pota2ReadRpmFromPLC);
            }
        }
        #endregion

        #region pota3
        public string Pota3
        {
            get
            {
                return "Пота 3";
            }
        }

        public static DataTable _tableDataPota3 = new DataTable();

        public DataTable TableDataPota3
        {
            get
            {
                return _tableDataPota3;
            }
            set
            {
                _tableDataPota3 = value;
                NotifyOfPropertyChange(() => TableDataPota3);
            }
        }

        private void FillTablePota3(string startDate, string endDate)
        {
            _tableDataPota3.Clear();
            _tableDataPota3 = DataAccess.LoadFromDB(StructurePota3.tableName, StructurePota3.TableColumns.id, StructurePota3.TableColumns.dateStamp, startDate, endDate);

            _tableDataPota3.Columns[0].ColumnName = "Запис";
            _tableDataPota3.Columns[1].ColumnName = "Stamp";
            _tableDataPota3.Columns[2].ColumnName = "Дата";
            _tableDataPota3.Columns[3].ColumnName = "Час";
            _tableDataPota3.Columns[4].ColumnName = "Температура";
            _tableDataPota3.Columns[5].ColumnName = "Обороти";

            NotifyOfPropertyChange(() => TableDataPota3);
        }

        private string _pota3TFromPLC
        {
            get; set;
        }

        public string Pota3TFromPLC
        {
            get
            {
                return _pota3TFromPLC;
            }
            set
            {
                if (_pota3TFromPLC == value)
                    return;
                _pota3TFromPLC = value;
                NotifyOfPropertyChange(() => Pota3TFromPLC);
            }
        }

        private bool _pota3ReadTFromPLC
        {
            get; set;
        }

        public bool Pota3ReadTFromPLC
        {
            get
            {
                return _pota3ReadTFromPLC;
            }
            set
            {
                if (_pota3ReadTFromPLC == value)
                    return;
                _pota3ReadTFromPLC = value;
                NotifyOfPropertyChange(() => Pota3ReadTFromPLC);
            }
        }

        private string _pota3RpmFromPLC
        {
            get; set;
        }

        public string Pota3RpmFromPLC
        {
            get
            {
                return _pota3RpmFromPLC;
            }
            set
            {
                if (_pota3RpmFromPLC == value)
                    return;
                _pota3RpmFromPLC = value;
                NotifyOfPropertyChange(() => Pota3RpmFromPLC);
            }
        }

        private bool _pota3ReadRpmFromPLC
        {
            get; set;
        }

        public bool Pota3ReadRpmFromPLC
        {
            get
            {
                return _pota3ReadRpmFromPLC;
            }
            set
            {
                if (_pota3ReadRpmFromPLC == value)
                    return;
                _pota3ReadRpmFromPLC = value;
                NotifyOfPropertyChange(() => Pota3ReadRpmFromPLC);
            }
        }
        #endregion

        #region pota4
        public string Pota4
        {
            get
            {
                return "Пота 4";
            }
        }

        public static DataTable _tableDataPota4 = new DataTable();

        public DataTable TableDataPota4
        {
            get
            {
                return _tableDataPota4;
            }
            set
            {
                _tableDataPota4 = value;
                NotifyOfPropertyChange(() => TableDataPota4);
            }
        }

        private void FillTablePota4(string startDate, string endDate)
        {
            _tableDataPota4.Clear();
            _tableDataPota4 = DataAccess.LoadFromDB(StructurePota4.tableName, StructurePota4.TableColumns.id, StructurePota4.TableColumns.dateStamp, startDate, endDate);

            _tableDataPota4.Columns[0].ColumnName = "Запис";
            _tableDataPota4.Columns[1].ColumnName = "Stamp";
            _tableDataPota4.Columns[2].ColumnName = "Дата";
            _tableDataPota4.Columns[3].ColumnName = "Час";
            _tableDataPota4.Columns[4].ColumnName = "Температура";
            _tableDataPota4.Columns[5].ColumnName = "Обороти";

            NotifyOfPropertyChange(() => TableDataPota4);
        }

        private string _pota4TFromPLC
        {
            get; set;
        }

        public string Pota4TFromPLC
        {
            get
            {
                return _pota4TFromPLC;
            }
            set
            {
                if (_pota4TFromPLC == value)
                    return;
                _pota4TFromPLC = value;
                NotifyOfPropertyChange(() => Pota4TFromPLC);
            }
        }

        private bool _pota4ReadTFromPLC
        {
            get; set;
        }

        public bool Pota4ReadTFromPLC
        {
            get
            {
                return _pota4ReadTFromPLC;
            }
            set
            {
                if (_pota4ReadTFromPLC == value)
                    return;
                _pota4ReadTFromPLC = value;
                NotifyOfPropertyChange(() => Pota4ReadTFromPLC);
            }
        }

        private string _pota4RpmFromPLC
        {
            get; set;
        }

        public string Pota4RpmFromPLC
        {
            get
            {
                return _pota4RpmFromPLC;
            }
            set
            {
                if (_pota4RpmFromPLC == value)
                    return;
                _pota4RpmFromPLC = value;
                NotifyOfPropertyChange(() => Pota4RpmFromPLC);
            }
        }

        private bool _pota4ReadRpmFromPLC
        {
            get; set;
        }

        public bool Pota4ReadRpmFromPLC
        {
            get
            {
                return _pota4ReadRpmFromPLC;
            }
            set
            {
                if (_pota4ReadRpmFromPLC == value)
                    return;
                _pota4ReadRpmFromPLC = value;
                NotifyOfPropertyChange(() => Pota4ReadRpmFromPLC);
            }
        }
        #endregion

        #region burner
        public string Burner
        {
            get
            {
                return "Горелка";
            }
        }

        public string ExpenseGas
        {
            get
            {
                return "Разход Газ";
            }
        }

        public string ExpenseOx
        {
            get
            {
                return "Разход Кислород";
            }
        }

        public string SumExpenseGas
        {
            get
            {
                return "Сума Разход Газ";
            }
        }

        public string SumExpenseOx
        {
            get
            {
                return "Сума Разход Кислород";
            }
        }

        public string PressureGas
        {
            get
            {
                return "Налягане Газ";
            }
        }

        public string PressureOx
        {
            get
            {
                return "Налягане Кислород";
            }
        }

        public string TemperatureGas
        {
            get
            {
                return "Температура Газ";
            }
        }

        public string TemperatureOx
        {
            get
            {
                return "Температура Кислород";
            }
        }

        public static DataTable _tableDataBurner = new DataTable();

        public DataTable TableDataBurner
        {
            get
            {
                return _tableDataBurner;
            }
            set
            {
                _tableDataBurner = value;
                NotifyOfPropertyChange(() => TableDataBurner);
            }
        }

        private void FillTableBurner(string startDate, string endDate)
        {
            _tableDataBurner.Clear();
            _tableDataBurner = DataAccess.LoadFromDB(StructureBurner.tableName, StructureBurner.TableColumns.id, StructureBurner.TableColumns.dateStamp, startDate, endDate);

            _tableDataBurner.Columns[0].ColumnName = "Запис";
            _tableDataBurner.Columns[1].ColumnName = "Stamp";
            _tableDataBurner.Columns[2].ColumnName = "Дата";
            _tableDataBurner.Columns[3].ColumnName = "Час";
            _tableDataBurner.Columns[4].ColumnName = "Температура_Газ";
            _tableDataBurner.Columns[5].ColumnName = "Температура_Кислород";
            _tableDataBurner.Columns[6].ColumnName = "Разход_Газ";
            _tableDataBurner.Columns[7].ColumnName = "Разход_Кислород";
            _tableDataBurner.Columns[8].ColumnName = "Налягане_Газ";
            _tableDataBurner.Columns[9].ColumnName = "Налягане_Кислород";
            _tableDataBurner.Columns[10].ColumnName = "Разход_Газ_Сума";
            _tableDataBurner.Columns[11].ColumnName = "Разход_Кислород_Сума";

            NotifyOfPropertyChange(() => TableDataBurner);
        }

        private bool _burnerReadFromPLC
        {
            get; set;
        }

        public bool BurnerReadFromPLC
        {
            get
            {
                return _burnerReadFromPLC;
            }
            set
            {
                if (_burnerReadFromPLC == value)
                    return;
                _burnerReadFromPLC = value;
                NotifyOfPropertyChange(() => BurnerReadFromPLC);
            }
        }

        private string _burnerTGasFromPLC
        {
            get; set;
        }

        public string BurnerTGasFromPLC
        {
            get
            {
                return _burnerTGasFromPLC;
            }
            set
            {
                if (_burnerTGasFromPLC == value)
                    return;
                _burnerTGasFromPLC = value;
                NotifyOfPropertyChange(() => BurnerTGasFromPLC);
            }
        }

        private string _burnerTOxFromPLC
        {
            get; set;
        }

        public string BurnerTOxFromPLC
        {
            get
            {
                return _burnerTOxFromPLC;
            }
            set
            {
                if (_burnerTOxFromPLC == value)
                    return;
                _burnerTOxFromPLC = value;
                NotifyOfPropertyChange(() => BurnerTOxFromPLC);
            }
        }


        private string _burnerExpenseGasFromPLC
        {
            get; set;
        }

        public string BurnerExpenseGasFromPLC
        {
            get
            {
                return _burnerExpenseGasFromPLC;
            }
            set
            {
                if (_burnerExpenseGasFromPLC == value)
                    return;
                _burnerExpenseGasFromPLC = value;
                NotifyOfPropertyChange(() => BurnerExpenseGasFromPLC);
            }
        }

        private string _burnerExpenseOxFromPLC
        {
            get; set;
        }

        public string BurnerExpenseOxFromPLC
        {
            get
            {
                return _burnerExpenseOxFromPLC;
            }
            set
            {
                if (_burnerExpenseOxFromPLC == value)
                    return;
                _burnerExpenseOxFromPLC = value;
                NotifyOfPropertyChange(() => BurnerExpenseOxFromPLC);
            }
        }

        private string _burnerSumExpenseGasFromPLC
        {
            get; set;
        }

        public string BurnerSumExpenseGasFromPLC
        {
            get
            {
                return _burnerSumExpenseGasFromPLC;
            }
            set
            {
                if (_burnerSumExpenseGasFromPLC == value)
                    return;
                _burnerSumExpenseGasFromPLC = value;
                NotifyOfPropertyChange(() => BurnerSumExpenseGasFromPLC);
            }
        }

        private string _burnerSumExpenseOxFromPLC
        {
            get; set;
        }

        public string BurnerSumExpenseOxFromPLC
        {
            get
            {
                return _burnerSumExpenseOxFromPLC;
            }
            set
            {
                if (_burnerSumExpenseOxFromPLC == value)
                    return;
                _burnerSumExpenseOxFromPLC = value;
                NotifyOfPropertyChange(() => BurnerSumExpenseOxFromPLC);
            }
        }

        private string _burnerPressureGasFromPLC
        {
            get; set;
        }

        public string BurnerPressureGasFromPLC
        {
            get
            {
                return _burnerPressureGasFromPLC;
            }
            set
            {
                if (_burnerPressureGasFromPLC == value)
                    return;
                _burnerPressureGasFromPLC = value;
                NotifyOfPropertyChange(() => BurnerPressureGasFromPLC);
            }
        }

        private string _burnerPressureOxFromPLC
        {
            get; set;
        }

        public string BurnerPressureOxFromPLC
        {
            get
            {
                return _burnerPressureOxFromPLC;
            }
            set
            {
                if (_burnerPressureOxFromPLC == value)
                    return;
                _burnerPressureOxFromPLC = value;
                NotifyOfPropertyChange(() => BurnerPressureOxFromPLC);
            }
        }

        #endregion

        #region furnace
        public string Furnace
        {
            get
            {
                return "Пещ";
            }
        }

        public string FurnaceSetpoint
        {
            get
            {
                return "Setpoint";
            }
        }

        public string FurnaceSpeed
        {
            get
            {
                return "Current";
            }
        }

        public static DataTable _tableDataFurnace = new DataTable();

        public DataTable TableDataFurnace
        {
            get
            {
                return _tableDataFurnace;
            }
            set
            {
                _tableDataFurnace = value;
                NotifyOfPropertyChange(() => TableDataFurnace);
            }
        }

        private void FillTableFurnace(string startDate, string endDate)
        {
            _tableDataFurnace.Clear();
            _tableDataFurnace = DataAccess.LoadFromDB(StructureFurnace.tableName, StructureFurnace.TableColumns.id, StructureFurnace.TableColumns.dateStamp, startDate, endDate);

            _tableDataFurnace.Columns[0].ColumnName = "Запис";
            _tableDataFurnace.Columns[1].ColumnName = "Stamp";
            _tableDataFurnace.Columns[2].ColumnName = "Дата";
            _tableDataFurnace.Columns[3].ColumnName = "Час";
            _tableDataFurnace.Columns[4].ColumnName = "Setpoint";
            _tableDataFurnace.Columns[5].ColumnName = "Current";

            NotifyOfPropertyChange(() => TableDataFurnace);
        }

        private bool _furnaceReadFromPLC
        {
            get; set;
        }

        public bool FurnaceReadFromPLC
        {
            get
            {
                return _furnaceReadFromPLC;
            }
            set
            {
                if (_furnaceReadFromPLC == value)
                    return;
                _furnaceReadFromPLC = value;
                NotifyOfPropertyChange(() => FurnaceReadFromPLC);
            }
        }

        private string _furnaceSetpointFromPLC
        {
            get; set;
        }

        public string FurnaceSetpointFromPLC
        {
            get
            {
                return _furnaceSetpointFromPLC;
            }
            set
            {
                if (_furnaceSetpointFromPLC == value)
                    return;
                _furnaceSetpointFromPLC = value;
                NotifyOfPropertyChange(() => FurnaceSetpointFromPLC);
            }
        }

        private string _furnaceSpeedFromPLC
        {
            get; set;
        }

        public string FurnaceSpeedFromPLC
        {
            get
            {
                return _furnaceSpeedFromPLC;
            }
            set
            {
                if (_furnaceSpeedFromPLC == value)
                    return;
                _furnaceSpeedFromPLC = value;
                NotifyOfPropertyChange(() => FurnaceSpeedFromPLC);
            }
        }

        #endregion

        #region filter
        public string Filter
        {
            get
            {
                return "Филтър";
            }
        }

        public string Load
        {
            get
            {
                return "Натоварване";
            }
        }

        public static DataTable _tableDataFilter = new DataTable();

        public DataTable TableDataFilter
        {
            get
            {
                return _tableDataFilter;
            }
            set
            {
                _tableDataFilter = value;
                NotifyOfPropertyChange(() => TableDataFilter);
            }
        }

        private void FillTableFilter(string startDate, string endDate)
        {
            _tableDataFilter.Clear();
            _tableDataFilter = DataAccess.LoadFromDB(StructureFilter.tableName, StructureFilter.TableColumns.id,
                StructureFilter.TableColumns.dateStamp, startDate, endDate);

            _tableDataFilter.Columns[0].ColumnName = "Запис";
            _tableDataFilter.Columns[1].ColumnName = "Stamp";
            _tableDataFilter.Columns[2].ColumnName = "Дата";
            _tableDataFilter.Columns[3].ColumnName = "Час";
            _tableDataFilter.Columns[4].ColumnName = "Температура";
            _tableDataFilter.Columns[5].ColumnName = "Натоварване";

            NotifyOfPropertyChange(() => TableDataFilter);
        }

        private bool _filterReadFromPLC
        {
            get; set;
        }

        public bool FilterReadFromPLC
        {
            get
            {
                return _filterReadFromPLC;
            }
            set
            {
                if (_filterReadFromPLC == value)
                    return;
                _filterReadFromPLC = value;
                NotifyOfPropertyChange(() => FilterReadFromPLC);
            }
        }

        private string _filterTFromPLC
        {
            get; set;
        }

        public string FilterTFromPLC
        {
            get
            {
                return _filterTFromPLC;
            }
            set
            {
                if (_filterTFromPLC == value)
                    return;
                _filterTFromPLC = value;
                NotifyOfPropertyChange(() => FilterTFromPLC);
            }
        }

        private string _filterLoadFromPLC
        {
            get; set;
        }

        public string FilterLoadFromPLC
        {
            get
            {
                return _filterLoadFromPLC;
            }
            set
            {
                if (_filterLoadFromPLC == value)
                    return;
                _filterLoadFromPLC = value;
                NotifyOfPropertyChange(() => FilterLoadFromPLC);
            }
        }
        #endregion

        #region scale
        public string Scale
        {
            get
            {
                return "Кантар";
            }
        }

        public string Weight
        {
            get
            {
                return "Тегло";
            }
        }

        public static DataTable _tableDataScale = new DataTable();

        public DataTable TableDataScale
        {
            get
            {
                return _tableDataScale;
            }
            set
            {
                _tableDataScale = value;
                NotifyOfPropertyChange(() => TableDataScale);
            }
        }

        private void FillTableScale(string startDate, string endDate)
        {
            _tableDataScale.Clear();
            _tableDataScale = DataAccess.LoadFromDB(StructureScale.tableName, StructureScale.TableColumns.id, StructureScale.TableColumns.dateStamp,
                startDate, endDate);

            _tableDataScale.Columns[0].ColumnName = "Запис";
            _tableDataScale.Columns[1].ColumnName = "Stamp";
            _tableDataScale.Columns[2].ColumnName = "Дата";
            _tableDataScale.Columns[3].ColumnName = "Час";
            _tableDataScale.Columns[4].ColumnName = "Тегло";

            NotifyOfPropertyChange(() => TableDataScale);
        }

        private bool _scaleReadFromPLC
        {
            get; set;
        }

        public bool ScaleReadFromPLC
        {
            get
            {
                return _scaleReadFromPLC;
            }
            set
            {
                if (_scaleReadFromPLC == value)
                    return;
                _scaleReadFromPLC = value;
                NotifyOfPropertyChange(() => ScaleReadFromPLC);
            }
        }

        private string _scaleWeightFromPLC
        {
            get; set;
        }

        public string ScaleWeightFromPLC
        {
            get
            {
                return _scaleWeightFromPLC;
            }
            set
            {
                if (_scaleWeightFromPLC == value)
                    return;
                _scaleWeightFromPLC = value;
                NotifyOfPropertyChange(() => ScaleWeightFromPLC);
            }
        }
        #endregion

    }
}

