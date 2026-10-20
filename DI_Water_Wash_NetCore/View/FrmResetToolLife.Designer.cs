namespace LifeTimeToolCheck
{
    partial class FrmResetToolLife
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            lblSubtitle = new System.Windows.Forms.Label();
            grpOring = new System.Windows.Forms.GroupBox();
            flpOring = new System.Windows.Forms.FlowLayoutPanel();
            grpQD = new System.Windows.Forms.GroupBox();
            flpQD = new System.Windows.Forms.FlowLayoutPanel();
            pnlFooter = new System.Windows.Forms.Panel();
            btnClose = new System.Windows.Forms.Button();
            pnlHeader.SuspendLayout();
            grpOring.SuspendLayout();
            grpQD.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.SystemColors.ActiveCaption;
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(640, 55);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitle.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblTitle.Location = new System.Drawing.Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(640, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "TOOL LIFE RESET MANAGEMENT";
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblSubtitle.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            lblSubtitle.Location = new System.Drawing.Point(0, 30);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new System.Drawing.Size(640, 25);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Fixture Mode: Single";
            lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpOring
            // 
            grpOring.Controls.Add(flpOring);
            grpOring.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            grpOring.Location = new System.Drawing.Point(12, 65);
            grpOring.Name = "grpOring";
            grpOring.Size = new System.Drawing.Size(616, 150);
            grpOring.TabIndex = 1;
            grpOring.TabStop = false;
            grpOring.Text = "O-Ring Tool Life Counter Reset";
            // 
            // flpOring
            // 
            flpOring.AutoScroll = true;
            flpOring.Dock = System.Windows.Forms.DockStyle.Fill;
            flpOring.Location = new System.Drawing.Point(3, 19);
            flpOring.Name = "flpOring";
            flpOring.Padding = new System.Windows.Forms.Padding(5);
            flpOring.Size = new System.Drawing.Size(610, 128);
            flpOring.TabIndex = 0;
            // 
            // grpQD
            // 
            grpQD.Controls.Add(flpQD);
            grpQD.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            grpQD.Location = new System.Drawing.Point(12, 225);
            grpQD.Name = "grpQD";
            grpQD.Size = new System.Drawing.Size(616, 150);
            grpQD.TabIndex = 2;
            grpQD.TabStop = false;
            grpQD.Text = "QD Tool Life Counter Reset";
            // 
            // flpQD
            // 
            flpQD.AutoScroll = true;
            flpQD.Dock = System.Windows.Forms.DockStyle.Fill;
            flpQD.Location = new System.Drawing.Point(3, 19);
            flpQD.Name = "flpQD";
            flpQD.Padding = new System.Windows.Forms.Padding(5);
            flpQD.Size = new System.Drawing.Size(610, 128);
            flpQD.TabIndex = 0;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(btnClose);
            pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlFooter.Location = new System.Drawing.Point(0, 385);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new System.Drawing.Size(640, 45);
            pnlFooter.TabIndex = 3;
            // 
            // btnClose
            // 
            btnClose.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnClose.Location = new System.Drawing.Point(265, 7);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(110, 30);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // FrmResetToolLife
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(640, 430);
            Controls.Add(pnlFooter);
            Controls.Add(grpQD);
            Controls.Add(grpOring);
            Controls.Add(pnlHeader);
            Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmResetToolLife";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Tool Life Reset Management";
            pnlHeader.ResumeLayout(false);
            grpOring.ResumeLayout(false);
            grpQD.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.GroupBox grpOring;
        private System.Windows.Forms.FlowLayoutPanel flpOring;
        private System.Windows.Forms.GroupBox grpQD;
        private System.Windows.Forms.FlowLayoutPanel flpQD;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnClose;
    }
}
