using System.Drawing;
using System.Windows.Forms;


partial class FrmLoading
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
        pictureBox1 = new PictureBox();
        progressBar1 = new ProgressBar();
        richTextBox1 = new RichTextBox();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // pictureBox1
        // 
        pictureBox1.Image = DI_Water_Wash_NetCore.Properties.Resources.Capture;
        pictureBox1.Location = new Point(18, 14);
        pictureBox1.Margin = new Padding(4, 3, 4, 3);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(366, 122);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 2;
        pictureBox1.TabStop = false;
        // 
        // progressBar1
        // 
        progressBar1.Location = new Point(18, 167);
        progressBar1.Margin = new Padding(4, 3, 4, 3);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new Size(363, 23);
        progressBar1.TabIndex = 3;
        // 
        // richTextBox1
        // 
        richTextBox1.Location = new Point(18, 218);
        richTextBox1.Margin = new Padding(4, 3, 4, 3);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(362, 335);
        richTextBox1.TabIndex = 4;
        richTextBox1.Text = "";
        // 
        // FrmLoading
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(398, 568);
        ControlBox = false;
        Controls.Add(richTextBox1);
        Controls.Add(progressBar1);
        Controls.Add(pictureBox1);
        FormBorderStyle = FormBorderStyle.None;
        Margin = new Padding(4, 3, 4, 3);
        Name = "FrmLoading";
        ShowIcon = false;
        Text = "FrmLoading";
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.RichTextBox richTextBox1;
}
