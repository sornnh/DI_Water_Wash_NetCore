using DI_Water_Wash.Unit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DI_Water_Wash_NetCore.View
{
    public partial class UC_Main : UserControl
    {
        private UC_MaintTest[] uC_MaintTests;
        private Panel[] pl_main;
        private string password;
        private string[] Testmodes = new string[4] { "Production Test", "Production Retest", "Engineering Test", "Golden Sample Test" };
        public UC_Main(string pass)
        {
            InitializeComponent();
            password = pass;
            pl_main = new Panel[ClsUnitManagercs.cls_Units.Length];
            TableLayoutPanel tbl_MainPanel = new TableLayoutPanel();
            tbl_MainPanel.Dock = DockStyle.Fill;
            tbl_MainPanel.ColumnCount = ClsUnitManagercs.cls_Units.Length;
            tbl_MainPanel.RowCount = 1;
            tbl_MainPanel.ColumnStyles.Clear();
            for (int i = 0; i < tbl_MainPanel.ColumnCount; i++)
            {
                tbl_MainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / tbl_MainPanel.ColumnCount));
            }
            tableLayoutPanel1.Controls.Add(tbl_MainPanel, 0, 0);
            uC_MaintTests = new UC_MaintTest[ClsUnitManagercs.cls_Units.Length];
            for (int i = 0; i < ClsUnitManagercs.cls_Units.Length; i++)
            {
                pl_main[i] = new Panel();
                pl_main[i].Dock = DockStyle.Fill;
                tbl_MainPanel.Controls.Add(pl_main[i], i, 0);
            }
            for (int i = 0; i < ClsUnitManagercs.cls_Units.Length; i++)
            {
                uC_MaintTests[i] = new UC_MaintTest(i,  pass);
                uC_MaintTests[i].Dock = DockStyle.Fill;
                pl_main[i].Controls.Add(uC_MaintTests[i]);
            }
            this.cbb_TestMode.Items.AddRange(Testmodes);
            this.cbb_TestMode.SelectedIndex = 0;
            lblStationID.Text = ClsUnitManagercs.cls_Units[0].StationID;
            if(ClsDefine.DefineFixture.bAutoScanner)
                txt_Fixture.Visible = false;
        }
        public void TriggerUpdateToolLife()
        {
            for (int i = 0; i < uC_MaintTests.Length; i++)
            {
                uC_MaintTests[i].UpdateToolLife(null, EventArgs.Empty);
            }
        }
        private void txt_Fixture_TextChanged(object sender, EventArgs e)
        {
            string fixtureText = txt_Fixture.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(fixtureText) || !fixtureText.Contains("STATION"))
            {
                return; // Không làm gì nếu TextBox rỗng
            }
            string lastchar = fixtureText[fixtureText.Length - 1].ToString();
            switch (lastchar)
            {
                case "1":
                    if ((ClsUnitManagercs.cls_Units[0].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.WAIT ||
                    ClsUnitManagercs.cls_Units[0].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.SN_INSERT ||
                    ClsUnitManagercs.cls_Units[0].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.ERROR))
                    {
                        uC_MaintTests[0].SetSNForTest();
                        ClsUnitManagercs.cls_Units[0].clsSequenceTest.testSeq = Cls_SequenceTest.TestSeq.FIXTURE_SCAN;
                    }
                    txt_Fixture.Text = ""; // Xóa nội dung TextBox sau khi xử lý
                    break;
                case "2":
                    if ((ClsUnitManagercs.cls_Units[1].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.WAIT ||
                    ClsUnitManagercs.cls_Units[1].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.SN_INSERT ||
                    ClsUnitManagercs.cls_Units[1].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.ERROR))
                    {
                        uC_MaintTests[1].SetSNForTest();
                        ClsUnitManagercs.cls_Units[1].clsSequenceTest.testSeq = Cls_SequenceTest.TestSeq.FIXTURE_SCAN;
                    }
                    txt_Fixture.Text = ""; // Xóa nội dung TextBox sau khi xử lý
                    break;
                case "3":
                    if ((ClsUnitManagercs.cls_Units[2].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.WAIT ||
                    ClsUnitManagercs.cls_Units[2].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.SN_INSERT ||
                    ClsUnitManagercs.cls_Units[2].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.ERROR))
                    {
                        uC_MaintTests[2].SetSNForTest();
                        ClsUnitManagercs.cls_Units[2].clsSequenceTest.testSeq = Cls_SequenceTest.TestSeq.FIXTURE_SCAN;
                    }
                    txt_Fixture.Text = ""; // Xóa nội dung TextBox sau khi xử lý
                    break;
            }
        }
        private int previousTestModeIndex = -1;
        private void cbb_TestMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbb_TestMode.SelectedIndex == 2)
            {
                using (var passwordForm = new Frm_PasswordInput())
                {
                    var result = passwordForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        string Inputpassword = passwordForm.EnteredPassword;
                        if (Inputpassword != password)
                        {
                            MessageBox.Show("Sai mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            // Quay lại giá trị cũ
                            cbb_TestMode.SelectedIndex = previousTestModeIndex;
                            return;
                        }
                    }
                    else
                    {
                        cbb_TestMode.SelectedIndex = previousTestModeIndex;
                        return;
                    }
                }
            }
            for (int i = 0; i < ClsUnitManagercs.cls_Units.Length; i++)
            {
                ClsUnitManagercs.cls_Units[i].clsSequenceTest.TestType = cbb_TestMode.Text;
            }
            // Cập nhật chỉ số đã chọn gần nhất
            previousTestModeIndex = cbb_TestMode.SelectedIndex;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<bool> SNInsert = new List<bool>();
            for (int i = 0; i < ClsUnitManagercs.cls_Units.Length; i++)
            {
                SNInsert.Add(ClsUnitManagercs.cls_Units[i].clsSequenceTest.testSeq == Cls_SequenceTest.TestSeq.SN_INSERT);
            }
            if (SNInsert.Contains(true)) return;
                txt_Fixture.Focus();
        }
    }
}
