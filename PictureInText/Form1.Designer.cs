
namespace PictureInText
{
  partial class Form1
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
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
      this.button1 = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox1.Location = new System.Drawing.Point(12, 56);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(590, 382);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBox1.TabIndex = 4;
      this.pictureBox1.TabStop = false;
      // 
      // textBox1
      // 
      this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox1.Location = new System.Drawing.Point(608, 56);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(180, 382);
      this.textBox1.TabIndex = 8;
      // 
      // button2
      // 
      this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.button2.Location = new System.Drawing.Point(649, 5);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(102, 45);
      this.button2.TabIndex = 9;
      this.button2.Text = "Save results";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // button3
      // 
      this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.button3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.button3.Location = new System.Drawing.Point(423, 5);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(102, 45);
      this.button3.TabIndex = 10;
      this.button3.Text = "Analize";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // checkedListBox1
      // 
      this.checkedListBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.checkedListBox1.CheckOnClick = true;
      this.checkedListBox1.FormattingEnabled = true;
      this.checkedListBox1.Items.AddRange(new object[] {
            "IronOcr",
            "AspriseOCR",
            "Vintasoft"});
      this.checkedListBox1.Location = new System.Drawing.Point(247, 1);
      this.checkedListBox1.MultiColumn = true;
      this.checkedListBox1.Name = "checkedListBox1";
      this.checkedListBox1.Size = new System.Drawing.Size(81, 49);
      this.checkedListBox1.TabIndex = 11;
      this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
      // 
      // button1
      // 
      this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.button1.Location = new System.Drawing.Point(74, 5);
      this.button1.MaximumSize = new System.Drawing.Size(102, 100);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(102, 45);
      this.button1.TabIndex = 12;
      this.button1.Text = "Set picture";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.checkedListBox1);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.pictureBox1);
      this.Name = "Form1";
      this.Text = "PictureInText";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.CheckedListBox checkedListBox1;
    private System.Windows.Forms.Button button1;
  }
}

