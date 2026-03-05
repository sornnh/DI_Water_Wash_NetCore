using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using log4net;

public class Cls_ASPcontrol
{
    #region Define Variables
    public delegate void DelShow_addLog_Request(string sender);

    public delegate void DelShow_UpdateRelayStatus_Request(Cls_ASPcontrol sender);

    public delegate void DelShow_UpdateADC_Request(Cls_ASPcontrol sender);
    public delegate void DelShow_UpdateStatusInput(Cls_ASPcontrol sender);

    private static readonly ILog log = LogManager.GetLogger("ASPBoardLogger");

    private bool Asyncrelay = false;

    private bool _isConnected = false;

    private double[] _ADVs = new double[4];

    private double[] _ADIs = new double[4];
    private double[] _ADCs = new double[8];

    private List<byte> _buffer = new List<byte>();

    private TaskCompletionSource<string[]> _tcs = new TaskCompletionSource<string[]>();

    public string SerialCom { get; set; }

    public int Baudrate { get; set; }

    public int DataBit { get; set; }

    public Parity Parity { get; set; }

    public StopBits StopBit { get; set; }

    public SerialPort SerialPort { get; private set; }

    public string DataReceive { get; private set; }

    public string AllDataReceive { get; private set; }

    public bool[] relayStates { get; private set; }
    public bool[] inputStates { get; private set; }
    public double[] ADVs => _ADVs;
    public double[] ADIs => _ADIs;
    public double[] ADCs => _ADCs;

    public bool isConnected
    {
        get
        {
            return _isConnected;
        }
        set
        {
            _isConnected = value;
        }
    }
    public string[] SerialNumber { get; private set; } = new string[4];
    public bool _isWaitingResponse { get; private set; }
    private int _expectedLineCount = 1; // mặc định là 1 dòng
    private List<string> _receivedLines = new List<string>();

    public event DelShow_addLog_Request OnRequestAddLog;

    public event DelShow_UpdateRelayStatus_Request OnRequestUpdateRelayStatus;

    public event DelShow_UpdateADC_Request OnRequestUpdateADC;

    public event DelShow_UpdateStatusInput OnRequestUpdateStatusInput;

    private readonly SemaphoreSlim _cmdLock = new SemaphoreSlim(1, 1); //FIFO Queue

    #endregion
    public Cls_ASPcontrol()
    {
        GetASPSerialPortInformation();
        ConnectASPSerial();
        DataReceive = "";
        AllDataReceive = "";
        relayStates = new bool[30];
        inputStates = new bool[20];
        for (int i = 0; i < relayStates.Length; i++)
        {
            relayStates[i] = false;
        }
        for (int i = 0; i < inputStates.Length; i++)
        {
            inputStates[i] = false;
        }
        GetAllInOut();
    }
    public async void GetAllInOut()
    {
        await GetAllRelay();
        await GetAllInput();
    }
    public void StartRelayReading()
    {
        Asyncrelay = true;
    }

    public async Task GetAllRelay()
    {
        try
        {
            string bitString = (await SendCommandAndWaitLineResponseAsync("RR000\n"))[0].Substring(5, 30);
            bool[] result = bitString.Select((char c) => c == '1').ToArray();
            if (result.Length != 30)
            {
                log.Error("bit length incorrect: " + bitString.Length);
            }
            relayStates = result;
            UpdaterelayStatus();
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            log.Error("Error in GetAllRelay" + ex2);
        }
    }
    public async Task<bool> SetUSONProgram(int prog)
    {
        try
        {
            string cmdsetprog = $"WUProg#{prog.ToString("000")}\n";
            string responsesetprog = (await SendCommandAndWaitLineResponseAsync(cmdsetprog, 1, 3000, 5000))[0];
            if (!responsesetprog.Contains(cmdsetprog.Trim()))
            {

                log.Error("Failed to set program: " + responsesetprog);
                return false;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (await SetUSONAbort()) break;
                }
                log.Info("Set time 1 program success: " + responsesetprog);
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> SetUSONAbort()
    {
        try
        {
            string cmdsetprog = $"WUAbort\n";
            string responsesetprog = (await SendCommandAndWaitLineResponseAsync(cmdsetprog, 1, 3000, 5000))[0];
            if (!responsesetprog.Contains(cmdsetprog.Trim()))
            {
                log.Error("Failed to set program: " + responsesetprog);
                return false;
            }
            else
            {
                log.Info("Set time 1 program success: " + responsesetprog);
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
   

    public async Task<bool> GetUSONReady()
    {
        if (!isConnected) return false;
        try
        {
            string cmdgetstatus = $"RUStatus\n";
            string responstatus = (await SendCommandAndWaitLineResponseAsync(cmdgetstatus, 1, 3000, 5000))[0];
            if (responstatus.Contains(cmdgetstatus.Trim('\n')))
            {
                int index = 1 + 1 + 6;
                int BitsReady = int.Parse(responstatus.Substring(index, 1));
                if (BitsReady == 1)
                {
                    log.Debug($"Uson ready to test {responstatus}");
                    return true;
                }
                else
                {
                    log.Debug($"Uson not ready to test {responstatus}");
                    return false;
                }
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> StartUSON()
    {
        try
        {
            string cmdstart = $"WUStart\n";
            string responstart = (await SendCommandAndWaitLineResponseAsync(cmdstart, 1, 3000, 5000))[0];
            if (responstart.Contains(cmdstart.Trim()))
            {
                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> CheckUSONEnd()
    {
        try
        {
            string cmdgetstatus = $"RUStatus\n";
            string responstatus = (await SendCommandAndWaitLineResponseAsync(cmdgetstatus, 1, 3000, 5000))[0];
            if (responstatus.Contains(cmdgetstatus.Trim()))
            {
                int index = 1 + 1 + 6 + 1 + 1 + 1;
                int BitsReady = int.Parse(responstatus.Substring(index, 1));
                if (BitsReady == 1)
                {
                    log.Debug($"Uson still testing {responstatus}");
                    return true;
                }
                else
                {
                    log.Debug($"Uson end of cycle {responstatus}");
                    return false;
                }
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }
 
    public async Task<bool> RequestTriggerScanner(int Index)
    {
        SerialNumber[Index - 1] = "";
        try
        {
            string cmd = $"WS{Index.ToString("00")}\n";
            string[] response = await SendCommandAndWaitLineResponseAsync(cmd, 2, 6000, 6000);
            if (response.Length < 2) return false;
            if (response[0].Trim() == cmd.Trim())
            {
                SerialNumber[Index - 1] = response[1].Trim();
                return true;
            }
            else
            {
                log.Error((object)("ASP Serial Scanner " + Index.ToString("00") + " response: " + response[0]));
                return false;
            }
        }
        catch (Exception ex)
        {
            log.Error((object)("Error send command: " + ex.Message));
            return false;
        }

    }
    public async Task GetAllInput()
    {
        try
        {
            string GetResult = (await SendCommandAndWaitLineResponseAsync("RI000\n"))[0];
            string bitString = GetResult.Substring(5, 20);
            bool[] result = bitString.Select((char c) => c == '1').ToArray();
            if (result.Length != 20)
            {
                log.Error("bit length incorrect: " + bitString.Length);
            }
            inputStates = result;
            UpdateStatusInput();
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            log.Error("Error in GetAllInput" + ex2);
        }
    }
    public async Task SetRelayONOFFAsync(int relay, int onOff)
    {
        if (relayStates[relay - 1] == (onOff == 1))
        {
            return;
        }
        string cmd = ((onOff == 1) ? $"WR{relay:D3}1\n" : $"WR{relay:D3}0\n");
        try
        {
            await SendCommandAndWaitLineResponseAsync(cmd);
        }
        catch (Exception ex)
        {
            log.Error($"Error when send command {cmd}: " + ex.Message);
        }
    }

    public async Task<double> GetADVAsync(int iFlowChannel)
    {
        string cmd = $"RADV{iFlowChannel}\n";
        string subresponse = "";
        string response = "";
        try
        {
            response = (await SendCommandAndWaitLineResponseAsync(cmd))[0];
            if (response.Contains(cmd.Trim()))
            {
                subresponse = response.Substring(cmd.Length - 1);
            }
            _ADVs[iFlowChannel - 1] = double.Parse(subresponse);
        }
        catch (Exception ex)
        {
            log.Error($"Error when send command {cmd} with response {response} : " + ex.Message);
        }
        UpdateADC();
        return _ADVs[iFlowChannel - 1];
    }
    public async Task<double> GetADIAsync(int iFlowChannel)
    {
        string cmd = $"RADA{iFlowChannel}\n";
        try
        {
            string response = (await SendCommandAndWaitLineResponseAsync(cmd))[0];
            string subresponse = "";
            if (response.Contains(cmd.Trim()))
            {
                subresponse = response.Substring(cmd.Length - 1);
            }
            _ADIs[iFlowChannel - 1] = double.Parse(subresponse);
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            log.Error($"Error when send command {cmd}: " + ex2.Message);
        }
        UpdateADC();
        return _ADIs[iFlowChannel - 1];
    }
    public async Task<double> GetADCAsync(int iFlowChannel)
    {
        string cmd = $"RADC{iFlowChannel}\n";
        try
        {
            string response = (await SendCommandAndWaitLineResponseAsync(cmd))[0];
            string subresponse = "";
            if (response.Contains(cmd.Trim()))
            {
                subresponse = response.Substring(cmd.Length - 1);
            }
            _ADCs[iFlowChannel - 1] = double.Parse(subresponse);
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            log.Error("Lỗi gửi lệnh: " + ex2.Message);
        }
        UpdateADC();
        return _ADCs[iFlowChannel - 1];
    }
    public async Task<bool> SetRelayONOFFAsyncCheckResult(int relay, int onOff)
    {
        if (relayStates[relay - 1] == (onOff == 1))
        {
            return true;
        }
        string cmd = ((onOff == 1) ? $"WR{relay:D3}1\n" : $"WR{relay:D3}0\n");
        try
        {
            await SendCommandAndWaitLineResponseAsync(cmd);
            await GetAllRelay();
            if (relayStates[relay - 1] == (onOff == 1))
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            log.Error("Lỗi gửi lệnh: " + ex.Message);
            return false;
        }
    }
    public async Task SetAllRelayOFF()
    {
        await GetAllRelay();
        for (int i = 0; i < relayStates.Length; i++)
        {
            if (relayStates[i] == false)
            {
                continue;
            }
            int iRelay = i + 1;
            string cmd = $"WR{iRelay:D3}0\n";
            log.Debug((object)($"Set Relay {iRelay} to OFF: " + cmd));
            await SendCommandAndWaitLineResponseAsync(cmd);
        }
    }
    public async Task<bool> SetFlowRateCheckResult(int channel, int FlowRate)
    {
        try
        {
            string cmd = $"WFLOW{channel}{FlowRate:D4}\n";
            string response = (await SendCommandAndWaitLineResponseAsync(cmd))[0];
            if (response.Contains(cmd.Trim()))
            {
                return true;
            }
            log.Error((object)("Failed send command: " + response));
            return false;
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            log.Error("Error send command: " + ex2.Message);
            return false;
        }
    }
    public async void ConnectASPSerial()
    {
        try
        {
            SerialPort = new SerialPort(SerialCom, Baudrate, Parity, DataBit, StopBit);
            SerialPort.Open();
            SerialPort.DataReceived += SerialPort_DataReceived;
            _isConnected = SerialPort.IsOpen;
            DateTime dt = DateTime.Now;
            await GetAllRelay();
            AddLogText(dt.ToString("HH:mm:ss") + "+ ASP Serial open");
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            log.Error("Exception when connect to ASP Serial :" + ex2.Message);
        }
    }
    public void DisconnectASPSerial()
    {
        try
        {
            SerialPort?.Close();
            _isConnected = false;
            SerialPort.DataReceived -= SerialPort_DataReceived;
            AddLogText(DateTime.Now.ToString("HH:mm:ss") + "+ ASP Serial closed");
        }
        catch (Exception ex)
        {
            log.Error("Exception when disconnect to ASP Serial :" + ex.Message);
        }
    }

    public void ASPSerialWriteCommand(string text)
    {
        if (!_isConnected)
        {
            DisconnectASPSerial();
            Thread.Sleep(500);
            ConnectASPSerial();
            _isConnected = SerialPort.IsOpen;
            AddLogText(DateTime.Now.ToString("HH:mm:ss") + "+ ASP Serial Re-open");
        }
        try
        {
            AddLogText(DateTime.Now.ToString("HH:mm:ss") + "+ ASP Serial write: " + text);
            SerialPort.Write(text);
        }
        catch (Exception ex)
        {
            log.Error("Exception when write to ASP Serial :" + ex.Message);
        }
    }
    public void UpdateStatusInput()
    {
        this.OnRequestUpdateStatusInput?.Invoke(this);
    }

    public void UpdateADC()
    {
        this.OnRequestUpdateADC?.Invoke(this);
    }

    public void UpdaterelayStatus()
    {
        this.OnRequestUpdateRelayStatus?.Invoke(this);
    }

    public void AddLogText(string _text)
    {
        this.OnRequestAddLog?.Invoke(_text);
    }

    public async Task<string[]> SendCommandAndWaitLineResponseAsync(
                  string command,
                  int expectedLineCount = 1,
                  int timeoutResponse = 1500,
                  int timeoutAwait = 7000)
    {
        log.Debug($"[{DateTime.Now:HH:mm:ss.fff}] → WaitAsync enter");
        await _cmdLock.WaitAsync();
        log.Debug($"[{DateTime.Now:HH:mm:ss.fff}] → WaitAsync passed");
        try
        {
            if (SerialPort == null || !SerialPort.IsOpen)
            {
                try
                {
                    SerialPort = new SerialPort(SerialCom, Baudrate, Parity, DataBit, StopBit);
                    SerialPort.Open();
                    SerialPort.DataReceived -= SerialPort_DataReceived;
                    SerialPort.DataReceived += SerialPort_DataReceived;
                    _isConnected = true;
                    AddLogText($"{DateTime.Now:HH:mm:ss.fff} ASP Serial Re-open");
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Serial port can not re-open.");
                }
            }
            await WaitUntilNoPendingResponse(timeoutAwait);
            _isWaitingResponse = true;
            _buffer.Clear();
            _receivedLines.Clear();
            _expectedLineCount = expectedLineCount;
            var tcs = new TaskCompletionSource<string[]>(TaskCreationOptions.RunContinuationsAsynchronously);
            _tcs = tcs;
            try
            {
                AddLogText($"{DateTime.Now:HH:mm:ss.fff} ASP Serial write: {command}");
                log.Debug($"{DateTime.Now:HH:mm:ss.fff} ASP Serial write: {command}");
                SerialPort.Write(command);

                Task completedTask = await Task.WhenAny(tcs.Task, Task.Delay(timeoutResponse));
                log.Debug($"{DateTime.Now:HH:mm:ss.fff} task complete: {completedTask == tcs.Task}");
                if (completedTask == tcs.Task)
                {
                    string[] response = await tcs.Task; // đủ dòng
                    log.Debug($"{DateTime.Now:HH:mm:ss.fff} task result: {response[0]}");
                    return response;
                }

                _tcs = null;
                throw new TimeoutException("Timeout waiting for response.");
            }
            finally
            {
                _isWaitingResponse = false;
                if (_tcs == tcs)
                    _tcs = null;
            }
        }
        finally
        {
            _cmdLock.Release();  // 🔓 cho command kế tiếp chạy
        }
    }
    private async Task WaitUntilNoPendingResponse(int maxWaitMs = 5000, int pollIntervalMs = 50)
    {
        int waited = 0;
        while (_isWaitingResponse)
        {
            await Task.Delay(pollIntervalMs);
            waited += pollIntervalMs;
            if (waited >= maxWaitMs)
            {
                throw new TimeoutException("Timeout waiting for previous response to complete.");
            }
        }
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            SerialPort sp = (SerialPort)sender;
            string incoming = sp.ReadExisting();
            _buffer.AddRange(Encoding.ASCII.GetBytes(incoming));

            while (true)
            {

                int newLineIndex = _buffer.IndexOf(10); // LF (\n)
                if (newLineIndex < 0)
                    break;
                byte[] lineBytes = _buffer.Take(newLineIndex + 1).ToArray();
                _buffer.RemoveRange(0, newLineIndex + 1);
                string line = Encoding.ASCII.GetString(lineBytes).Trim();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    _receivedLines.Add(line);
                    AddLogText($"{DateTime.Now:HH:mm:ss.fff} ASP Serial read: {line}");
                    // chỉ set nếu vẫn còn người chờ (_tcs != null)
                    if (_tcs != null && _receivedLines.Count >= _expectedLineCount)
                    {
                        _tcs.TrySetResult(_receivedLines.ToArray());
                        _tcs = null; // clear để không set lại sau timeout
                        _receivedLines.Clear();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in SerialPort_DataReceived", ex);
            _tcs?.TrySetException(ex);
            _tcs = null;
        }
    }
    public void GetASPSerialPortInformation()
    {
        SerialCom = ClsDefine.DefineASP.PortName;
        Baudrate = ClsDefine.DefineASP.Baudrate;
        DataBit = ClsDefine.DefineASP.DataBit;
        switch (ClsDefine.DefineASP.Parity)
        {
            case "None":
                Parity = Parity.None;
                break;
            case "Odd":
                Parity = Parity.Odd;
                break;
            case "Even":
                Parity = Parity.Even;
                break;
        }
        switch (ClsDefine.DefineASP.StopBit)
        {
            case "One":
                StopBit = StopBits.One;
                break;
            case "OnePointFive":
                StopBit = StopBits.OnePointFive;
                break;
            case "Two":
                StopBit = StopBits.Two;
                break;
        }
    }
}


