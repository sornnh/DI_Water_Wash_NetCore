using LifeTimeToolCheck;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LifeTimeToolCheck
{
    public partial class FrmResetToolLife : Form
    {
        private readonly int _itemsCount;

        public FrmResetToolLife(int fixtureCount)
        {
            InitializeComponent();
            _itemsCount = Math.Min(8, fixtureCount);
            lblSubtitle.Text = $"Fixture Mode: {fixtureCount} | Active Counters: 1 to {_itemsCount}";
            if(ClsDefine.DefineFixture.bAutoScanner)
            {
                grpQD.Visible = true;
            }
            else
            {
                grpQD.Visible = false;
            }
            GenerateResetButtons();
        }

        private void GenerateResetButtons()
        {
            flpOring.Controls.Clear();
            flpQD.Controls.Clear();

            for (int i = 0; i < _itemsCount; i++)
            {
                int index = i;

                // O-Ring Reset Button
                Button btnOring = new Button
                {
                    Text = $"Reset Oring {index + 1}",
                    Size = new Size(135, 32),
                    Margin = new Padding(4),
                    Font = new Font("Century Gothic", 9F, FontStyle.Regular),
                    UseVisualStyleBackColor = true
                };
                btnOring.Click += (s, e) => ResetSingleItem(ToolLifeCheck.ToolName.Oring, index);
                flpOring.Controls.Add(btnOring);

                // QD Reset Button
                Button btnQD = new Button
                {
                    Text = $"Reset QD {index + 1}",
                    Size = new Size(135, 32),
                    Margin = new Padding(4),
                    Font = new Font("Century Gothic", 9F, FontStyle.Regular),
                    UseVisualStyleBackColor = true
                };
                btnQD.Click += (s, e) => ResetSingleItem(ToolLifeCheck.ToolName.QD, index);
                flpQD.Controls.Add(btnQD);
            }
        }

        private async Task<bool> ValidatePassword()
        {
            using (Frm_PasswordInput frm_Password = new Frm_PasswordInput())
            {
                frm_Password.ShowDialog();
                if (frm_Password.DialogResult == DialogResult.OK)
                {
                    string password = "VietNam@RS2026";
                    var passRes = await API.commonFunctionService.GetPasswordAsync("ResetOringCounter");
                    password = (passRes != null && passRes.Success) ? passRes.Password : "VietNam@RS2026";

                    if (frm_Password.EnteredPassword == password)
                    {
                        return true;
                    }
                    else
                    {
                        using (FrmMessageError frmMessageError = new FrmMessageError("Password incorrect!!!!!", Color.Red, Color.Black, 10000))
                        {
                            frmMessageError.ShowDialog();
                        }
                    }
                }
            }
            return false;
        }

        private string GetAssyPN()
        {
            if (ClsUnitManagercs.cls_Units != null && ClsUnitManagercs.cls_Units.Length > 0 && ClsUnitManagercs.cls_Units[0] != null)
            {
                return ClsUnitManagercs.cls_Units[0].AssyPN ?? "";
            }
            return "";
        }

        private async void ResetSingleItem(ToolLifeCheck.ToolName toolName, int index)
        {
            if (! await ValidatePassword()) return;

            string assyPn = GetAssyPN();
            using var toolLife = new ToolLifeCheck(StateCommon.Secsion.ToString(), assyPn, toolName, index);

            if (toolName == ToolLifeCheck.ToolName.Oring)
            {
                toolLife.ResetOringCount(0);
                MessageBox.Show($"Reset O-Ring {index + 1} Time Used successfully!", "Reset Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                toolLife.ResetQDCount(0);
                MessageBox.Show($"Reset QD {index + 1} Time Used successfully!", "Reset Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
