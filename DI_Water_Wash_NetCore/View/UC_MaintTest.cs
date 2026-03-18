using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DI_Water_Wash.Unit
{
    public partial class UC_MaintTest : UserControl
    {
        public int UnitIndex { get; private set; }
        private string[] Testmodes = new string[4] { "Production Test", "Production Retest", "Engineering Test", "Golden Sample Test" };
        private string password;
        public UC_MaintTest(int index, string pass)
        {
            InitializeComponent();

            password = pass;
            UnitIndex = index;
            lb_Fixture.Text = $"Fixture {index + 1}";
            lb_PN.Text = ClsUnitManagercs.cls_Units[UnitIndex].AssyPN;
            lb_Status.Text = "Ready for test";
            lb_TotalCycle.Text = ClsUnitManagercs.cls_Units[UnitIndex].iWash_Cycle.ToString();
            timer1.Start();
            //pictureBox1.Image = Image.FromFile(@"panel.jpg");
            if (ClsUnitManagercs.cls_Units[UnitIndex].bCheck_DI_Huminity)
            {
                label13.Visible = true;
                lb_Humidity.Visible = true;
                label21.Visible = true;
            }
            else
            {
                label13.Visible = false;
                lb_Humidity.Visible = false;
                label21.Visible = false;
            }
            ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.OnRequestAddProcessLogTest += UpdateProcessData;
            ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.OnRequestUpdateResult += UpdateModeTest;
            this.cbb_TestMode.Items.AddRange(Testmodes);
            this.cbb_TestMode.SelectedIndex = 0;

            lblQDLifeTimeValue.Text = ClsUnitManagercs.cls_Units[UnitIndex].iQDLifeTime.ToString();

            UpdateLabelColor(lblQDUsedTimesValue, ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.iQDTimeused.ToString(),
                 ClsUnitManagercs.cls_Units[UnitIndex].iQDLifeTime - ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.iQDTimeused > ClsDefine.DefineSystem.iLifeTimeWarning ? SystemColors.ControlLightLight : Color.Red);
        }
        private void UpdateModeTest()
        {
            if (cbb_TestMode.InvokeRequired)
            {
                cbb_TestMode.BeginInvoke(new Action(UpdateModeTest));
            }
            else
                cbb_TestMode.SelectedItem = ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.TestType;
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
            ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.TestType = cbb_TestMode.Text;
            // Cập nhật chỉ số đã chọn gần nhất
            previousTestModeIndex = cbb_TestMode.SelectedIndex;
        }
        public void SetSNForTest()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => SetSNForTest()));
                return;
            }
            txt_SN.Enabled = true;
            txt_SN.Text = "";
        }
        public void UpdateData(DataTable dt)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => UpdateData(dt)));
                return;
            }
            else
            {
                dgv_process.SuspendLayout(); // Tạm ngừng layout để tránh flicker
                dgv_process.ReadOnly = true; // Không cho sửa
                dgv_process.AllowUserToAddRows = false; // Không cho thêm dòng trống cuối
                dgv_process.AllowUserToDeleteRows = false; // Không cho xoá
                dgv_process.AutoGenerateColumns = true; // Tự tạo cột theo DataTable
                dgv_process.Columns.Clear(); // Xoá cột dư nếu có
                dgv_process.DataSource = dt;   // Gán nguồn dữ liệu mới

                // Ẩn cột row header (cột trống bên trái)
                dgv_process.RowHeadersVisible = false;

                // ❌ Không cho sort
                foreach (DataGridViewColumn col in dgv_process.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                dgv_process.ResumeLayout();
            }
        }
        //private void txt_SN_KeyDown(object sender, KeyEventArgs e)
        //{
        //    string SN = txt_SN.Text.Trim();
        //    if(SN.Length == ClsUnitManagercs.cls_Units[UnitIndex].SN_Length )
        //    {
        //        if (txt_SN.Text.Length == 0 )
        //        {
        //            MessageBox.Show("Please input the serial number.");
        //            return;
        //        }
        //        // Assuming there's a method to handle the test with the provided serial number
        //        ClsUnitManagercs.cls_Units[UnitIndex].cls_SequencyTest.SN = txt_SN.Text.Trim();
        //        txt_SN.Enabled = false;
        //    }
        //}
        private void UpdateLabelResult(string text)
        {
            switch (text)
            {
                case "PASS":
                    UpdatelabelwithColor("PASS", Color.Green, lb_Result);
                    break;
                case "RUNNING":
                    UpdatelabelwithColor("RUNNING", Color.Yellow, lb_Result);
                    break;
                case "N/A":
                    UpdatelabelwithColor("N/A", Color.Gray, lb_Result);
                    break;
                default:
                    UpdatelabelwithColor(text, Color.Red, lb_Result);
                    break;
            }
        }
        private void Updatelabel(string value, Label label)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => Updatelabel(value, label)));
                    return;
                }
                else
                {
                    label.Text = value;
                    try
                    {
                        if (label == lb_CurrentTime && int.TryParse(lb_CurrentTime.Text, out int currentTime) && int.TryParse(lb_TimeStep.Text, out int timeStep) && timeStep > 0)
                        {
                            int percen = currentTime * 100 / timeStep;
                            UpdatePercent(percen + 1);
                        }
                    }
                    catch { }

                }
            }
            catch { }
        }
        private void Hidenlabel(Label label)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => Hidenlabel(label)));
                    return;
                }
                else
                {
                    label.Visible = false;
                }
            }
            catch { }
        }
        private void UnHidenlabel(Label label)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => UnHidenlabel(label)));
                    return;
                }
                else
                {
                    label.Visible = true;
                }
            }
            catch { }
        }
        private void UpdatelabelwithColor(string value, Color color, Label label)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => UpdatelabelwithColor(value, color, label)));
                    return;
                }
                else
                {
                    label.Text = value;
                    label.BackColor = color;
                }
            }
            catch { }
        }
        private void UpdatePercent(int value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => UpdatePercent(value)));
                return;
            }
            else
                progressBar1.Value = value;
        }
        private void UpdatePercentTotal(int value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => UpdatePercent(value)));
                return;
            }
            else
                procTesting.Value = value;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.testSeq)
            {
                case Cls_SequenceTest.TestSeq.WAIT:
                    UnHidenlabel(lb_Airpressure);
                    UnHidenlabel(label11);
                    UnHidenlabel(label19);
                    UnHidenlabel(lb_Flowrate);
                    UnHidenlabel(lb_Waterpressure);
                    UnHidenlabel(label10);
                    UnHidenlabel(label12);
                    UnHidenlabel(label18);
                    UnHidenlabel(label20);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.WaterPressure).ToString("0.00"), lb_Waterpressure);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.FlowRate).ToString("0.00"), lb_Flowrate);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.AirPressure).ToString("0.00"), lb_Airpressure);
                    txt_SN.Enabled = false;
                    Updatelabel("Ready for test", lb_Status);
                    break;
                case Cls_SequenceTest.TestSeq.SN_INSERT:
                    txt_SN.Enabled = true;
                    txt_SN.Focus();
                    Updatelabel("Wait for insert SN", lb_Status);
                    break;
                case Cls_SequenceTest.TestSeq.SN_CHECK_ROUTER:
                    txt_SN.Enabled = false;
                    Updatelabel("Checking Router", lb_Status);
                    break;
                case Cls_SequenceTest.TestSeq.SN_CHECK_ROUTER_OK:
                    txt_SN.Enabled = false;
                    Updatelabel("Start test", lb_Status);
                    break;
                case Cls_SequenceTest.TestSeq.ERROR:
                    txt_SN.Enabled = false;
                    Updatelabel("ERROR", lb_Status);
                    Updatelabel("ERROR", lb_Result);
                    UpdateLabelColor(lblQDUsedTimesValue, ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.iQDTimeused.ToString(),
                                     ClsUnitManagercs.cls_Units[UnitIndex].iQDLifeTime - ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.iQDTimeused > ClsDefine.DefineSystem.iLifeTimeWarning ? SystemColors.ControlLightLight : Color.Red); break;
                    break;
                case Cls_SequenceTest.TestSeq.PRE_WASHING:
                    Hidenlabel(lb_Airpressure);
                    Hidenlabel(label11);
                    Hidenlabel(label19);
                    UnHidenlabel(lb_Flowrate);
                    UnHidenlabel(lb_Waterpressure);
                    UnHidenlabel(label10);
                    UnHidenlabel(label12);
                    UnHidenlabel(label18);
                    UnHidenlabel(label20);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.WaterPressure).ToString("0.00"), lb_Waterpressure);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.FlowRate).ToString("0.00"), lb_Waterpressure);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.StepTesting + 1).ToString(), lb_CurentCycle);
                    Updatelabel("Pre Washing", lb_CycleTest);
                    Updatelabel(ClsUnitManagercs.cls_Units[UnitIndex].iPre_Washing_Time.ToString(), lb_TimeStep);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.sw_prewash.ElapsedMilliseconds / 1000).ToString("0"), lb_CurrentTime);
                    UpdatePercentTotal(10);
                    break;
                case Cls_SequenceTest.TestSeq.FLUSHING_WASHING:
                    UpdatePercentTotal(40);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.WaterPressure).ToString("0.00"), lb_Waterpressure);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.FlowRate).ToString("0.00"), lb_Flowrate);
                    Updatelabel("Washing", lb_CycleTest);
                    Updatelabel(ClsUnitManagercs.cls_Units[UnitIndex].iWashing_Time.ToString(), lb_TimeStep);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.sw_flush.ElapsedMilliseconds / 1000).ToString("0"), lb_CurrentTime);
                    break;
                case Cls_SequenceTest.TestSeq.REVERSE_WASHING:
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.WaterPressure).ToString("0.00"), lb_Waterpressure);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.FlowRate).ToString("0.00"), lb_Flowrate);
                    Updatelabel("Reverse Washing", lb_CycleTest);
                    Updatelabel(ClsUnitManagercs.cls_Units[UnitIndex].iDI_Reverse_Washing_Time.ToString(), lb_TimeStep);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.sw_reverse.ElapsedMilliseconds / 1000).ToString("0"), lb_CurrentTime);
                    break;
                case Cls_SequenceTest.TestSeq.DRYING:
                    UpdatePercentTotal(70);
                    UnHidenlabel(lb_Airpressure);
                    UnHidenlabel(label11);
                    UnHidenlabel(label19);
                    Hidenlabel(lb_Flowrate);
                    Hidenlabel(lb_Waterpressure);
                    Hidenlabel(label10);
                    Hidenlabel(label12);
                    Hidenlabel(label18);
                    Hidenlabel(label20);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.AirPressure).ToString("0.00"), lb_Airpressure);
                    Updatelabel("Drying", lb_CycleTest);
                    Updatelabel(ClsUnitManagercs.cls_Units[UnitIndex].iDI_Drying_Time.ToString(), lb_TimeStep);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.sw_drying.ElapsedMilliseconds / 1000).ToString("0"), lb_CurrentTime);
                    break;
                case Cls_SequenceTest.TestSeq.REVERSE_DRYING:
                    Updatelabel("Reverse Drying", lb_CycleTest);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.AirPressure).ToString("0.00"), lb_Airpressure);
                    Updatelabel(ClsUnitManagercs.cls_Units[UnitIndex].iReverse_DI_Drying_Time.ToString(), lb_TimeStep);
                    Updatelabel((ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.sw_reverse_drying.ElapsedMilliseconds / 1000).ToString("0"), lb_CurrentTime);
                    UpdatePercentTotal(100);
                    break;
                case Cls_SequenceTest.TestSeq.END:
                    UpdateLabelColor(lblQDUsedTimesValue, ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.iQDTimeused.ToString(),
                                     ClsUnitManagercs.cls_Units[UnitIndex].iQDLifeTime - ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.iQDTimeused > ClsDefine.DefineSystem.iLifeTimeWarning ? SystemColors.ControlLightLight : Color.Red); break;
                default:
                    txt_SN.Enabled = false;
                    Updatelabel("Testing", lb_Status);
                    break;
            }
            switch (ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceCommon.process)
            {
                case Cls_SequenceCommon.SequenceState.Idle:
                    Updatelabel("Idle", lb_StatusMachine);
                    lb_StatusMachine.BackColor = Color.DarkOrange;
                    UpdateLabelResult(ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.TestResult);
                    break;
                case Cls_SequenceCommon.SequenceState.Running:
                    Updatelabel("Running", lb_StatusMachine);
                    lb_StatusMachine.BackColor = Color.Yellow;
                    UpdateLabelResult("RUNNING");
                    break;
                case Cls_SequenceCommon.SequenceState.CompletedPass:
                    Updatelabel("Pass", lb_StatusMachine);
                    lb_StatusMachine.BackColor = Color.Green;
                    UpdateLabelResult("PASS");
                    lb_Result.BackColor = Color.Green;
                    break;
                case Cls_SequenceCommon.SequenceState.CompletedFail:
                    Updatelabel("Fail", lb_StatusMachine);
                    lb_StatusMachine.BackColor = Color.Red;
                    UpdateLabelResult(ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.TestResult);
                    lb_Result.BackColor = Color.Red;
                    break;
                case Cls_SequenceCommon.SequenceState.Error:
                    Updatelabel("Error", lb_StatusMachine);
                    lb_StatusMachine.BackColor = Color.Red;
                    UpdateLabelResult("ERROR");
                    break;
            }
        }

        private void UpdateLabelColor(Label lb, string _text, Color backcolor = default)
        {
            if (lb.InvokeRequired)
            {
                lb.BeginInvoke(new Action(() => { UpdateLabelColor(lb, _text, backcolor); }));
            }
            else
            {
                lb.Text = _text;
                lb.BackColor = backcolor;
            }
        }




        private void txt_SN_TextChanged(object sender, EventArgs e)
        {
            string SN = txt_SN.Text.Trim();

            // Kiểm tra nếu độ dài của SN đã đủ
            if (SN.Length == ClsUnitManagercs.cls_Units[UnitIndex].SN_Length)
            {
                if (txt_SN.Text.Length == 0)
                {
                    MessageBox.Show("Please input the serial number.");
                    return;
                }
                // Nếu đủ độ dài, thực hiện hành động cần thiết
                ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.SN = txt_SN.Text.Trim();
                txt_SN.Enabled = false;  // Vô hiệu hóa TextBox sau khi nhập đủ SN
            }
        }

        private void btn_Abort_Click(object sender, EventArgs e)
        {
            ClsUnitManagercs.cls_Units[UnitIndex].clsSequenceTest.bTestAbort = true;
        }
        public void UpdateProcessData(int SNIndex, DataTable dt)
        {
            if (dgv_process.InvokeRequired)
            {
                dgv_process.BeginInvoke(new Action(() => UpdateProcessData(SNIndex, dt)));
                return;
            }
            else
            {
                dgv_process.SuspendLayout();
                dgv_process.ReadOnly = true;
                dgv_process.AllowUserToAddRows = false;
                dgv_process.AllowUserToDeleteRows = false;
                dgv_process.AutoGenerateColumns = true;
                dgv_process.Columns.Clear();
                dgv_process.DataSource = dt;
                dgv_process.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv_process.RowHeadersVisible = false;
                foreach (DataGridViewColumn col in dgv_process.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgv_process.ResumeLayout();
            }
        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
