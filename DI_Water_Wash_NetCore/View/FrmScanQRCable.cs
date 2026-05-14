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
    private Cls_DBMsSQL CableCheckDB = new Cls_DBMsSQL();
    private int iLifeTimeLimit;
    private string StationID;
    private int QDNo;
    public FrmScanQRCable(int QDno, int iQDlifeTime, string stationid)
    {
        InitializeComponent();
        CableCheckDB.Initialize("10.102.4.20", "EquipmentManagerment", "readuser", "Boyd@2025");
        CableCheckDB.Open();
        lbl_Infor.Text = $"Please scan QR Code QD No {QDno} !!!";
        iLifeTimeLimit = iQDlifeTime;
        StationID = stationid;
        QDNo = QDno;
    }
    private void txt_QRInput_TextChanged(object sender, EventArgs e)
    {
        string QDInput = txt_QRInput.Text;
        if (QDInput.Length == 17)
        {
            bool bCheckCable = CheckCableInDB(txt_QRInput.Text.Trim());
            if (bCheckCable)
            {
                InsertCableInDB(txt_QRInput.Text.Trim());
                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
    private bool CheckQDFormat(string sn)
    {
        switch (StateCommon.Secsion.ToString())
        {
            case "Helium_Leak_Test":
                if (sn.StartsWith("QD_34_"))
                    return true;
                else
                    return false;
            case "DI_Water_Wash":
                if (sn.StartsWith("QD_30_"))
                    return true;
                else
                    return false;
            default:
                return true;
        }
    }
    private bool CheckCableInDB(string serialNumber)
    {
        if (!CheckQDFormat(serialNumber))
        {
            this.Invoke(new Action(() =>
            {
                var dialog = new FrmMessageError($"Fixture không phải thuộc station này!!!\r\nCall technical kiểm tra."
                    , Color.Red, Color.Black, 10000);
                dialog.StartPosition = FormStartPosition.CenterScreen; // ✅ hiện giữa màn hình
                dialog.ShowDialog(this);   // hoặc ShowDialog(_mainForm)
            }));
            return false;
        }
        string query = $@"SELECT [SerialNumber]
                                FROM [EquipmentManagerment].[dbo].[EquipmentRemaining]
                                where SerialNumber = '{serialNumber}'";
        DataTable dtResult = CableCheckDB.ExecuteQuery(query);
        if ((iLifeTimeLimit - dtResult.Rows.Count) < ClsDefine.DefineSystem.iLifeTimeWarning)
        {
            this.Invoke(new Action(() =>
            {
                var dialog = new FrmMessageError($"Fixture cần được thay thế!!!\r\n tuổi thọ còn lại {iLifeTimeLimit - dtResult.Rows.Count}"
                    , Color.Red, Color.Black, 10000);
                dialog.StartPosition = FormStartPosition.CenterScreen; // ✅ hiện giữa màn hình
                dialog.ShowDialog(this);   // hoặc ShowDialog(_mainForm)
            }));
            return false;
        }
        if (QDNo == 1)
        {
            LifeTimeLog QDlifeTime = LifeTimeLogManager.LoadLog(ClsDefine.DefineSystem.sQDlogpath);
            if (QDlifeTime != null)
            {
                QDlifeTime.TimeUsed = dtResult.Rows.Count + 1;
                QDlifeTime.LifeTimeWarning = ClsUnitManagercs.cls_Units[QDNo-1].iQDLifeTime - dtResult.Rows.Count;
                LifeTimeLogManager.AddOrReplaceLog(QDlifeTime, ClsDefine.DefineSystem.sQDlogpath);
            }
            if (QDlifeTime == null || dtResult.Rows.Count == 0)
            {
                QDlifeTime = new LifeTimeLog();
                QDlifeTime.SerialNumber = serialNumber;
                QDlifeTime.TimeUsed = dtResult.Rows.Count + 1;
                QDlifeTime.LifeTimeWarning = ClsUnitManagercs.cls_Units[QDNo - 1].iQDLifeTime - dtResult.Rows.Count;
                QDlifeTime.DateTimeAdded = DateTime.Now;
                LifeTimeLogManager.AddOrReplaceLog(QDlifeTime, ClsDefine.DefineSystem.sQDlogpath);
            }
        }
        else if (QDNo == 2)
        {
            LifeTimeLog QD2lifeTime = LifeTimeLogManager.LoadLog(ClsDefine.DefineSystem.sQD2logpath);
            if (QD2lifeTime != null)
            {
                QD2lifeTime.TimeUsed = dtResult.Rows.Count + 1;
                QD2lifeTime.LifeTimeWarning = ClsUnitManagercs.cls_Units[QDNo - 1].iQDLifeTime - dtResult.Rows.Count;
                LifeTimeLogManager.AddOrReplaceLog(QD2lifeTime, ClsDefine.DefineSystem.sQD2logpath);
            }
            if (QD2lifeTime == null || dtResult.Rows.Count == 0)
            {
                QD2lifeTime = new LifeTimeLog();
                QD2lifeTime.SerialNumber = serialNumber;
                QD2lifeTime.TimeUsed = dtResult.Rows.Count + 1;
                QD2lifeTime.LifeTimeWarning = ClsUnitManagercs.cls_Units[QDNo - 1].iQDLifeTime - dtResult.Rows.Count;
                QD2lifeTime.DateTimeAdded = DateTime.Now;
                LifeTimeLogManager.AddOrReplaceLog(QD2lifeTime, ClsDefine.DefineSystem.sQD2logpath);
            }
        }
        else if (QDNo == 3)
        {
            LifeTimeLog QD3lifeTime = LifeTimeLogManager.LoadLog(ClsDefine.DefineSystem.sQD3logpath);
            if (QD3lifeTime != null)
            {
                QD3lifeTime.TimeUsed = dtResult.Rows.Count + 1;
                QD3lifeTime.LifeTimeWarning = ClsUnitManagercs.cls_Units[QDNo - 1].iQDLifeTime - dtResult.Rows.Count;
                LifeTimeLogManager.AddOrReplaceLog(QD3lifeTime, ClsDefine.DefineSystem.sQD3logpath);
            }
            if (QD3lifeTime == null || dtResult.Rows.Count == 0)
            {
                QD3lifeTime  = new LifeTimeLog();
                QD3lifeTime.SerialNumber = serialNumber;
                QD3lifeTime.TimeUsed = dtResult.Rows.Count + 1;
                QD3lifeTime.LifeTimeWarning = ClsUnitManagercs.cls_Units[QDNo - 1].iQDLifeTime - dtResult.Rows.Count;
                QD3lifeTime.DateTimeAdded = DateTime.Now;
                LifeTimeLogManager.AddOrReplaceLog(QD3lifeTime, ClsDefine.DefineSystem.sQD3logpath);
            }
        }
        else if (QDNo == 4)
        {
            LifeTimeLog QD4lifeTime = LifeTimeLogManager.LoadLog(ClsDefine.DefineSystem.sQD4logpath);
            if (QD4lifeTime != null)
            {
                QD4lifeTime.TimeUsed = dtResult.Rows.Count + 1;
                QD4lifeTime.LifeTimeWarning = ClsUnitManagercs.cls_Units[QDNo - 1].iQDLifeTime - dtResult.Rows.Count;
                LifeTimeLogManager.AddOrReplaceLog(QD4lifeTime, ClsDefine.DefineSystem.sQD4logpath);
            }
            if (QD4lifeTime == null || dtResult.Rows.Count == 0)
            {
                QD4lifeTime = new LifeTimeLog();
                QD4lifeTime.SerialNumber = serialNumber;
                QD4lifeTime.TimeUsed = dtResult.Rows.Count + 1;
                QD4lifeTime.LifeTimeWarning = ClsUnitManagercs.cls_Units[QDNo - 1].iQDLifeTime - dtResult.Rows.Count;
                QD4lifeTime.DateTimeAdded = DateTime.Now;
                LifeTimeLogManager.AddOrReplaceLog(QD4lifeTime, ClsDefine.DefineSystem.sQD4logpath);
            }
        }
        if (dtResult.Rows.Count > iLifeTimeLimit)
        {
            this.Invoke(new Action(() =>
            {
                var dialog = new FrmMessageError($"Fixture cần được thay thế!!!\r\n tuổi thọ còn lại {iLifeTimeLimit - dtResult.Rows.Count}"
                    , Color.Red, Color.Black, 10000);
                dialog.StartPosition = FormStartPosition.CenterScreen; // ✅ hiện giữa màn hình
                dialog.ShowDialog(this);   // hoặc ShowDialog(_mainForm)
            }));
            return false;
        }
        else
            return true;
    }
    private bool InsertCableInDB(string serialNumber)
    {
        string insertQuery = $@"
        INSERT INTO [EquipmentManagerment].[dbo].[EquipmentRemaining] 
        ( [SerialNumber]
          ,[Date_Time]
          ,[Process_Monitor]
          ,[StationID]) 
        VALUES ('{serialNumber}', GETDATE(), '{StateCommon.Secsion.ToString()}','{StationID}')";

        // Thực thi, không cần quan tâm đến kết quả trả về
        CableCheckDB.ExecuteQuery(insertQuery);
        return true;
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

