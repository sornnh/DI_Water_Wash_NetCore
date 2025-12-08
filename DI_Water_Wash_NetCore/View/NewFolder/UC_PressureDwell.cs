using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public partial class UC_PressureDwell : UserControl
{
    private int UnitIndex;
    public UC_PressureDwell(int unitIndex)
    {
        UnitIndex = unitIndex;
        InitializeComponent();
        InitializeDwellParameters();
    }
    private void InitializeDwellParameters()
    {
        txt_DwellTime.Text = ClsUnitManagercs.cls_Units[UnitIndex].fDwellTime.ToString("0.000");
        txt_DwellTotalCheck.Text = ClsUnitManagercs.cls_Units[UnitIndex].iNumberofDwellChecks.ToString();
        txt_Max_Pressure_Drop.Text = ClsUnitManagercs.cls_Units[UnitIndex].fMaxPressureDwellDrop.ToString("0.000");
        txt_Max_Set_Pressure.Text = ClsUnitManagercs.cls_Units[UnitIndex].fMaxPressureDwellFill.ToString("0.000");
        txt_Min_Pressure_Drop.Text = ClsUnitManagercs.cls_Units[UnitIndex].fMinPressureDwellDrop.ToString("0.000");
        txt_Min_Set_Pressure.Text = ClsUnitManagercs.cls_Units[UnitIndex].fMinPressureDwellFill.ToString("0.000");
    }
}

