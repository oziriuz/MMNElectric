using OPCAutomation;
using System;
using System.Configuration;
using System.Windows;


namespace MMNElectric
{
    public class OPCConnection
    {
        public OPCConnection()
        {

        }

        //main node for the plc
        public string node = ConfigurationManager.AppSettings["OPCNode"];
        #region OPC ItemID Tags

        // group test and booleans
        public static string tagDynamicTest = ConfigurationManager.AppSettings["tagDynamicTest"];
        public static string tagPota1ReadT = ConfigurationManager.AppSettings["tagPota1ReadT"];
        public static string tagPota1ReadRpm = ConfigurationManager.AppSettings["tagPota1ReadRpm"];
        public static string tagPota2ReadT = ConfigurationManager.AppSettings["tagPota2ReadT"];
        public static string tagPota2ReadRpm = ConfigurationManager.AppSettings["tagPota2ReadRpm"];
        public static string tagPota3ReadT = ConfigurationManager.AppSettings["tagPota3ReadT"];
        public static string tagPota3ReadRpm = ConfigurationManager.AppSettings["tagPota3ReadRpm"];
        public static string tagPota4ReadT = ConfigurationManager.AppSettings["tagPota4ReadT"];
        public static string tagPota4ReadRpm = ConfigurationManager.AppSettings["tagPota4ReadRpm"];
        public static string tagBurnerRead = ConfigurationManager.AppSettings["tagBurnerRead"];
        public static string tagFurnaceRead = ConfigurationManager.AppSettings["tagFurnaceRead"];
        public static string tagFilterRead = ConfigurationManager.AppSettings["tagFilterRead"];
        public static string tagScaleRead = ConfigurationManager.AppSettings["tagScaleRead"];

        // group data floats
        public static string tagPota1T = ConfigurationManager.AppSettings["tagPota1T"];
        public static string tagPota1Rpm = ConfigurationManager.AppSettings["tagPota1Rpm"];

        public static string tagPota2T = ConfigurationManager.AppSettings["tagPota2T"];
        public static string tagPota2Rpm = ConfigurationManager.AppSettings["tagPota2Rpm"];

        public static string tagPota3T = ConfigurationManager.AppSettings["tagPota3T"];
        public static string tagPota3Rpm = ConfigurationManager.AppSettings["tagPota3Rpm"];

        public static string tagPota4T = ConfigurationManager.AppSettings["tagPota4T"];
        public static string tagPota4Rpm = ConfigurationManager.AppSettings["tagPota4Rpm"];

        public static string tagBurnerExpenseGas = ConfigurationManager.AppSettings["tagBurnerExpenseGas"];
        public static string tagBurnerExpenseOx = ConfigurationManager.AppSettings["tagBurnerExpenseOx"];
        public static string tagBurnerSumExpenseGas = ConfigurationManager.AppSettings["tagBurnerSumExpenseGas"];
        public static string tagBurnerSumExpenseOx = ConfigurationManager.AppSettings["tagBurnerSumExpenseOx"];
        public static string tagBurnerPressureGas = ConfigurationManager.AppSettings["tagBurnerPressureGas"];
        public static string tagBurnerPressureOx = ConfigurationManager.AppSettings["tagBurnerPressureOx"];
        public static string tagBurnerTGas = ConfigurationManager.AppSettings["tagBurnerTGas"];
        public static string tagBurnerTOx = ConfigurationManager.AppSettings["tagBurnerTOx"];

        public static string tagFurnaceSetpoint = ConfigurationManager.AppSettings["tagFurnaceSetpoint"];
        public static string tagFurnaceSpeed = ConfigurationManager.AppSettings["tagFurnaceSpeed"];

        public static string tagFilterT = ConfigurationManager.AppSettings["tagFilterT"];
        public static string tagFilterLoad = ConfigurationManager.AppSettings["tagFilterLoad"];

        public static string tagScaleWeight = ConfigurationManager.AppSettings["tagScaleWeight"];
        #endregion

        #region OPC Connection vars
        public static OPCServer MyOPCServer;
        public static OPCGroups MyOPCGroup;
        public static string strServer = ConfigurationManager.AppSettings["OPCServerName"];

        // signals - test and booleans - plc vars
        public OPCGroup signalsGroup;
        public string signalsGroupName = "Signals";
        static Array signals_OPCItemIDs = Array.CreateInstance(typeof(string), 14);
        static Array signals_ItemServerHandles = Array.CreateInstance(typeof(Int32), 14);
        static Array signals_ItemServerErrors = Array.CreateInstance(typeof(Int32), 14);
        static Array signals_ClientHandles = Array.CreateInstance(typeof(Int32), 14);
        static readonly Array signals_RequestedDataTypes = Array.CreateInstance(typeof(Int16), 14);
        static readonly Array signals_AccessPaths = Array.CreateInstance(typeof(string), 14);
        static Array signals_WriteItems = Array.CreateInstance(typeof(object), 14); //this must always be object type
        public bool[] signals_Temp = new bool[14];

        // floats plc vars
        public OPCGroup floatsGroup;
        public string floatsGroupName = "Floats";
        static Array floats_OPCItemIDs = Array.CreateInstance(typeof(string), 22);
        static Array floats_ItemServerHandles = Array.CreateInstance(typeof(Int32), 22);
        static Array floats_ItemServerErrors = Array.CreateInstance(typeof(Int32), 22);
        static Array floats_ClientHandles = Array.CreateInstance(typeof(Int32), 22);
        static readonly Array floats_RequestedDataTypes = Array.CreateInstance(typeof(Int16), 22);
        static readonly Array floats_AccessPaths = Array.CreateInstance(typeof(string), 22);
        public int[] floats_Temp = new int[22];

        #endregion

        public static bool OPCConnect()
        {

            try
            {
                MyOPCServer = new OPCServer();
                MyOPCServer.Connect(strServer, "");
                MyOPCServer.OPCGroups.DefaultGroupDeadband = 0f;
                //MessageBox.Show(MyOPCServer.ServerState.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw;
            }
            return true;
        }

        public bool SignalsConnect()
        {
            try
            {
                signalsGroup = MyOPCServer.OPCGroups.Add(signalsGroupName);
                signalsGroup.UpdateRate = 100;
                signalsGroup.IsSubscribed = signalsGroup.IsActive;

                int signalsCount = 13;

                signals_OPCItemIDs.SetValue(node + tagDynamicTest, 1);

                signals_OPCItemIDs.SetValue(node + tagPota1ReadT, 2);
                signals_OPCItemIDs.SetValue(node + tagPota1ReadRpm, 3);

                signals_OPCItemIDs.SetValue(node + tagPota2ReadT, 4);
                signals_OPCItemIDs.SetValue(node + tagPota2ReadRpm, 5);

                signals_OPCItemIDs.SetValue(node + tagPota3ReadT, 6);
                signals_OPCItemIDs.SetValue(node + tagPota3ReadRpm, 7);

                signals_OPCItemIDs.SetValue(node + tagPota4ReadT, 8);
                signals_OPCItemIDs.SetValue(node + tagPota4ReadRpm, 9);

                signals_OPCItemIDs.SetValue(node + tagBurnerRead, 10);

                signals_OPCItemIDs.SetValue(node + tagFurnaceRead, 11);

                signals_OPCItemIDs.SetValue(node + tagFilterRead, 12);

                signals_OPCItemIDs.SetValue(node + tagScaleRead, 13);

                for (int i = 1; i <= signalsCount; i++)
                {
                    signals_ClientHandles.SetValue(i, i);
                }

                signalsGroup.OPCItems.DefaultIsActive = true;
                signalsGroup.OPCItems.AddItems(signalsCount, ref signals_OPCItemIDs,
                    ref signals_ClientHandles, out signals_ItemServerHandles,
                    out signals_ItemServerErrors, signals_RequestedDataTypes,
                    signals_AccessPaths);
                signalsGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(SignalsGroup_DataChange);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return true;
        }

        public void SignalsGroup_DataChange(int TransactionID, int NumItems,
            ref Array ClientHandles, ref Array ItemValues,
            ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                for (int i = 1; i <= NumItems; i++)
                {
                    for (int all = 1; all <= 13; all++)
                    {
                        if ((Convert.ToInt32(ClientHandles.GetValue(i))) == all)
                        {
                            signals_Temp[all] = Convert.ToBoolean(ItemValues.GetValue(i));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool FloatsConnect()
        {
            try
            {
                floatsGroup = MyOPCServer.OPCGroups.Add(floatsGroupName);
                floatsGroup.UpdateRate = 100;
                floatsGroup.IsSubscribed = floatsGroup.IsActive;

                int floatsCount = 21;

                floats_OPCItemIDs.SetValue(node + tagPota1T, 1);
                floats_OPCItemIDs.SetValue(node + tagPota1Rpm, 2);

                floats_OPCItemIDs.SetValue(node + tagPota2T, 3);
                floats_OPCItemIDs.SetValue(node + tagPota2Rpm, 4);

                floats_OPCItemIDs.SetValue(node + tagPota3T, 5);
                floats_OPCItemIDs.SetValue(node + tagPota3Rpm, 6);

                floats_OPCItemIDs.SetValue(node + tagPota4T, 7);
                floats_OPCItemIDs.SetValue(node + tagPota4Rpm, 8);

                floats_OPCItemIDs.SetValue(node + tagBurnerExpenseGas, 9);
                floats_OPCItemIDs.SetValue(node + tagBurnerExpenseOx, 10);
                floats_OPCItemIDs.SetValue(node + tagBurnerSumExpenseGas, 11);
                floats_OPCItemIDs.SetValue(node + tagBurnerSumExpenseOx, 12);
                floats_OPCItemIDs.SetValue(node + tagBurnerPressureGas, 13);
                floats_OPCItemIDs.SetValue(node + tagBurnerPressureOx, 14);
                floats_OPCItemIDs.SetValue(node + tagBurnerTGas, 15);
                floats_OPCItemIDs.SetValue(node + tagBurnerTOx, 16);

                floats_OPCItemIDs.SetValue(node + tagFurnaceSetpoint, 17);
                floats_OPCItemIDs.SetValue(node + tagFurnaceSpeed, 18);

                floats_OPCItemIDs.SetValue(node + tagFilterT, 19);
                floats_OPCItemIDs.SetValue(node + tagFilterLoad, 20);

                floats_OPCItemIDs.SetValue(node + tagScaleWeight, 21);

                for (int i = 1; i <= floatsCount; i++)
                {
                    floats_ClientHandles.SetValue(i, i);
                }

                floatsGroup.OPCItems.DefaultIsActive = true;
                floatsGroup.OPCItems.AddItems(floatsCount, ref floats_OPCItemIDs,
                    ref floats_ClientHandles, out floats_ItemServerHandles,
                    out floats_ItemServerErrors, floats_RequestedDataTypes,
                    floats_AccessPaths);
                floatsGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(FloatsGroup_DataChange);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return true;
        }

        public void FloatsGroup_DataChange(int TransactionID, int NumItems,
            ref Array ClientHandles, ref Array ItemValues,
            ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                for (int i = 1; i <= NumItems; i++)
                {
                    for (int all = 1; all <= 21; all++)
                    {
                        if ((Convert.ToInt32(ClientHandles.GetValue(i))) == all)
                        {
                            floats_Temp[all] = Convert.ToInt32(ItemValues.GetValue(i));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void WriteToPLC(int indexPos)
        {
            try
            {
                signals_WriteItems.SetValue(false, indexPos);
                for (int i = 1; i <= 13; i++)
                {
                    if (i != indexPos)
                    {
                        signals_WriteItems.SetValue(signals_Temp[i], i);
                    }
                }
                signalsGroup.SyncWrite(13, ref signals_ItemServerHandles, ref signals_WriteItems, out signals_ItemServerErrors);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
