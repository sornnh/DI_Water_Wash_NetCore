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

public partial class UC_Setting : UserControl
{
    
    public UC_Setting()
    {
        InitializeComponent();
        GenerateComPort();
        
    }
    private void GenerateComPort()
    {
        try
        {
            chk_AutoFixture.Checked = ClsDefine.DefineFixture.bAutoFixture;
            chk_AutoScanner.Checked = ClsDefine.DefineFixture.bAutoScanner;
            chk_DualTest.Checked = ClsDefine.DefineFixture.bDualFixture;
            chk_CheckN2Pressure.Checked = ClsDefine.DefineFixture.bN2PressureCheck;

            txt_N2FillTimeOut.Text = ClsDefine.DefineFixture.dN2FillTimeOut.ToString();
            txt_N2Min.Text = ClsDefine.DefineFixture.dN2CheckMin.ToString();
            txt_N2Max.Text = ClsDefine.DefineFixture.dN2CheckMax.ToString();
            txt_N2StabilityTime.Text = ClsDefine.DefineFixture.dN2StabilityTime.ToString();
            if (!File.Exists("C:\\Aavid_Test\\Setup-ini\\FixtureConfiguration.ini"))
            {
                ClsDefine.DefineFixture.SaveFixtureSetting();
            }
            if (!File.Exists("C:\\Aavid_Test\\Setup-ini\\AnalogSensorConfiguration.ini"))
            {
                ClsDefine.DefineAnalogSensor.SaveFixtureSetting();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error when generate Com Port: " + ex.Message);
        }
    }
    private void cbb_Program_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    private void btn_Save_Click(object sender, EventArgs e)
    {
        using (FrmMessage frmMessage = new FrmMessage("Do you want to save configuration for Fixture define?", Color.Red, Color.Black))
        {
            frmMessage.ShowDialog();
            if (frmMessage.DialogResult == DialogResult.OK)
            {
                ClsDefine.DefineFixture.bAutoScanner = chk_AutoScanner.Checked;
                ClsDefine.DefineFixture.bAutoFixture = chk_AutoFixture.Checked;
                ClsDefine.DefineFixture.bDualFixture = chk_DualTest.Checked;
                ClsDefine.DefineFixture.bN2PressureCheck = chk_CheckN2Pressure.Checked;

                ClsDefine.DefineFixture.dN2FillTimeOut = int.Parse(txt_N2FillTimeOut.Text);
                ClsDefine.DefineFixture.dN2CheckMin = double.Parse(txt_N2Min.Text);
                ClsDefine.DefineFixture.dN2CheckMax = double.Parse(txt_N2Max.Text);
                ClsDefine.DefineFixture.dN2StabilityTime = int.Parse(txt_N2StabilityTime.Text);
                ClsDefine.DefineFixture.SaveFixtureSetting();
            }
            else
            {
                chk_AutoFixture.Checked = ClsDefine.DefineFixture.bAutoFixture;
                chk_AutoScanner.Checked = ClsDefine.DefineFixture.bAutoScanner;
                chk_DualTest.Checked = ClsDefine.DefineFixture.bDualFixture;
                chk_CheckN2Pressure.Checked = ClsDefine.DefineFixture.bN2PressureCheck;

                txt_N2FillTimeOut.Text = ClsDefine.DefineFixture.dN2FillTimeOut.ToString();
                txt_N2Min.Text = ClsDefine.DefineFixture.dN2CheckMin.ToString();
                txt_N2Max.Text = ClsDefine.DefineFixture.dN2CheckMax.ToString();
                txt_N2StabilityTime.Text = ClsDefine.DefineFixture.dN2StabilityTime.ToString();
            }
        }
    }
}

