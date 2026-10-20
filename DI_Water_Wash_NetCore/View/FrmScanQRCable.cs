using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public partial class FrmScanQRCable : Form
{
    private string StationID;
    private int QDIndex;
    private int UnitIndex;
    public string QDSerialNumber { get; private set; }
    public FrmScanQRCable(int iUnitIndex, int iQDIndex, string stationid)
    {
        InitializeComponent();
        lbl_Infor.Text = $"Please scan QR Code QD No {iQDIndex + 1} !!!";
        StationID = stationid;
        UnitIndex = iUnitIndex;
        QDIndex = iQDIndex;
    }
    private async void txt_QRInput_TextChanged(object sender, EventArgs e)
    {
        string QDInput = txt_QRInput.Text;
        if (QDInput.Length == 17)
        {
            QDSerialNumber = QDInput.Trim();
            bool bCheckCable = await CheckCableInDB(txt_QRInput.Text.Trim());
            if (bCheckCable)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
    private async Task<bool> CheckCableInDB(string serialNumber)
    {
        var checkQD = ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.checkQD;
        int icheckQD = await checkQD.CheckLifeTimeInDB(serialNumber, StationID, QDIndex);
        if (icheckQD == 0)
            return true;
        else
        {
            this.Invoke(new Action(() =>
            {
                var dialog = new FrmMessageError($"QD {QDIndex +1 } ERROR \r\n {checkQD.GetDescriptionCode(icheckQD)}!!!"
                    , Color.Red, Color.Black, 10000);
                dialog.StartPosition = FormStartPosition.CenterScreen; // ✅ hiện giữa màn hình
                dialog.ShowDialog(this);   // hoặc ShowDialog(_mainForm)
            }));
            return false;
        }
    }
    private void timer1_Tick(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }

    private void timer2_Tick(object sender, EventArgs e)
    {
        txt_QRInput.Focus();
    }
}

