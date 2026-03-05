
partial class UC_PartNumberInfor
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
        label1 = new Label();
        label2 = new Label();
        txtPN = new TextBox();
        txtDescription = new TextBox();
        pl_UC1 = new Panel();
        panel1 = new Panel();
        panel2 = new Panel();
        panel3 = new Panel();
        tableLayoutPanel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
        tableLayoutPanel1.Controls.Add(pl_UC1, 0, 1);
        tableLayoutPanel1.Controls.Add(panel1, 0, 2);
        tableLayoutPanel1.Controls.Add(panel2, 0, 3);
        tableLayoutPanel1.Controls.Add(panel3, 0, 4);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 5;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 23.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 23.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 23.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 23.5F));
        tableLayoutPanel1.Size = new Size(1699, 710);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 4;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel2.Controls.Add(label1, 0, 0);
        tableLayoutPanel2.Controls.Add(label2, 2, 0);
        tableLayoutPanel2.Controls.Add(txtPN, 1, 0);
        tableLayoutPanel2.Controls.Add(txtDescription, 3, 0);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(5, 4);
        tableLayoutPanel2.Margin = new Padding(4, 3, 4, 3);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 1;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        tableLayoutPanel2.Size = new Size(1689, 36);
        tableLayoutPanel2.TabIndex = 0;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(4, 0);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(414, 36);
        label1.TabIndex = 0;
        label1.Text = "PN:";
        label1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Dock = DockStyle.Fill;
        label2.Location = new Point(848, 0);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new Size(414, 36);
        label2.TabIndex = 0;
        label2.Text = "Description:";
        label2.TextAlign = ContentAlignment.MiddleLeft;
        label2.Click += label2_Click;
        // 
        // txtPN
        // 
        txtPN.Dock = DockStyle.Fill;
        txtPN.Enabled = false;
        txtPN.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtPN.Location = new Point(426, 3);
        txtPN.Margin = new Padding(4, 3, 4, 3);
        txtPN.Name = "txtPN";
        txtPN.Size = new Size(414, 22);
        txtPN.TabIndex = 1;
        // 
        // txtDescription
        // 
        txtDescription.Dock = DockStyle.Fill;
        txtDescription.Enabled = false;
        txtDescription.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtDescription.Location = new Point(1270, 3);
        txtDescription.Margin = new Padding(4, 3, 4, 3);
        txtDescription.Name = "txtDescription";
        txtDescription.Size = new Size(415, 22);
        txtDescription.TabIndex = 1;
        // 
        // pl_UC1
        // 
        pl_UC1.Dock = DockStyle.Fill;
        pl_UC1.Location = new Point(5, 47);
        pl_UC1.Margin = new Padding(4, 3, 4, 3);
        pl_UC1.Name = "pl_UC1";
        pl_UC1.Size = new Size(1689, 159);
        pl_UC1.TabIndex = 1;
        // 
        // panel1
        // 
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(5, 213);
        panel1.Margin = new Padding(4, 3, 4, 3);
        panel1.Name = "panel1";
        panel1.Size = new Size(1689, 159);
        panel1.TabIndex = 1;
        // 
        // panel2
        // 
        panel2.Dock = DockStyle.Fill;
        panel2.Location = new Point(5, 379);
        panel2.Margin = new Padding(4, 3, 4, 3);
        panel2.Name = "panel2";
        panel2.Size = new Size(1689, 159);
        panel2.TabIndex = 1;
        // 
        // panel3
        // 
        panel3.Dock = DockStyle.Fill;
        panel3.Location = new Point(5, 545);
        panel3.Margin = new Padding(4, 3, 4, 3);
        panel3.Name = "panel3";
        panel3.Size = new Size(1689, 161);
        panel3.TabIndex = 1;
        // 
        // UC_PartNumberInfor
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.ActiveBorder;
        Controls.Add(tableLayoutPanel1);
        Margin = new Padding(4, 3, 4, 3);
        Name = "UC_PartNumberInfor";
        Size = new Size(1699, 710);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtPN;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Panel pl_UC1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel panel3;
}

