using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

public class ClsUnit
{
    private Cls_DBMsSQL ParameterDB = new Cls_DBMsSQL();
    private Cls_DBMsSQL ProductionDB = new Cls_DBMsSQL();
    private string _AssyPN;
    private string _WO;
    private string _StationID;
    private bool _DualFixture = false;
    private Form _mainForm;
    public bool DualFixture
    {
        get { return _DualFixture; }
        set { _DualFixture = value; }
    }
    #region ConfigVerify
    public string WO => _WO;
    public string StationID => _StationID;
    public string AssyPN => _AssyPN;
    public bool Verify_Linked_Part { get; private set; }
    public bool Verify_Serial_No { get; private set; }

    public bool Verify_Thermal_Test { get; private set; }

    public bool Verify_Fans_Test { get; private set; }

    public bool Verify_Flatness { get; private set; }

    public bool Verify_Leak_Test { get; private set; }

    public bool Verify_Pressure_Dwell { get; private set; }

    public bool Verify_DeltaT_Test { get; private set; }

    public bool Verify_Bubble_Test { get; private set; }

    public bool Verify_PartN_Revisions { get; private set; }

    public bool Verify_High_Pressure { get; private set; }

    public bool Verify_Flow_Test { get; private set; }

    public bool Verify_Humidity { get; private set; }

    public bool Verify_Final_Inspection { get; private set; }

    public bool Verify_QuickQ_Test { get; private set; }

    public bool Verify_Links_at_Thermal { get; private set; }
    #endregion
    #region Config Process, Serial
    public string[] verifyFlags { get; private set; }
    public int iMax_Test_Attempts { get; private set; }
    public int iTest_to_Pass_if_Fail { get; private set; }
    public int SN_Length { get; private set; }
    public string PN_Ctl_String { get; private set; }
    public int Ctl_Str_Length { get; private set; }
    public int Ctl_Str_Offset { get; private set; }
    #endregion
    #region DI_Water_Washing
    public int iWash_Cycle { get; private set; }

    public int iPre_Washing_Time { get; private set; }

    public int iWashing_Time { get; private set; }

    public bool bReverse_Washing_Flow { get; private set; }

    public int iDI_Reverse_Washing_Time { get; set; }

    public bool bReverse_DI_Flushing_Flow { get; private set; }

    public int iDI_Drying_Time { get; set; }

    public int iReverse_DI_Drying_Time { get; set; }

    public double dDi_Flow_Rate { get; private set; }
    public double dDi_Flow_Tol { get; private set; }
    public double dDI_Max_AirPressure { get; private set; }
    public double dDI_Min_AirPressure { get; private set; }
    public double dDI_Max_WaterPressure { get; private set; }
    public double dDI_Min_WaterPressure { get; private set; }
    public bool bCheck_DI_Huminity { get; private set; }
    public int iDI_Max_Huminity { get; private set; }
    #endregion
    #region Helium_Leak_Test
    public int iPre_Vacuum { get; private set; }
    public int iRoughing_Time_On { get; private set; }
    public float fRoughing_Time { get; private set; }
    public float fGross_Leak_Pressure { get; private set; }
    public int iHelium_Valve_Open_Time { get; private set; }
    public float fNormal_pressure { get; private set; }
    public int iOpeningDelay { get; private set; }
    public int iVentTimeMin { get; private set; }
    public int iVentTimeSec { get; private set; }
    public int iLeak_Test_Time { get; private set; }
    public float fMin_Threshold { get; private set; }
    public float fReject_Threshold { get; private set; }
    public float fMin_He_Pressure { get; private set; }
    public float fMax_He_Pressure { get; private set; }
    public bool bCheck_He_Tank_Pressure { get; private set; }
    public float fHe_Volume_Max { get; private set; }
    public float fHe_Volume_Min { get; private set; }
    public bool bCheck_He_Volume { get; private set; }
    public float fN2_Pressure_Max { get; private set; }
    public float fN2_Pressure_Min { get; private set; }
    public bool bCheck_N2_Pressure { get; private set; }
    public float fChamber_Vacuum_Max { get; private set; }
    public float fChamber_Vacuum_Min { get; private set; }
    public bool bCheck_Chamber_Vacuum { get; private set; }
    public float fProduct_Vacuum_Max { get; private set; }
    public float fProduct_Vacuum_Min { get; private set; }
    public bool bCheck_Product_Vacuum { get; private set; }
    public float fHe_Fill_Time_Min { get; private set; }
    public float fHe_Fill_Time_Max { get; private set; }
    public bool bCheck_He_Fill_Time { get; private set; }
    public int iASM_340_Test_Mode {  get; private set; }
    public bool bPre_Vacuum { get; private set; }
    public int iVent_Valve_Control { get; private set; }
    public int iLeak_Rate_Units { get; private set; }
    #endregion
    #region Hot_Air_Drying
    public int iNumber_of_Drying_Clycle { get; private set; }

    public int iHot_Air_Flushing_Time { get; private set; }

    public int iHot_Air_Reverse_Flow_Time { get; private set; }

    public int iMax_Drying_Humidity { get; private set; }

    public int iN2_Pressure { get; private set; }

    public int iN2_Drying_Time { get; private set; }

    public int iVacuum_Pressure { get; private set; }

    public int iVac_Baking_Temp { get; private set; }

    public int iVac_Max_Humidity { get; private set; }

    public int iBaking_Time { get; private set; }

    public bool bReverse_Hot_Air_Flushing_Flow { get; private set; }

    public bool bUse_Nitrogen_to_Dry { get; private set; }

    public bool bCheck_Humidity { get; private set; }
    #endregion
    #region Pressure_Decay
    public double iDecayMaxHumidity { get; private set; }
    public double iDecayStabilize_Time { get; private set; }
    public double iDecayTest_Time { get; private set; }
    public double iDecayPressure { get; private set; }
    public double iDecayPressureTolerance { get; private set; }
    public double iDecayMax_dP { get; private set; }
    public bool bDecay_Pre_Drying { get; private set; }
    #endregion
    #region Fill_N2
    public bool bN2Fill_to_Ship { get; private set; }
    public double iN2FillPressure { get; private set; }
    public double iN2FillPressureTolerance { get; private set; }
    #endregion
    #region PressureDwell
    public float fMinPressureDwellFill { get; private set; } //Set_Pressure
    public float fMaxPressureDwellFill { get; private set; } //Max_Set_Pressure
    public float fDwellTime { get; private set; } //Dwell_Time 
    public int iNumberofDwellChecks { get; private set; } //PDwell_NSteps
    public float fMaxPressureDwellDrop { get; private set; } //Max_drop
    public float fMinPressureDwellDrop { get; private set; } //Min_drop
    #endregion
    private int _UnitIndex;
    public Cls_SequenceCommon clsSequenceCommon { get; private set; }
    public Cls_SequenceTest clsSequenceTest { get; private set; }
    public ClsUnit(string AssyPN, string WorkOder, Form frmMain, string StationID,int _uIndex)
    {
        _UnitIndex = _uIndex;
        _AssyPN = AssyPN;
        _WO = WorkOder;
        _StationID = StationID;
        _mainForm = frmMain;
        ParameterDB.Initialize("10.102.4.20", "Parameters_SZ", "readuser", "Boyd@2025");
        ParameterDB.Open();
        ProductionDB.Initialize("10.102.4.20", "Production_SZ", "readuser", "Boyd@2025");
        ProductionDB.Open();
        GenerateProcessTesting();
        GetLimitTesting();
        GetSNVerify();
        clsSequenceCommon = new Cls_SequenceCommon(_uIndex);
        Thread thrcommon = new Thread(funThreadCommon);
        thrcommon.IsBackground = true;
        thrcommon.Name = "ThreadCommon";
        thrcommon.Start();
        clsSequenceTest = new Cls_SequenceTest(_uIndex, frmMain);
        Thread thrtest = new Thread(funThreadTest);
        thrtest.IsBackground = true;
        thrtest.Name = "ThreadTest";
        thrtest.Start();
    }
    private void funThreadCommon()
    {
        Thread.Sleep(5000);
        while (true)
        {
            clsSequenceCommon.LoopTowerLamp();
            Thread.Sleep(1000);
        }
    }
    private void funThreadTest()
    {
        Thread.Sleep(5000);
        while (true)
        {
            clsSequenceTest.LoopTest();
            Thread.Sleep(1000);
        }
    }
    private void GetLimitTesting()
    {
        string query = "SELECT *FROM dbo.Leak_Test_Limits Where Assy_PN ='" + _AssyPN + "' order by Date_Time";
        DataTable dt = ParameterDB.ExecuteQuery(query);
        if (dt == null || dt.Rows.Count == 0)
        {
            using (FrmMessageError error = new FrmMessageError($"Database don't have parameter for this Assy Part Number:\r\n {_AssyPN}", Color.Red, Color.Black, 150000))
            {
                error.ShowDialog();
                Environment.Exit(0);
            }
        }
        GetLimit_DIWaterWash(dt);
        string queryHeliumLeakTest = $"SELECT *FROM [Parameters_SZ].[dbo].[Helium_Leak_Test_Limits] Where Assy_PN ='{_AssyPN}' order by Date_Time";
        DataTable dtHeliumLeakTest = ParameterDB.ExecuteQuery(queryHeliumLeakTest);
        if(dtHeliumLeakTest == null || dtHeliumLeakTest.Rows.Count ==0)
        {
            using (FrmMessageError error = new FrmMessageError($"Database don't have Helium Leak Test parameter for this Assy Part Number:\r\n {_AssyPN}", Color.Red, Color.Black,150000))
            {
                error.ShowDialog();
                Environment.Exit(0);
            }    
        }    
        GetLimit_HeliumLeakageTest(dtHeliumLeakTest);
        GetLimit_DryingTest(dt);
        GetLimit_DecayTest(dt);
        GetLimit_DwellTest(dt);
        GetTestCountLimit(dt);
    }
    private void GetTestCountLimit(DataTable dt)
    {
        DataRow dataRow = dt.Rows[dt.Rows.Count - 1];
        foreach (DataColumn column in dataRow.Table.Columns)
        {
            switch (column.ColumnName)
            {
                case "Max_Test_Attempts":
                    iMax_Test_Attempts = Convert.ToInt32(dataRow[column]);
                    break;
                case "Test_to_Pass_if_Faily":
                    iTest_to_Pass_if_Fail = Convert.ToInt32(dataRow[column]);
                    break;
            }
        }
    }
    private void GetLimit_DecayTest(DataTable dt)
    {
        DataRow dataRow = dt.Rows[dt.Rows.Count - 1];
        foreach (DataColumn column in dataRow.Table.Columns)
        {
            switch (column.ColumnName)
            {
                case "PDc_Max_Humidity":
                    iDecayMaxHumidity = Convert.ToDouble(dataRow[column]);
                    break;
                case "Stabilize_Time":
                    iDecayStabilize_Time = Convert.ToDouble(dataRow[column]);
                    break;
                case "Decay_Time":
                    iDecayTest_Time = Convert.ToDouble(dataRow[column]);
                    break;
                case "Pressure":
                    iDecayPressure = Convert.ToDouble(dataRow[column]);
                    break;
                case "P_Tolerance":
                    iDecayPressureTolerance = Convert.ToDouble(dataRow[column]);
                    break;
                case "Max_dP":
                    iDecayMax_dP = Convert.ToDouble(dataRow[column]);
                    break;
                case "Pre_Drying":
                    bDecay_Pre_Drying = Convert.ToBoolean(dataRow[column]);
                    break;
                case "Fill_N2_to_Ship":
                    bN2Fill_to_Ship = Convert.ToBoolean(dataRow[column]);
                    break;
                case "Fill_Pressure":
                    iN2FillPressure = Convert.ToDouble(dataRow[column]);
                    break;
                case "FP_Tolerance":
                    iN2FillPressureTolerance = Convert.ToDouble(dataRow[column]);
                    break;
            }
        }
    }

    //#region PressureDwell
    //public float fMinPressureDwellFill { get; private set; } //Set_Pressure
    //public float fMaxPressureDwellFill { get; private set; } //Max_Set_Pressure
    //public float fDwellTime { get; private set; } //Dwell_Time 
    //public int iNumberofDwellChecks { get; private set; } //PDwell_NSteps
    //public float fMaxPressureDrop { get; private set; } //Max_drop
    //public float fMinPressureDrop { get; private set; } //Min_drop
    //#endregion
    private void GetLimit_DwellTest(DataTable dt)
    {
        DataRow dataRow = dt.Rows[dt.Rows.Count - 1];
        foreach (DataColumn column in dataRow.Table.Columns)
        {
            switch (column.ColumnName)
            {
                case "Set_Pressure":
                    if (float.TryParse(dataRow[column]?.ToString(), out var result))
                    {
                        fMinPressureDwellFill = result;
                    }
                    break;
                case "Max_Set_Pressure":
                    if (float.TryParse(dataRow[column]?.ToString(), out var result1))
                    {
                        fMaxPressureDwellFill = result1;
                    }
                    break;
                case "Dwell_Time":
                    if (float.TryParse(dataRow[column]?.ToString(), out var result2))
                    {
                        fDwellTime = result2;
                    }
                    break;
                case "PDwell_NSteps":
                    if (int.TryParse(dataRow[column]?.ToString(), out var result3))
                    {
                        iNumberofDwellChecks = result3;
                    }
                    break;
                case "Max_drop":
                    if (float.TryParse(dataRow[column]?.ToString(), out var result4))
                    {
                        fMaxPressureDwellDrop = result4;
                    }
                    break;
                case "Min_Drop":
                    if (float.TryParse(dataRow[column]?.ToString(), out var result5))
                    {
                        fMinPressureDwellDrop = result5;
                    }
                    break;
            }
        }
    }
    private void GetLimit_DryingTest(DataTable dt)
    {
        DataRow dataRow = dt.Rows[dt.Rows.Count - 1];
        foreach (DataColumn column in dataRow.Table.Columns)
        {
            switch (column.ColumnName)
            {
                case "Number_of_Drying_cycles":
                    iNumber_of_Drying_Clycle = Convert.ToInt32(dataRow[column]);
                    break;
                case "Hot_Air_Flushing_Time":
                    iHot_Air_Flushing_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Hot_Air_Reverse_Flow_Time":
                    iHot_Air_Reverse_Flow_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Max_Humidity":
                    iMax_Drying_Humidity = Convert.ToInt32(dataRow[column]);
                    break;
                case "N2_Pressure":
                    iN2_Pressure = Convert.ToInt32(dataRow[column]);
                    break;
                case "Drying_Time":
                    iN2_Drying_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Vacuum_Pressure":
                    iVacuum_Pressure = Convert.ToInt32(dataRow[column]);
                    break;
                case "Vac_Baking_Temp":
                    iVac_Baking_Temp = Convert.ToInt32(dataRow[column]);
                    break;
                case "Vac_Max_Humidity":
                    iVac_Max_Humidity = Convert.ToInt32(dataRow[column]);
                    break;
                case "Baking_Time":
                    iBaking_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Reverse_Hot_Flushing_Flow":
                    bReverse_Hot_Air_Flushing_Flow = Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "Use_Nitrogen_to_Dry":
                    bUse_Nitrogen_to_Dry = Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "Check_Humidity":
                    bCheck_Humidity = Convert.ToInt32(dataRow[column]) == 1;
                    break;
            }
        }
    }

    private void GetLimit_HeliumLeakageTest(DataTable dt)
    {
        DataRow dataRow = dt.Rows[dt.Rows.Count - 1];
        foreach (DataColumn column in dataRow.Table.Columns)
        {
            switch (column.ColumnName)
            {
                case "Pre_Vacuum":
                    bPre_Vacuum = Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "Roughing_Time_On":
                    iRoughing_Time_On = Convert.ToInt32(dataRow[column]);
                    break;
                case "Vent_Valve_Control":
                    iVent_Valve_Control = Convert.ToInt32(dataRow[column]);
                    break;
                case "Leak_Rate_Units":
                    iLeak_Rate_Units = Convert.ToInt32(dataRow[column]);
                    break;
                case "Vacuum_Time":
                    iPre_Vacuum = Convert.ToInt32(dataRow[column]);
                    break;
                case "Roughing_Time":
                    fRoughing_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Gross_Leak_Pressure":
                    if (float.TryParse(dataRow[column]?.ToString(), out var result6))
                    {
                        fGross_Leak_Pressure = result6;
                    }
                    break;
                case "Helium_Valve_Open_Time":
                    iHelium_Valve_Open_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Normal_Pressure":
                    if (float.TryParse(dataRow[column]?.ToString(), out var result5))
                    {
                        fNormal_pressure = result5;
                    }
                    break;
                case "Opening_Delay":
                    iOpeningDelay = Convert.ToInt32(dataRow[column]);
                    break;
                case "Vent_Minutes":
                    iVentTimeMin = Convert.ToInt32(dataRow[column]);
                    break;
                case "Vent_Seconds":
                    iVentTimeSec = Convert.ToInt32(dataRow[column]);
                    break;
                case "Leak_Test_Time":
                    iLeak_Test_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Max_Reject_Threshold":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result2))
                        {
                            fReject_Threshold = result2;
                        }
                        break;
                    }
                case "Min_Reject_Threshold":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result))
                        {
                            fMin_Threshold = result;
                        }
                        break;
                    }
                case "Min_He_Pressure":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result3))
                        {
                            fMin_He_Pressure = result3;
                        }
                        break;
                    }
                case "Max_He_Pressure":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result4))
                        {
                            fMax_He_Pressure = result4;
                        }
                        break;
                    }
                case "ASM_340_Test_Mode":
                    iASM_340_Test_Mode = dataRow[column] != DBNull.Value ? Convert.ToInt32(dataRow[column]) : 2;
                    break;
                case "Check_He_Tank_Pressure":
                    bCheck_He_Tank_Pressure = dataRow[column] != DBNull.Value && Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "He_Volume_Max":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result7))
                        {
                            fHe_Volume_Max = result7;
                        }
                        break;
                    }
                case "He_Volume_Min":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result8))
                        {
                            fHe_Volume_Min = result8;
                        }
                        break;
                    }
                case "Check_He_Volume":
                    bCheck_He_Volume = dataRow[column] != DBNull.Value && Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "N2_Tank_Pressure_Max":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result9))
                        {
                            fN2_Pressure_Max = result9;
                        }
                        break;
                    }
                case "N2_Tank_Pressure_Min":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result10))
                        {
                            fN2_Pressure_Min = result10;
                        }
                        break;
                    }
                case "Check_N2_Tank_Pressure":
                    bCheck_N2_Pressure = dataRow[column] != DBNull.Value && Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "Vacuum_Chamber_Pressure_Max":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result11))
                        {
                            fChamber_Vacuum_Max = result11;
                        }
                        break;
                    }
                case "Vacuum_Chamber_Pressure_Min":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result12))
                        {
                            fChamber_Vacuum_Min = result12;
                        }
                        break;
                    }
                case "Check_Vacuum_Chamber_Pressure":
                    bCheck_Chamber_Vacuum = dataRow[column] != DBNull.Value && Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "Product_Vacuum_Pressure_Max":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result13))
                        {
                            fProduct_Vacuum_Max = result13;
                        }
                        break;
                    }
                case "Product_Vacuum_Pressure_Min":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result14))
                        {
                            fProduct_Vacuum_Min = result14;
                        }
                        break;
                    }
                case "Check_Product_Vacuum_Pressure":
                    bCheck_Product_Vacuum = dataRow[column] != DBNull.Value && Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "He_Fill_Time_Max":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result15))
                        {
                            fHe_Fill_Time_Max = result15;
                        }
                        break;
                    }
                case "He_Fill_Time_Min":
                    {
                        if (float.TryParse(dataRow[column]?.ToString(), out var result16))
                        {
                            fHe_Fill_Time_Min = result16;
                        }
                        break;
                    }
                case "Check_He_Fill_Time":
                    bCheck_He_Fill_Time = dataRow[column] != DBNull.Value && Convert.ToInt32(dataRow[column]) == 1;
                    break;
            }
        }
    }

    private void GetLimit_DIWaterWash(DataTable dt)
    {
        DataRow dataRow = dt.Rows[dt.Rows.Count - 1];
        foreach (DataColumn column in dataRow.Table.Columns)
        {
            switch (column.ColumnName)
            {
                case "Number_of_Wash_cycles":
                    iWash_Cycle = Convert.ToInt32(dataRow[column]);
                    break;
                case "DI_Pre_Wash_Time":
                    iPre_Washing_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "DI_Washing_Time":
                    iWashing_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Reverse_Washing_Flow":
                    bReverse_Washing_Flow = Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "DI_Reverse_Washing_Time":
                    iDI_Reverse_Washing_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Reverse_DI_Flushing_Flow":
                    bReverse_DI_Flushing_Flow = Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "DI_Drying_Time":
                    iDI_Drying_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "Reversed_DI_Drying_Time":
                    iReverse_DI_Drying_Time = Convert.ToInt32(dataRow[column]);
                    break;
                case "DI_Flow_Rate":
                    dDi_Flow_Rate = Convert.ToDouble(dataRow[column]);
                    break;
                case "DI_Flow_Tol":
                    dDi_Flow_Tol = Convert.ToDouble(dataRow[column]);
                    break;
                case "Flush_Air_Pressure_Max":
                    dDI_Max_AirPressure = Convert.ToDouble(dataRow[column]);
                    break;
                case "Flush_Air_Pressure_Min":
                    dDI_Min_AirPressure = Convert.ToDouble(dataRow[column]);
                    break;
                case "Water_Pressure_Max":
                    dDI_Max_WaterPressure = Convert.ToDouble(dataRow[column]);
                    break;
                case "Water_Pressure_Min":
                    dDI_Min_WaterPressure = Convert.ToDouble(dataRow[column]);
                    break;
                case "Check_DI_Humidity":
                    bCheck_DI_Huminity = Convert.ToInt32(dataRow[column]) == 1;
                    break;
                case "DI_Max_Humidity":
                    iDI_Max_Huminity = Convert.ToInt32(dataRow[column]);
                    break;
            }
        }
    }
    private void GenerateProcessTesting()
    {
        string query = "SELECT * FROM Process_Flow_for_Part_Numbers WHERE Assy_PN = '" + _AssyPN + "' order by Date_Time";
        DataTable dt = ParameterDB.ExecuteQuery(query);
        LoadVerifyValues(dt);
    }

    public void LoadVerifyValues(DataTable dt)
    {
        if (dt.Rows.Count == 0)
        {
            MessageBox.Show("No data found for the given Assy_PN.");
            verifyFlags = new string[1] { "" };
            return;
        }
        DataRow dataRow = dt.Rows[0];
        foreach (DataColumn column in dt.Columns)
        {
            string columnName = column.ColumnName;
            if (columnName == "Process_List")
            {
                string[] array = dataRow[columnName].ToString().Split(',');
                verifyFlags = new string[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    verifyFlags[i] = array[i].Trim();
                }
            }
        }
    }

    public void GetSNVerify()
    {
        string query = $@"SELECT 
                            SN_Length,
                            PN_Ctl_String,
                            Ctl_Str_Length,
                            Ctl_Str_Offset,
                            [No_Linked_Units],
                            [No_Linked_PN_2],
                            [No_Linked_PN_3],
                            [No_Linked_PN_4],
                            [No_Linked_PN_5],
                            [No_Linked_PN_6],
                            [No_Linked_PN_7],
                            [No_Linked_PN_8]  
                            FROM [Parameters_SZ].[dbo].[Aavid_Part_Numbers] WHERE [ASSY_PN] = '{AssyPN}' order by Date_Time";
        DataTable dataTable = ParameterDB.ExecuteQuery(query);
        DataRow dataRow = dataTable.Rows[dataTable.Rows.Count - 1];
        foreach (DataColumn column in dataRow.Table.Columns)
        {
            switch (column.ColumnName)
            {
                case "SN_Length":
                    SN_Length = Convert.ToInt32(dataRow[column]);
                    break;
                case "PN_Ctl_String":
                    PN_Ctl_String = dataRow[column].ToString();
                    break;
                case "Ctl_Str_Length":
                    Ctl_Str_Length = Convert.ToInt32(dataRow[column]);
                    break;
                case "Ctl_Str_Offset":
                    Ctl_Str_Offset = Convert.ToInt32(dataRow[column]);
                    break;
            }
        }
        string[] linkedColumns = new[]
                                {
                                    "No_Linked_Units",
                                    "No_Linked_PN_2",
                                    "No_Linked_PN_3",
                                    "No_Linked_PN_4",
                                    "No_Linked_PN_5",
                                    "No_Linked_PN_6",
                                    "No_Linked_PN_7",
                                    "No_Linked_PN_8"
                                };
        foreach (string col in linkedColumns)
        {
            if (dataTable.Columns.Contains(col))
            {
                object val = dataRow[col];
                if (val != DBNull.Value && Convert.ToInt32(val) > 0)
                {
                    Verify_Linked_Part = true;
                    break;
                }
            }
        }
    }
}

