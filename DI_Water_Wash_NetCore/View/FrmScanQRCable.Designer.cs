
partial class FrmScanQRCable
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tableLayoutPanel1 = new TableLayoutPanel();
        tableLayoutPanel2 = new TableLayoutPanel();
        txt_QRInput = new TextBox();
        lbl_Infor = new Label();
        timer1 = new System.Windows.Forms.Timer(components);
        timer2 = new System.Windows.Forms.Timer(components);
        tableLayoutPanel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.BackColor = SystemColors.ControlLight;
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
        tableLayoutPanel1.Controls.Add(lbl_Infor, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 59.7315445F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.8421059F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 23.5087719F));
        tableLayoutPanel1.Size = new Size(637, 285);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 3;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        tableLayoutPanel2.Controls.Add(txt_QRInput, 1, 0);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(3, 173);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 1;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.Size = new Size(631, 41);
        tableLayoutPanel2.TabIndex = 0;
        // 
        // txt_QRInput
        // 
        txt_QRInput.Dock = DockStyle.Fill;
        txt_QRInput.Font = new Font("Century Gothic", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txt_QRInput.Location = new Point(66, 3);
        txt_QRInput.Name = "txt_QRInput";
        txt_QRInput.Size = new Size(498, 33);
        txt_QRInput.TabIndex = 0;
        txt_QRInput.TextChanged += txt_QRInput_TextChanged;
        // 
        // lbl_Infor
        // 
        lbl_Infor.AutoSize = true;
        lbl_Infor.BackColor = SystemColors.ActiveCaption;
        lbl_Infor.Dock = DockStyle.Fill;
        lbl_Infor.Font = new Font("Century Gothic", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lbl_Infor.Location = new Point(3, 0);
        lbl_Infor.Name = "lbl_Infor";
        lbl_Infor.Size = new Size(631, 170);
        lbl_Infor.TabIndex = 1;
        lbl_Infor.Text = "Scan Cable QR Code";
        lbl_Infor.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // timer1
        // 
        timer1.Enabled = true;
        timer1.Interval = 60000;
        timer1.Tick += timer1_Tick;
        // 
        // timer2
        // 
        timer2.Enabled = true;
        timer2.Tick += timer2_Tick;
        // 
        // FrmScanQRCable
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(637, 285);
        Controls.Add(tableLayoutPanel1);
        FormBorderStyle = FormBorderStyle.None;
        Name = "FrmScanQRCable";
        Text = "FrmScanQRCable";
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private TableLayoutPanel tableLayoutPanel2;
    private TextBox txt_QRInput;
    private Label lbl_Infor;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Timer timer2;
}
