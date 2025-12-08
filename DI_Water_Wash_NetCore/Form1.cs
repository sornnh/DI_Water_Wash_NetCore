using log4net;
using Microsoft.VisualBasic.Logging;
using System.Diagnostics;
using System.Management;

namespace DI_Water_Wash_NetCore
{
    public partial class FrmMain : Form
    {
        #region Define varialbles
        private enum MenuAction
        {
            NONE,
            MAIN,
            INTERFACE,
            SETTING,
            PARAMETER,
            EXIT
        }
        private MenuAction currentAction = MenuAction.NONE;
        #region Define Resource
        private bool bExit = false;
        private int iCPU = 0;
        private int iRAM = 0;
        private int iHardDiskC = 0;
        private int coreCount = 6;
        private System.Windows.Forms.Timer resourceTimer;
        private int maxMem = 0;
        public Thread thrSystemResource; //20150704 시스템 리소스 스레드
        private PerformanceCounter oCPU;// = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter oRAM;// = new PerformanceCounter("Memory", "Available MBytes");
        private PerformanceCounter oHardDiskC;// = new PerformanceCounter("LogicalDisk", "% Free Space", "C:");
        private static Mutex _mutex = null;
        private string Password;
        delegate void DelResource();
        private string AssyPN;
        private string WorkOrder;
        #endregion
        #region Define Log
        private static readonly ILog log = LogManager.GetLogger("InitializeLogger");
        // Thêm vào class:
        private readonly object _logLock = new object();
        private readonly Queue<string> _logBuffer = new Queue<string>();
        private System.Windows.Forms.Timer _logTimer;
        private const int MAX_LOG_LINES = 500;   // Giới hạn hiển thị
        private const int LOG_UPDATE_INTERVAL = 200; // ms: tần suất cập nhật UI
        #endregion
        #region Define Unit
        private CancellationToken cts;
        //private UC_Main _UcMain;
        //private UC_Interface _UcInterface;
        //private UC_Setting _UcSetting;
        //private UC_Parameter _UcParameter;
        #endregion
        #endregion
        public FrmMain(string _password)
        {
            const string appName = "DI_Water_Wash_NetCore";  // đổi tên theo app của bạn
            bool createdNew;
            _mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                using (FrmMessageError frmMessageError = new FrmMessageError("Application already Running !!!!!", Color.Red, Color.Black))
                {
                    frmMessageError.ShowDialog();
                }
                Environment.Exit(0);
                this.Close();
            }
            _mutex = new Mutex(true, appName, out createdNew);
            InitializeComponent();
            Password = _password;
            ThreadPool.QueueUserWorkItem(o =>
            {
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.StartPosition = FormStartPosition.CenterScreen;
                frmLoading.ShowDialog();
                frmLoading.Dispose();
            }
             );
            StateCommon.bLoading = true;
            StateCommon.LoadingText = "Loading...\r\n";
            log.Debug("Loading...");
            StateCommon.LoadingText += "Setting resource monitor...\r\n";
            try
            {
                oCPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                oRAM = new PerformanceCounter("Memory", "Available MBytes");
                oHardDiskC = new PerformanceCounter("LogicalDisk", "% Free Space", "C:");
                thrSystemResource = new Thread(new ThreadStart(getSystemResouce));
                thrSystemResource.Name = "thrResource";
                thrSystemResource.IsBackground = true;
                thrSystemResource.Start();
            }
            catch
            { }
            timer1.Start();
            StateCommon.LoadingText += "Setting thread update resuorce completed\r\n";
            log.Debug("Setting thread update resuorce completed");
            StateCommon.LoadingValue = 10;
            StateCommon.LoadingText += "Create DataBase Connection...\r\n";
            log.Debug("Create DataBase Connection completed");
            StateCommon.LoadingValue = 20;
            // 🟡 Gọi form Frm_ChangePartNumber và đợi kết quả
            StateCommon.LoadingText += "Setting Part Number ....\r\n";
            log.Debug("Setting Part Number ....");
            StateCommon.LoadingValue = 30;
            Frm_ChangePartNumber frmChange = new Frm_ChangePartNumber();
            frmChange.BringToFront();
            var result = frmChange.ShowDialog(); // chạy đồng bộ, chờ người dùng
            if (result == DialogResult.OK)
            {
                string[] array = File.ReadAllLines("C:\\Aavid_Test\\Setup-ini\\Flushing_Part_Numbers.txt")
                     .Where(line => !string.IsNullOrWhiteSpace(line))
                     .ToArray();
                AssyPN = array[1].Split(',')[1].Trim();
                WorkOrder = array[1].Split(',')[2].Trim();
                string stationID = ClsIO.ReadValue("LOGGING_DATA", "Tester_ID", "", "C:\\Aavid_Test\\Setup-ini\\LCS_Logging_Setup.ini").Trim().Trim('"').Trim('\'');
                if (stationID == "" || stationID == null || stationID == string.Empty)
                {
                    stationID = ClsIO.ReadValue("DEVICES", "Tester_ID", "", "C:\\Aavid_Test\\Setup-ini\\Leak_Test_Setup.ini").Trim().Trim('\'').Trim('"');
                    if (stationID == "" || stationID == null || stationID == string.Empty)
                    {
                        using (FrmMessageError frmMessageError = new FrmMessageError("Station ID is not valid. Please call Technical check again", Color.Red, Color.Black))
                        {
                            frmMessageError.ShowDialog();
                            StateCommon.bLoading = false;
                            Environment.Exit(0);
                            this.Close();
                        }
                    }
                }
                #region Initialize ASP Control
                StateCommon.LoadingText += "Config ASP Serial Port...\r\n";
                log.Debug("Config ASP Serial Port...");
                Cls_Interface.Initialize(AssyPN);
                cts = new CancellationToken();
                _ = Cls_Interface.PollScanAnalog(cts.Token, 1000);
                StateCommon.LoadingText += "Config ASP Serial Port completed\r\n";
                log.Debug("Config ASP Serial Port completed");
                StateCommon.LoadingValue = 50;
                StateCommon.LoadingText += "Getting USON Program from ASP Board completed\r\n";
                log.Debug("Getting USON Program from ASP Board completed");
                #endregion
                StateCommon.LoadingText += "Initial Unit...\r\n";
                log.Debug("Initial Unit");
            }
        }

        #region Function UI 
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dtNow = DateTime.Now;
            this.lb_Date.Text = dtNow.ToString("yyyy-MM-dd HH:mm:ss");

        }
        private void MenuActionChanged(MenuAction action)
        {
            if (action == MenuAction.EXIT)
            {
                using (FrmMessage frmMessage = new FrmMessage("Bạn có chắc muốn thoát?", Color.Red, Color.Black))
                {
                    frmMessage.ShowDialog();
                    if (frmMessage.DialogResult != DialogResult.OK) return;
                    else
                    {
                        //cts?.Cancel();
                        //ClsInterface.Disconnect();
                        Application.Exit();
                    }
                }
                return;
            }
            if (action == currentAction) return;
            currentAction = action;
            UpdateLabelMenuAction(currentAction);
            switch (action)
            {
                case MenuAction.MAIN:
                    pl_Main.Controls.Clear();
                    //_UcMain.Dock = DockStyle.Fill;
                    //pl_Main.Controls.Add(_UcMain);
                    break;
                case MenuAction.SETTING:
                    pl_Main.Controls.Clear();
                    //_UcSetting.Dock = DockStyle.Fill;
                    //pl_Main.Controls.Add(_UcSetting);
                    break;
                case MenuAction.PARAMETER:
                    pl_Main.Controls.Clear();
                    //_UcParameter.Dock = DockStyle.Fill;
                    //pl_Main.Controls.Add(_UcParameter);
                    break;
                case MenuAction.INTERFACE:
                    pl_Main.Controls.Clear();
                    //_UcInterface.Dock = DockStyle.Fill;
                    //pl_Main.Controls.Add(_UcInterface);
                    break;
            }
        }
        private void UpdateLabelMenuAction(MenuAction action)
        {
            lbl_Main.BackColor = action == MenuAction.MAIN ? Color.LimeGreen : SystemColors.ActiveCaption;
            lbl_Interface.BackColor = action == MenuAction.INTERFACE ? Color.LimeGreen : SystemColors.ActiveCaption;
            lbl_Setting.BackColor = action == MenuAction.SETTING ? Color.LimeGreen : SystemColors.ActiveCaption;
            lbl_Parameter.BackColor = action == MenuAction.PARAMETER ? Color.LimeGreen : SystemColors.ActiveCaption;
        }
        private void lbl_Main_Click(object sender, EventArgs e)
        {
            MenuActionChanged(MenuAction.MAIN);
        }

        private void lbl_Interface_Click(object sender, EventArgs e)
        {
            MenuActionChanged(MenuAction.INTERFACE);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            MenuActionChanged(MenuAction.EXIT);
        }

        private void lbl_Manual_Click(object sender, EventArgs e)
        {
            MenuActionChanged(MenuAction.SETTING);
        }

        private void lbl_Parameter_Click(object sender, EventArgs e)
        {
            MenuActionChanged(MenuAction.PARAMETER);
        }

        private void lbl_Exit_Click(object sender, EventArgs e)
        {
            MenuActionChanged(MenuAction.EXIT);
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            lb_Ver.Text = "Version: " + Application.ProductVersion;
            MenuActionChanged(MenuAction.MAIN);
        }
        #region Function Resource Monitor
        public void getSystemResouce()
        {
            getPhysicalMemory();
            while (!bExit)
            {
                oCPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                oRAM = new PerformanceCounter("Memory", "Available MBytes");
                oHardDiskC = new PerformanceCounter("LogicalDisk", "% Free Space", "C:");
                int[] _CPU = new int[coreCount];
                for (int i = 0; i < coreCount; i++)
                {
                    Thread.Sleep(500);
                    _CPU[i] = (int)oCPU.NextValue();
                }
                iCPU = GetMaxCore(_CPU);
                float availableMemory = oRAM.NextValue();
                float usedMemory = (float)maxMem - availableMemory;
                float _RAM = (usedMemory / (float)maxMem) * 100;
                iRAM = (int)_RAM;
                int _C = 100 - (int)oHardDiskC.NextValue();
                iHardDiskC = _C;
                DelResource a = new DelResource(funResource);
                if (progressBar_CPU.IsHandleCreated)
                    progressBar_CPU.BeginInvoke(a);
            }
        }
        private int GetMaxCore(int[] _iCPU)
        {
            int maxCore = 0;
            for (int i = 0; i < _iCPU.Length; i++)
            {
                maxCore = Math.Max(maxCore, _iCPU[i]);
            }
            return maxCore;
        }
        private void funResource()
        {
            int _cpu = 0;
            int _ram = 0;
            int _c = 0;
            int _d = 0;

            _cpu = iCPU;
            //Console.WriteLine("CPU: " + iCPU);

            _ram = iRAM;
            _c = iHardDiskC;

            if (_cpu > 100)
                _cpu = 100;
            if (_ram > 100)
                _ram = 100;
            if (_c > 100)
                _c = 100;
            if (_d > 100)
                _d = 100;

            progressBar_CPU.Value = _cpu;
            lb_CPU.Text = _cpu.ToString() + "%";
            progressBar_MEM.Value = _ram;
            lb_MEM.Text = _ram.ToString() + "%";
            progressBar_C.Value = _c;
            lb_DiskC.Text = _c.ToString() + "%";
        }
        private void getPhysicalMemory()
        {
            System.Management.ObjectQuery winQuery = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);
            ManagementObjectCollection queryCollection = searcher.Get();

            ulong memory = 0;
            foreach (ManagementObject item in queryCollection)
            {
                memory = ulong.Parse(item["TotalVisibleMemorySize"].ToString());
            }
            maxMem = (int)(memory / 1024);
        }
        #endregion
        #endregion

    }
}
