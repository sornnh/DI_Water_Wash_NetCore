namespace DI_Water_Wash_NetCore
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            pictureBox1 = new PictureBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            label5 = new Label();
            tableLayoutPanel7 = new TableLayoutPanel();
            lb_Ver = new Label();
            lb_Date = new Label();
            tableLayoutPanel6 = new TableLayoutPanel();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            lb_CPU = new Label();
            lb_MEM = new Label();
            lb_DiskC = new Label();
            progressBar_CPU = new ProgressBar();
            progressBar_MEM = new ProgressBar();
            progressBar_C = new ProgressBar();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel5 = new TableLayoutPanel();
            lbl_Exit = new Label();
            lbl_Parameter = new Label();
            lbl_Setting = new Label();
            lbl_Main = new Label();
            lbl_Interface = new Label();
            pl_Main = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            lbl_ResetToolLife = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10.9F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 89.1F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tableLayoutPanel1.Size = new Size(1900, 1000);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            tableLayoutPanel2.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel4, 1, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel6, 2, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 133F));
            tableLayoutPanel2.Size = new Size(1894, 103);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = Properties.Resources.Capture;
            pictureBox1.Location = new Point(4, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(504, 95);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Controls.Add(label5, 0, 0);
            tableLayoutPanel4.Controls.Add(tableLayoutPanel7, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(515, 4);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 69.09091F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 30.90909F));
            tableLayoutPanel4.Size = new Size(863, 95);
            tableLayoutPanel4.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Century Gothic", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(857, 65);
            label5.TabIndex = 0;
            label5.Text = "DI Water Wash";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Controls.Add(lb_Ver, 0, 0);
            tableLayoutPanel7.Controls.Add(lb_Date, 2, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 68);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Size = new Size(857, 24);
            tableLayoutPanel7.TabIndex = 1;
            // 
            // lb_Ver
            // 
            lb_Ver.AutoSize = true;
            lb_Ver.Dock = DockStyle.Fill;
            lb_Ver.Location = new Point(3, 0);
            lb_Ver.Name = "lb_Ver";
            lb_Ver.Size = new Size(422, 24);
            lb_Ver.TabIndex = 0;
            lb_Ver.Text = "Ver";
            lb_Ver.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lb_Date
            // 
            lb_Date.AutoSize = true;
            lb_Date.Dock = DockStyle.Fill;
            lb_Date.Location = new Point(431, 0);
            lb_Date.Name = "lb_Date";
            lb_Date.Size = new Size(423, 24);
            lb_Date.TabIndex = 1;
            lb_Date.Text = "Date";
            lb_Date.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 3;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.Controls.Add(label2, 0, 0);
            tableLayoutPanel6.Controls.Add(label3, 0, 1);
            tableLayoutPanel6.Controls.Add(label4, 0, 2);
            tableLayoutPanel6.Controls.Add(lb_CPU, 2, 0);
            tableLayoutPanel6.Controls.Add(lb_MEM, 2, 1);
            tableLayoutPanel6.Controls.Add(lb_DiskC, 2, 2);
            tableLayoutPanel6.Controls.Add(progressBar_CPU, 1, 0);
            tableLayoutPanel6.Controls.Add(progressBar_MEM, 1, 1);
            tableLayoutPanel6.Controls.Add(progressBar_C, 1, 2);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(1385, 4);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 3;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.Size = new Size(505, 95);
            tableLayoutPanel6.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Century Gothic", 12F);
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(145, 31);
            label2.TabIndex = 0;
            label2.Text = "CPU:";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Century Gothic", 12F);
            label3.Location = new Point(3, 31);
            label3.Name = "label3";
            label3.Size = new Size(145, 31);
            label3.TabIndex = 1;
            label3.Text = "Memmory:";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Century Gothic", 12F);
            label4.Location = new Point(3, 62);
            label4.Name = "label4";
            label4.Size = new Size(145, 33);
            label4.TabIndex = 2;
            label4.Text = "C :";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lb_CPU
            // 
            lb_CPU.AutoSize = true;
            lb_CPU.Dock = DockStyle.Fill;
            lb_CPU.Font = new Font("Century Gothic", 12F);
            lb_CPU.Location = new Point(406, 0);
            lb_CPU.Name = "lb_CPU";
            lb_CPU.Size = new Size(96, 31);
            lb_CPU.TabIndex = 3;
            lb_CPU.Text = "0%";
            lb_CPU.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lb_MEM
            // 
            lb_MEM.AutoSize = true;
            lb_MEM.Dock = DockStyle.Fill;
            lb_MEM.Font = new Font("Century Gothic", 12F);
            lb_MEM.Location = new Point(406, 31);
            lb_MEM.Name = "lb_MEM";
            lb_MEM.Size = new Size(96, 31);
            lb_MEM.TabIndex = 4;
            lb_MEM.Text = "0%";
            lb_MEM.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lb_DiskC
            // 
            lb_DiskC.AutoSize = true;
            lb_DiskC.Dock = DockStyle.Fill;
            lb_DiskC.Font = new Font("Century Gothic", 12F);
            lb_DiskC.Location = new Point(406, 62);
            lb_DiskC.Name = "lb_DiskC";
            lb_DiskC.Size = new Size(96, 33);
            lb_DiskC.TabIndex = 5;
            lb_DiskC.Text = "0%";
            lb_DiskC.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar_CPU
            // 
            progressBar_CPU.Dock = DockStyle.Fill;
            progressBar_CPU.Location = new Point(154, 3);
            progressBar_CPU.Name = "progressBar_CPU";
            progressBar_CPU.Size = new Size(246, 25);
            progressBar_CPU.TabIndex = 6;
            // 
            // progressBar_MEM
            // 
            progressBar_MEM.Dock = DockStyle.Fill;
            progressBar_MEM.Location = new Point(154, 34);
            progressBar_MEM.Name = "progressBar_MEM";
            progressBar_MEM.Size = new Size(246, 25);
            progressBar_MEM.TabIndex = 7;
            // 
            // progressBar_C
            // 
            progressBar_C.Dock = DockStyle.Fill;
            progressBar_C.Location = new Point(154, 65);
            progressBar_C.Name = "progressBar_C";
            progressBar_C.Size = new Size(246, 27);
            progressBar_C.TabIndex = 8;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel5, 0, 0);
            tableLayoutPanel3.Controls.Add(pl_Main, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 112);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 4.18552F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 95.814476F));
            tableLayoutPanel3.Size = new Size(1894, 885);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 6;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.Controls.Add(lbl_ResetToolLife, 4, 0);
            tableLayoutPanel5.Controls.Add(lbl_Exit, 5, 0);
            tableLayoutPanel5.Controls.Add(lbl_Parameter, 3, 0);
            tableLayoutPanel5.Controls.Add(lbl_Setting, 2, 0);
            tableLayoutPanel5.Controls.Add(lbl_Main, 0, 0);
            tableLayoutPanel5.Controls.Add(lbl_Interface, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(4, 4);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(1886, 30);
            tableLayoutPanel5.TabIndex = 1;
            // 
            // lbl_Exit
            // 
            lbl_Exit.AutoSize = true;
            lbl_Exit.BackColor = SystemColors.ActiveCaption;
            lbl_Exit.Dock = DockStyle.Fill;
            lbl_Exit.Font = new Font("Century Gothic", 12F);
            lbl_Exit.Location = new Point(1573, 0);
            lbl_Exit.Name = "lbl_Exit";
            lbl_Exit.Size = new Size(310, 30);
            lbl_Exit.TabIndex = 3;
            lbl_Exit.Text = "EXIT";
            lbl_Exit.TextAlign = ContentAlignment.MiddleCenter;
            lbl_Exit.Click += lbl_Exit_Click;
            // 
            // lbl_Parameter
            // 
            lbl_Parameter.AutoSize = true;
            lbl_Parameter.BackColor = SystemColors.ActiveCaption;
            lbl_Parameter.Dock = DockStyle.Fill;
            lbl_Parameter.Font = new Font("Century Gothic", 12F);
            lbl_Parameter.Location = new Point(945, 0);
            lbl_Parameter.Name = "lbl_Parameter";
            lbl_Parameter.Size = new Size(308, 30);
            lbl_Parameter.TabIndex = 2;
            lbl_Parameter.Text = "PARAMETER";
            lbl_Parameter.TextAlign = ContentAlignment.MiddleCenter;
            lbl_Parameter.Click += lbl_Parameter_Click;
            // 
            // lbl_Setting
            // 
            lbl_Setting.AutoSize = true;
            lbl_Setting.BackColor = SystemColors.ActiveCaption;
            lbl_Setting.Dock = DockStyle.Fill;
            lbl_Setting.Font = new Font("Century Gothic", 12F);
            lbl_Setting.Location = new Point(631, 0);
            lbl_Setting.Name = "lbl_Setting";
            lbl_Setting.Size = new Size(308, 30);
            lbl_Setting.TabIndex = 1;
            lbl_Setting.Text = "SETTING";
            lbl_Setting.TextAlign = ContentAlignment.MiddleCenter;
            lbl_Setting.Click += lbl_Setting_Click;
            // 
            // lbl_Main
            // 
            lbl_Main.AutoSize = true;
            lbl_Main.BackColor = SystemColors.ActiveCaption;
            lbl_Main.Dock = DockStyle.Fill;
            lbl_Main.Font = new Font("Century Gothic", 12F);
            lbl_Main.Location = new Point(3, 0);
            lbl_Main.Name = "lbl_Main";
            lbl_Main.Size = new Size(308, 30);
            lbl_Main.TabIndex = 0;
            lbl_Main.Text = "MAIN";
            lbl_Main.TextAlign = ContentAlignment.MiddleCenter;
            lbl_Main.Click += lbl_Main_Click;
            // 
            // lbl_Interface
            // 
            lbl_Interface.AutoSize = true;
            lbl_Interface.BackColor = SystemColors.ActiveCaption;
            lbl_Interface.Dock = DockStyle.Fill;
            lbl_Interface.Font = new Font("Century Gothic", 12F);
            lbl_Interface.Location = new Point(317, 0);
            lbl_Interface.Name = "lbl_Interface";
            lbl_Interface.Size = new Size(308, 30);
            lbl_Interface.TabIndex = 0;
            lbl_Interface.Text = "INTERFACE";
            lbl_Interface.TextAlign = ContentAlignment.MiddleCenter;
            lbl_Interface.Click += lbl_Interface_Click;
            // 
            // pl_Main
            // 
            pl_Main.Dock = DockStyle.Fill;
            pl_Main.Location = new Point(4, 41);
            pl_Main.Name = "pl_Main";
            pl_Main.Size = new Size(1886, 840);
            pl_Main.TabIndex = 3;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // lbl_ResetToolLife
            // 
            lbl_ResetToolLife.AutoSize = true;
            lbl_ResetToolLife.BackColor = SystemColors.ActiveCaption;
            lbl_ResetToolLife.Dock = DockStyle.Fill;
            lbl_ResetToolLife.Font = new Font("Century Gothic", 12F);
            lbl_ResetToolLife.Location = new Point(1259, 0);
            lbl_ResetToolLife.Name = "lbl_ResetToolLife";
            lbl_ResetToolLife.Size = new Size(308, 30);
            lbl_ResetToolLife.TabIndex = 4;
            lbl_ResetToolLife.Text = "RESET TOOL LIFE";
            lbl_ResetToolLife.TextAlign = ContentAlignment.MiddleCenter;
            lbl_ResetToolLife.Click += lbl_ResetToolLife_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1900, 1000);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            FormClosing += FrmMain_FormClosing;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private PictureBox pictureBox1;
        private TableLayoutPanel tableLayoutPanel4;
        private Label label5;
        private TableLayoutPanel tableLayoutPanel7;
        private Label lb_Ver;
        private Label lb_Date;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label lb_CPU;
        private Label lb_MEM;
        private Label lb_DiskC;
        private ProgressBar progressBar_CPU;
        private ProgressBar progressBar_MEM;
        private ProgressBar progressBar_C;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel5;
        private Label lbl_Exit;
        private Label lbl_Parameter;
        private Label lbl_Setting;
        private Label lbl_Main;
        private Label lbl_Interface;
        private Panel pl_Main;
        private System.Windows.Forms.Timer timer1;
        private Label lbl_ResetToolLife;
    }
}
