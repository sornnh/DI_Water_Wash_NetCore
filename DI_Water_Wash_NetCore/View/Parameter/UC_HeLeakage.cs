using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


    public partial class UC_HeLeakage : UserControl
    {
        private int UnitIndex;
        public UC_HeLeakage(int unitIndex)
        {
            InitializeComponent();
            UnitIndex = unitIndex;
            InitializeDryingParameters();
        }

        private void InitializeDryingParameters()
        {
            txt_Pre_Vacuum.Text = ClsUnitManagercs.cls_Units[UnitIndex].iPre_Vacuum.ToString();
            txt_Roughing_Time_On.Text = ClsUnitManagercs.cls_Units[UnitIndex].fRoughing_Time.ToString();
            txt_Gross_Leak_Pressure.Text = ClsUnitManagercs.cls_Units[UnitIndex].fGross_Leak_Pressure.ToString();
            txt_Helium_Valve_Open_Time.Text = ClsUnitManagercs.cls_Units[UnitIndex].iHelium_Valve_Open_Time.ToString();
            txt_Normal_Pressure.Text = ClsUnitManagercs.cls_Units[UnitIndex].fNormal_pressure.ToString();
            txt_OpeningDelay.Text = ClsUnitManagercs.cls_Units[UnitIndex].iOpeningDelay.ToString();
            txt_VentTime.Text = ClsUnitManagercs.cls_Units[UnitIndex].iVentTimeMin.ToString();
            txt_Leak_Test_Time.Text = ClsUnitManagercs.cls_Units[UnitIndex].iLeak_Test_Time.ToString();
            txt_Min.Text = ClsUnitManagercs.cls_Units[UnitIndex].fMin_Threshold.ToString("E");
            txt_Max.Text = ClsUnitManagercs.cls_Units[UnitIndex].fReject_Threshold.ToString("E");
            if (ClsUnitManagercs.cls_Units[UnitIndex].bPre_Vacuum)
                cBox_Pre_Vacuum.Checked = true;
            else
                cBox_Pre_Vacuum.Checked = false;
            if (ClsUnitManagercs.cls_Units[UnitIndex].iRoughing_Time_On == 1)
                cBox_Roughing_Time_On.Checked = true;
            else
                cBox_Roughing_Time_On.Checked = false;
            if (ClsUnitManagercs.cls_Units[UnitIndex].iVent_Valve_Control == 1)
            {
                cBox_Automatic.Checked = true;
                cBox_Manual.Checked = false;
            }    
            else
            {
                cBox_Automatic.Checked = false;
                cBox_Manual.Checked = true;
            }    
        }

        private void txt_Min_TextChanged(object sender, EventArgs e)
        {

        }
    }
