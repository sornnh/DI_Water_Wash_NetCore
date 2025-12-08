using DI_Water_Wash.UISource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public partial class UC_Interface : UserControl
{
    private System.Windows.Forms.Label[] relayLabels = new System.Windows.Forms.Label[30];
    private int[] Baudrates = { 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200 };
    private int[] DataBits = { 5, 6, 7, 8 };
    private Parity[] Paritys = { Parity.None, Parity.Odd, Parity.Even };
    private StopBits[] stopBits = { StopBits.One, StopBits.None, StopBits.OnePointFive, StopBits.Two };
    public UC_Interface()
    {
        InitializeComponent();
        InitRelayLabels();
        GenerateComPort();
        Cls_Interface.cls_ASPcontrol.OnRequestAddLog += (sender) =>
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(() => funAddLog_Auto(sender)));
            else
                funAddLog_Auto(sender);
        };
        Cls_Interface.cls_ASPcontrol.OnRequestUpdateRelayStatus += (sender) =>
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(() => funUpdateRelayStatus_Auto(sender)));
            else
                funUpdateRelayStatus_Auto(sender);
        };
        
        if (ClsUnitManagercs.cls_Units[0].clsSequenceTest.AutoMode)
        {
            DisableAllControl();
            btn_ChangeMode.Text = "AUTO";
        }
    }
    public void funUpdateRelayStatus_Auto(Cls_ASPcontrol unit)
    {
        UpdateRelayStatus(unit.relayStates); // Sau đó mới cập nhật UI
        if (ClsUnitManagercs.cls_Units == null) return;
        if (!ClsUnitManagercs.cls_Units[0].clsSequenceTest.AutoMode) return;
    }
    private void ChangeToggle(ToggleButton toggleButton, bool isOn)
    {
        if (toggleButton == null)
        {
            return; // Nếu toggleButton là null, không làm gì cả
        }
        if (toggleButton.Created == false)
        {
            return; // Nếu toggleButton chưa được tạo, không làm gì cả
        }
        if (toggleButton.InvokeRequired)
        {
            toggleButton.BeginInvoke(new Action(() => ChangeToggle(toggleButton, isOn)));
        }
        else
        {
            toggleButton.Checked = isOn;
        }
    }
    private void UpdateRelayStatus(bool[] relays)
    {
        bool hasNull = relayLabels != null && relayLabels.Any(label => label == null);
        if (hasNull) return;
        for (int i = 0; i < 30; i++)
        {
            relayLabels[i].Text = $"R{i + 1}: {(relays[i] ? "ON" : "OFF")}";
            relayLabels[i].BackColor = relays[i] ? Color.LimeGreen : Color.LightGray;
            relayLabels[i].ForeColor = relays[i] ? Color.White : Color.Black;
        }
        void UpdateColor(Label label, Color color)
        {
            if (label.InvokeRequired)
                label.Invoke(new Action(() => label.BackColor = color));
            else
                label.BackColor = color;
        }
    }
    private void InitRelayLabels()
    {
        tbl_RelayControl.Controls.Clear();
        tbl_RelayControl.ColumnCount = 6;
        tbl_RelayControl.RowCount = 5;
        tbl_RelayControl.ColumnStyles.Clear();
        tbl_RelayControl.RowStyles.Clear();
        for (int i = 0; i < 6; i++)
            tbl_RelayControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6));
        for (int i = 0; i < 5; i++)
            tbl_RelayControl.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 5));
        for (int i = 0; i < 30; i++)
        {
            int index = i; // lưu lại để dùng trong event handler
            Label lbl = new Label();
            lbl.Text = $"R{index + 1}: OFF";
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Dock = DockStyle.Fill;
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.BackColor = Color.LightGray;
            lbl.Cursor = Cursors.Hand;
            lbl.Click += (s, e) => ToggleRelay(index);
            relayLabels[i] = lbl;
            int row = i / 6;
            int col = i % 6;
            tbl_RelayControl.Controls.Add(lbl, col, row);
        }
    }
    private async void ToggleRelay(int index)
    {
        bool currentStatus = Cls_Interface.cls_ASPcontrol.relayStates[index];
        bool newStatus = !currentStatus;
        try
        {
            await Cls_Interface.cls_ASPcontrol.SetRelayONOFFAsyncCheckResult(index + 1, newStatus ? 1 : 0);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Exception when send command to ASPPort: {ex.Message}");
        }
    }
    private void btn_Save_Click(object sender, EventArgs e)
    {
        using (FrmMessage frmMessage = new FrmMessage("Do you want to save configuration for ASP control Serial Port?", Color.Red, Color.Black))
        {
            frmMessage.ShowDialog();
            if (frmMessage.DialogResult == DialogResult.OK)
            {
                ClsDefine.DefineASP.PortName = cbbASPComPort.Text;
                ClsDefine.DefineASP.Baudrate = int.Parse(cbbASPBaudrate.Text);
                ClsDefine.DefineASP.DataBit = int.Parse(cbbASPDataBit.Text);
                ClsDefine.DefineASP.Parity = cbbASPParity.Text;
                ClsDefine.DefineASP.StopBit = cbbASPStopBit.Text;
                ClsDefine.DefineASP.SaveASPSerialPortInformation();
                Cls_Interface.cls_ASPcontrol.GetASPSerialPortInformation();
                Cls_Interface.cls_ASPcontrol.DisconnectASPSerial();
                Cls_Interface.cls_ASPcontrol.ConnectASPSerial();
                if (Cls_Interface.cls_ASPcontrol.isConnected)
                {
                    this.lb_ASPConnected.Text = "Connected";
                    this.lb_ASPConnected.BackColor = Color.Green;
                }
                else
                {
                    this.lb_ASPConnected.Text = "Disconnected";
                    this.lb_ASPConnected.BackColor = Color.Red;
                }
            }
        }
    }
    public void funAddLog_Auto(string _text)
    {
        if (!rtb_ASPPortBuffer.Created) return;
        if (rtb_ASPPortBuffer.InvokeRequired)
        {
            rtb_ASPPortBuffer.BeginInvoke(new MethodInvoker(delegate
            {
                rtb_ASPPortBuffer.AppendText(_text + "\n");
                rtb_ASPPortBuffer.ScrollToCaret();
            }));
        }
        else
        {
            rtb_ASPPortBuffer.AppendText(_text + "\n");
            rtb_ASPPortBuffer.ScrollToCaret();
        }
    }

    private async void btn_GetAllRelay_Click(object sender, EventArgs e)
    {
        await Cls_Interface.cls_ASPcontrol.GetAllRelay();
    }
    private void DisableAllControl()
    {
        tbl_RelayControl.Enabled = false;
        Tbn_UpDown.Enabled = false;
        Tbn_OpenClose.Enabled = false;
    }
    private void EnableAllControl()
    {
        tbl_RelayControl.Enabled = true;
        Tbn_UpDown.Enabled = true;
        Tbn_OpenClose.Enabled = true;
    }

    private void lb_BuzzerU1_Click(object sender, EventArgs e)
    {

    }

    private void btn_ChangeMode_Click(object sender, EventArgs e)
    {
        if (!ClsUnitManagercs.cls_Units[0].clsSequenceTest.AutoMode)
        {
            ClsUnitManagercs.cls_Units[0].clsSequenceTest.AutoMode = true;
            DisableAllControl();
            btn_ChangeMode.Text = "AUTO";
        }
        else if (ClsUnitManagercs.cls_Units[0].clsSequenceTest.AutoMode
            && ClsUnitManagercs.cls_Units[0].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.WAIT)
        {
            ClsUnitManagercs.cls_Units[0].clsSequenceTest.AutoMode = false;
            EnableAllControl();
            btn_ChangeMode.Text = "MANUAL";
        }
    }
    private void GenerateComPort()
    {
        try
        {
            this.cbbASPComPort.Items.AddRange(SerialPort.GetPortNames());
            this.cbbASPBaudrate.Items.AddRange(Baudrates.Cast<object>().ToArray());
            this.cbbASPDataBit.Items.AddRange(DataBits.Cast<object>().ToArray());
            this.cbbASPParity.Items.AddRange(Paritys.Cast<object>().ToArray());
            this.cbbASPStopBit.Items.AddRange(stopBits.Cast<object>().ToArray());
            string SerialCom = ClsDefine.DefineASP.PortName;
            int Baudrate = ClsDefine.DefineASP.Baudrate;
            int DataBit = ClsDefine.DefineASP.DataBit;
            Parity Parity = (Parity)Enum.Parse(typeof(Parity), ClsDefine.DefineASP.Parity);
            StopBits StopBit = (StopBits)Enum.Parse(typeof(StopBits), ClsDefine.DefineASP.StopBit);
            cbbASPComPort.SelectedItem = SerialCom;
            cbbASPBaudrate.SelectedItem = Baudrate;
            cbbASPDataBit.SelectedItem = DataBit;
            cbbASPParity.SelectedItem = Parity;
            cbbASPStopBit.SelectedItem = StopBit;
            if (!File.Exists("C:\\Aavid_Test\\Setup-ini\\SerialPort.ini"))
            {
                ClsDefine.DefineASP.SaveASPSerialPortInformation();
            }
            if (Cls_Interface.cls_ASPcontrol.isConnected)
            {
                this.lb_ASPConnected.Text = "Connected";
                this.lb_ASPConnected.BackColor = Color.Green;
            }
            else
            {
                this.lb_ASPConnected.Text = "Disconnected";
                this.lb_ASPConnected.BackColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error when generate Com Port: " + ex.Message);
        }
    }
    private async void Scan_Serial_NumberRequest(int Index)
    {
        try
        {
            if (Cls_Interface.cls_ASPcontrol == null) return;
            bool result = await Cls_Interface.cls_ASPcontrol.RequestTriggerScanner(Index);
            if (!result)
                Cls_Interface.cls_ASPcontrol.SerialNumber[Index - 1] = "";
            Label lbl = this.Controls.Find($"lbl_SN{Index}", true).FirstOrDefault() as Label;
            if (lbl != null)
            {
                lbl.Text = Cls_Interface.cls_ASPcontrol.SerialNumber[Index - 1];
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error when Scan SN: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
