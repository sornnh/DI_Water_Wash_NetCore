using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StateCommon;

public partial class UC_Parameter : UserControl
{
    public UC_Parameter()
    {
        InitializeComponent();
        TrysetSNVerifyShow();
        GenerateProcess();
    }
    private void GenerateProcess()
    {
        DataGridView[] dgv = new DataGridView[1];
        string tabName = $"Fixture";
        TabPage tabPage = new TabPage(tabName);
        // Tạo TableLayoutPanel
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        tableLayout.Dock = DockStyle.Fill;
        tableLayout.ColumnCount = 2;
        tableLayout.RowCount = 1; // Có thể tăng sau nếu bạn muốn nhiều hàng
        tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
        tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        tableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single; // Tùy chọn, để có viền giữa các ô
        dgv[0] = new DataGridView();
        dgv[0].Columns.Clear();
        dgv[0].Rows.Clear();
        dgv[0].ReadOnly = true;                      // Không cho phép sửa
        dgv[0].ClearSelection();                     // Bỏ chọn dòng mặc định
        dgv[0].DefaultCellStyle.SelectionBackColor = dgv[0].DefaultCellStyle.BackColor;
        dgv[0].DefaultCellStyle.SelectionForeColor = dgv[0].DefaultCellStyle.ForeColor;
        dgv[0].RowHeadersVisible = false;
        dgv[0].MultiSelect = false;
        dgv[0].Enabled = true;
        dgv[0].Dock = DockStyle.Fill; // Để nó chiếm toàn bộ ô
                                      // Vẫn scroll được
                                      // Tạo một cột duy nhất để hiển thị giá trị
        DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
        col.HeaderText = "Process";                  // Không hiện tiêu đề
        col.Name = "Process";
        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        col.SortMode = DataGridViewColumnSortMode.NotSortable;
        dgv[0].Columns.Add(col);
        for (int index = 0; index < ClsUnitManagercs.cls_Units[0].verifyFlags.Length; index++)
        {
            dgv[0].Rows.Add(ClsUnitManagercs.cls_Units[0].verifyFlags[index]);

        }
        tableLayout.Controls.Add(dgv[0], 1, 0);
        System.Windows.Forms.Panel pl_infor = new System.Windows.Forms.Panel();
        pl_infor.Dock = DockStyle.Fill;
        tableLayout.Controls.Add(pl_infor, 0, 0);
        tabPage.Controls.Add(tableLayout);
        tabControl3.TabPages.Add(tabPage);
        UC_PartNumberInfor uC_PartNumberInfors = new UC_PartNumberInfor(0, UC_PartNumberInfor.SecsionTest.DIWaterWash);
        uC_PartNumberInfors.InitUI();
        uC_PartNumberInfors.Dock = DockStyle.Fill;
        pl_infor.Controls.Add(uC_PartNumberInfors);

    }
    private void TrysetSNVerifyShow()
    {
        try
        {
            txt_SN_Lenght.Text = ClsUnitManagercs.cls_Units[0].SN_Length.ToString();
            txt_ControlString.Text = ClsUnitManagercs.cls_Units[0].PN_Ctl_String;
            txt_Leght.Text = ClsUnitManagercs.cls_Units[0].Ctl_Str_Length.ToString();
            txt_Offset.Text = ClsUnitManagercs.cls_Units[0].Ctl_Str_Offset.ToString();
        }
        catch
        {
            MessageBox.Show("Error when set SN verify string");
            txt_SN_Lenght.Text = "";
            txt_ControlString.Text = "";
            txt_Leght.Text = "";
            txt_Offset.Text = "";
        }
    }
}

