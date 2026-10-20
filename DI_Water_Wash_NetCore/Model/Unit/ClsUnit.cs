using log4net;
using Microsoft.VisualBasic.Logging;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

public class ClsUnit
{
    private string _AssyPN;
    private string _WO;
    private string _StationID;
    private bool _DualFixture = false;
    private Form _mainForm;
    private string _OperatorID;
    private static readonly ILog log = LogManager.GetLogger("SequenceTestLogger");
    public CancellationTokenSource _ctsTest;
    private bool _isDisposing;
    public bool DualFixture
    {
        get { return _DualFixture; }
        set { _DualFixture = value; }
    }
    #region ConfigVerify
    public string WO => _WO;
    public string StationID => _StationID;
    public string AssyPN => _AssyPN;
    public string OperatorID => _OperatorID;
    public string Description { get; private set; }
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
    #region Fixture lifetime
    public int iQDLifeTime { get; private set; }
    public int iOringLifeTime { get; private set; }

    #endregion
    #region PressureDwell
    public float fMinPressureDwellFill { get; private set; } //Set_Pressure
    public float fMaxPressureDwellFill { get; private set; } //Max_Set_Pressure
    public float fDwellTime { get; private set; } //Dwell_Time 
    public int iNumberofDwellChecks { get; private set; } //PDwell_NSteps
    public float fMaxPressureDwellDrop { get; private set; } //Max_drop
    public float fMinPressureDwellDrop { get; private set; } //Min_drop
    #endregion

    #region Config SN in range WO
    public bool bVerify_SN_in_WO { get; private set; } = false;
    public int iSNin_WO_Ctl_String_Length { get; private set; }
    public int iSNin_WO_Ctl_String_Offset { get; private set; }
    private string Customer_Name;
    public int iFirst_SN_in_WO { get; private set; }
    public int iLast_SN_in_WO { get; private set; }
    #endregion
    private int _UnitIndex;
    public Cls_SequenceCommon clsSequenceCommon { get; private set; }
    public Cls_SequenceTest clsSequenceTest { get; private set; }
    public ClsUnit(string AssyPN, string WorkOder, Form frmMain, string StationID,int _uIndex, string OperatorID)
    {
        _UnitIndex = _uIndex;
        _AssyPN = AssyPN;
        _WO = WorkOder;
        _StationID = StationID;
        _mainForm = frmMain;
        _OperatorID = OperatorID;
        GenerateProcessTesting();
        GetLimitTesting();
        GetSNVerify();
        clsSequenceCommon = new Cls_SequenceCommon(_uIndex);
        Thread thrcommon = new Thread(funThreadCommon);
        thrcommon.IsBackground = true;
        thrcommon.Name = "ThreadCommon";
        thrcommon.Start();
        clsSequenceTest = new Cls_SequenceTest(_uIndex, frmMain);
        _ctsTest = new CancellationTokenSource();
        // Sử dụng Task thay vì Thread
        Task.Run(() => funThreadTestAsync(_ctsTest.Token), _ctsTest.Token);
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
    private async Task funThreadTestAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(5000, token).ConfigureAwait(false);

            while (!token.IsCancellationRequested && !_isDisposing)
            {
                try
                {
                    // Await trực tiếp — KHÔNG bọc trong Task.Run
                    await clsSequenceTest.LoopTest().ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Cancel trong LoopTest → thoát luôn
                    throw;
                }
                catch (Exception e)
                {
                    log.Error("Error when loop task: " + e.ToString());
                }

                // Delay giữa các lần gọi — chỉ chạy sau khi LoopTest xong
                await Task.Delay(100, token).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            log.Info("ThreadTest stopped normally");  // Info, không phải Error
        }
        catch (Exception ex)
        {
            log.Error($"Fatal error in ThreadTest: {ex.Message}", ex);
        }
    }
    private void GetLimitTesting()
    {
        var responseTask = Task.Run(() => API.productionLimitService.GetDIWaterWashLimitAsync(_AssyPN));
        var response = responseTask.GetAwaiter().GetResult();

        if (response == null || !response.Success)
        {
            using (FrmMessageError error = new FrmMessageError($"Database don't have parameter for this Assy Part Number:\r\n {_AssyPN}", Color.Red, Color.Black, 150000))
            {
                error.ShowDialog();
                Environment.Exit(0);
            }
        }
        iWashing_Time = response.WashingTime ?? 0;
        iPre_Washing_Time = response.PreWashingTime ?? 0;
        iWash_Cycle = response.WashCycles ?? 0;
        bReverse_Washing_Flow = response.ReverseWashingFlow ?? false;
        iDI_Reverse_Washing_Time = response.DIReverseWashingTime ?? 0;
        bReverse_DI_Flushing_Flow = response.ReverseDIFlushingFlow ?? false;
        iDI_Drying_Time = response.DIDryingTime ?? 0;
        iReverse_DI_Drying_Time = response.ReverseDIDryingTime ?? 0;
        dDi_Flow_Rate = response.DIFlowRate ?? 0;
        dDi_Flow_Tol = response.DIFlowTolerance ?? 0;
        dDI_Max_AirPressure = response.DIMaxAirPressure ?? 0;
        dDI_Min_AirPressure = response.DIMinAirPressure ?? 0;
        dDI_Max_WaterPressure = response.DIMaxWaterPressure ?? 0;
        dDI_Min_WaterPressure = response.DIMinWaterPressure ?? 0;
        bCheck_DI_Huminity = response.CheckDIHumidity ?? false;
        iDI_Max_Huminity = response.DIMaxHumidity ?? 0;
    }
    private void GenerateProcessTesting()
    {
        var responseTask = Task.Run(() => API.processService.GetProcessListAsync(_AssyPN));
        var response = responseTask.GetAwaiter().GetResult();

        if (response != null && response.Success && response.ProcessList != null && response.ProcessList.Count > 0)
        {
            verifyFlags = response.ProcessList.ToArray();
        }
        else
        {
            MessageBox.Show("No data found for the given Assy_PN.");
            verifyFlags = new string[1] { "" };
        }
    }

    public void GetSNVerify()
    {
        var responseTask = Task.Run(() => API.commonFunctionService.GetSNVerifyAsync(_AssyPN, _WO));
        var response = responseTask.GetAwaiter().GetResult();

        if (response != null && response.Success)
        {
            SN_Length = response.SN_Length;
            PN_Ctl_String = response.PN_Ctl_String;
            Ctl_Str_Length = response.Ctl_Str_Length;
            Ctl_Str_Offset = response.Ctl_Str_Offset;
            Customer_Name = response.Customer_Name;
            iSNin_WO_Ctl_String_Length = response.Serial_Length;
            iSNin_WO_Ctl_String_Offset = response.SN_Offset;
            Description = response.Description;
            bVerify_SN_in_WO = response.Verify_SN_in_WO;
            Verify_Linked_Part = response.Verify_Linked_Part;
        }
    }
}

