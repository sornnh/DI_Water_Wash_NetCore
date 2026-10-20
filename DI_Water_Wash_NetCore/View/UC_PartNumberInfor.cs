using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;


public partial class UC_PartNumberInfor : UserControl
{
    private UC_DIWaterWash uc_DIWaterWash;
    private UC_Drying uc_Drying;
    private UC_HeLeakage uc_HeLeakage;
    private UC_PressureDecay uC_PressureDecay;
    private UC_PressureDwell uC_PressureDwell ;
    private int _UnitIndex;
    private SecsionTest secsion;
    private Panel[] pl_UC = new Panel[4];
    public enum SecsionTest
    {
        PressureDecay,
        DIWaterWash,
        HeliumLeakage,
        Drying,
        PressureDwell
    }
    public UC_PartNumberInfor(int _Index, SecsionTest secsionTest)
    {
        _UnitIndex = _Index;
        InitializeComponent();
        secsion = secsionTest;
        pl_UC[0] = pl_UC1;
        pl_UC[1] = panel1;
        pl_UC[2] = panel2;
        pl_UC[3] = panel3;
    }
    public void InitUI()
    {
        GetDescreption();
        uc_DIWaterWash = new UC_DIWaterWash(_UnitIndex);
        uc_Drying = new UC_Drying(_UnitIndex);
        uc_HeLeakage = new UC_HeLeakage(_UnitIndex);
        uC_PressureDecay = new UC_PressureDecay(_UnitIndex);
        uC_PressureDwell = new UC_PressureDwell(_UnitIndex);
        uc_DIWaterWash.Dock = DockStyle.Fill;
        uc_Drying.Dock = DockStyle.Fill;
        uc_HeLeakage.Dock = DockStyle.Fill;
        pl_UC1.Controls.Clear();
        switch(secsion)
        {
            case SecsionTest.DIWaterWash:
                SafeAddToPanel(pl_UC[0], uc_DIWaterWash);
                break;
            case SecsionTest.Drying:
                SafeAddToPanel(pl_UC[0], uc_Drying);
                break;
            case SecsionTest.HeliumLeakage:
                SafeAddToPanel(pl_UC[0], uc_HeLeakage);
                break;
            case SecsionTest.PressureDecay:
                SafeAddToPanel(pl_UC[0], uC_PressureDecay);
                break;
            case SecsionTest.PressureDwell:
                SafeAddToPanel(pl_UC[0], uC_PressureDwell);
                break;
        }    
        //switch (secsion)
        //{
        //    case SecsionTest.DIWaterWash:
        //        pl_UC4.Controls.Add(uc_DIWaterWash);
        //        break;
        //    case SecsionTest.Drying:
        //        pl_UC4.Controls.Add(uc_Drying);
        //        break;
        //    case SecsionTest.HeLeakage:
        //        pl_UC4.Controls.Add(uc_HeLeakage);
        //        break;
        //}
    }

    private void GetDescreption()   
    {
        txtPN.Text = ClsUnitManagercs.AssyPN;
        txtDescription.Text = ClsUnitManagercs.cls_Units[0].Description;
    }

    void SafeAddToPanel(Panel targetPanel, Control control)
    {
        if (control.Parent != null)
            ((Panel)control.Parent).Controls.Remove(control);
        targetPanel.Enabled = true;
        targetPanel.Visible = true;
        control.Visible = true;
        targetPanel.Controls.Clear();  // Xóa trước nếu muốn chỉ có 1 control
        control.Dock = DockStyle.Fill;
        targetPanel.Controls.Add(control);
    }
    private void label2_Click(object sender, EventArgs e)
    {
        InitUI();
    }
}

