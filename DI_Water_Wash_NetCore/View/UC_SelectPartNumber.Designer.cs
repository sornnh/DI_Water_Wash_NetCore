
using System.Drawing;
using System.Windows.Forms;

partial class UC_SelectPartNumber
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tableLayoutPanel1 = new TableLayoutPanel();
        tableLayoutPanel2 = new TableLayoutPanel();
        btn_ChangePart = new Button();
        tableLayoutPanel3 = new TableLayoutPanel();
        txt_WO = new TextBox();
        lb_WO = new Label();
        checkBox1 = new CheckBox();
        cbb_PartNumberInfo = new ComboBox();
        txt_Description = new TextBox();
        lb_Fixture = new Label();
        txt_PN = new TextBox();
        tableLayoutPanel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        tableLayoutPanel3.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 23F));
        tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
        tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 75.53191F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 24.46808F));
        tableLayoutPanel1.Size = new Size(454, 217);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 3;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
        tableLayoutPanel2.Controls.Add(btn_ChangePart, 1, 0);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(4, 166);
        tableLayoutPanel2.Margin = new Padding(4, 3, 4, 3);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 1;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        tableLayoutPanel2.Size = new Size(446, 48);
        tableLayoutPanel2.TabIndex = 0;
        // 
        // btn_ChangePart
        // 
        btn_ChangePart.Dock = DockStyle.Fill;
        btn_ChangePart.Location = new Point(152, 3);
        btn_ChangePart.Margin = new Padding(4, 3, 4, 3);
        btn_ChangePart.Name = "btn_ChangePart";
        btn_ChangePart.Size = new Size(140, 42);
        btn_ChangePart.TabIndex = 0;
        btn_ChangePart.Text = "Save Part";
        btn_ChangePart.UseVisualStyleBackColor = true;
        btn_ChangePart.Click += btn_ChangePart_Click;
        // 
        // tableLayoutPanel3
        // 
        tableLayoutPanel3.ColumnCount = 2;
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.45953F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.54047F));
        tableLayoutPanel3.Controls.Add(txt_WO, 1, 2);
        tableLayoutPanel3.Controls.Add(lb_WO, 0, 2);
        tableLayoutPanel3.Controls.Add(checkBox1, 1, 3);
        tableLayoutPanel3.Controls.Add(cbb_PartNumberInfo, 1, 0);
        tableLayoutPanel3.Controls.Add(txt_Description, 1, 1);
        tableLayoutPanel3.Controls.Add(lb_Fixture, 0, 0);
        tableLayoutPanel3.Controls.Add(txt_PN, 0, 1);
        tableLayoutPanel3.Dock = DockStyle.Fill;
        tableLayoutPanel3.Location = new Point(4, 3);
        tableLayoutPanel3.Margin = new Padding(4, 3, 4, 3);
        tableLayoutPanel3.Name = "tableLayoutPanel3";
        tableLayoutPanel3.RowCount = 4;
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        tableLayoutPanel3.Size = new Size(446, 157);
        tableLayoutPanel3.TabIndex = 1;
        // 
        // txt_WO
        // 
        txt_WO.Dock = DockStyle.Fill;
        txt_WO.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txt_WO.Location = new Point(130, 81);
        txt_WO.Margin = new Padding(4, 3, 4, 3);
        txt_WO.Name = "txt_WO";
        txt_WO.Size = new Size(312, 26);
        txt_WO.TabIndex = 2;
        // 
        // lb_WO
        // 
        lb_WO.AutoSize = true;
        lb_WO.Dock = DockStyle.Fill;
        lb_WO.Location = new Point(4, 78);
        lb_WO.Margin = new Padding(4, 0, 4, 0);
        lb_WO.Name = "lb_WO";
        lb_WO.Size = new Size(118, 39);
        lb_WO.TabIndex = 5;
        lb_WO.Text = "Work Oder";
        lb_WO.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Dock = DockStyle.Fill;
        checkBox1.Location = new Point(130, 120);
        checkBox1.Margin = new Padding(4, 3, 4, 3);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new Size(312, 34);
        checkBox1.TabIndex = 7;
        checkBox1.Text = "Dual Unit Fixture";
        checkBox1.UseVisualStyleBackColor = true;
        // 
        // cbb_PartNumberInfo
        // 
        cbb_PartNumberInfo.Dock = DockStyle.Fill;
        cbb_PartNumberInfo.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbb_PartNumberInfo.FormattingEnabled = true;
        cbb_PartNumberInfo.Location = new Point(130, 3);
        cbb_PartNumberInfo.Margin = new Padding(4, 3, 4, 3);
        cbb_PartNumberInfo.Name = "cbb_PartNumberInfo";
        cbb_PartNumberInfo.Size = new Size(312, 28);
        cbb_PartNumberInfo.TabIndex = 8;
        cbb_PartNumberInfo.SelectedIndexChanged += cbb_PartNumberInfo_SelectedIndexChanged;
        // 
        // txt_Description
        // 
        txt_Description.Dock = DockStyle.Fill;
        txt_Description.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txt_Description.Location = new Point(130, 42);
        txt_Description.Margin = new Padding(4, 3, 4, 3);
        txt_Description.Name = "txt_Description";
        txt_Description.Size = new Size(312, 26);
        txt_Description.TabIndex = 0;
        // 
        // lb_Fixture
        // 
        lb_Fixture.AutoSize = true;
        lb_Fixture.Dock = DockStyle.Fill;
        lb_Fixture.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lb_Fixture.Location = new Point(4, 0);
        lb_Fixture.Margin = new Padding(4, 0, 4, 0);
        lb_Fixture.Name = "lb_Fixture";
        lb_Fixture.Size = new Size(118, 39);
        lb_Fixture.TabIndex = 9;
        lb_Fixture.Text = "Fixture";
        lb_Fixture.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // txt_PN
        // 
        txt_PN.Dock = DockStyle.Fill;
        txt_PN.Font = new Font("Microsoft Sans Serif", 12F);
        txt_PN.Location = new Point(4, 42);
        txt_PN.Margin = new Padding(4, 3, 4, 3);
        txt_PN.Name = "txt_PN";
        txt_PN.Size = new Size(118, 26);
        txt_PN.TabIndex = 10;
        // 
        // UC_SelectPartNumber
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel1);
        Margin = new Padding(4, 3, 4, 3);
        Name = "UC_SelectPartNumber";
        Size = new Size(454, 217);
        Load += UC_SelectPartNumber_Load;
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel3.ResumeLayout(false);
        tableLayoutPanel3.PerformLayout();
        ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button btn_ChangePart;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.TextBox txt_Description;
    private System.Windows.Forms.TextBox txt_WO;
    private System.Windows.Forms.Label lb_WO;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.ComboBox cbb_PartNumberInfo;
    private System.Windows.Forms.Label lb_Fixture;
    private System.Windows.Forms.TextBox txt_PN;
}

