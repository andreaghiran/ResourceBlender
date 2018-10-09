namespace ResourceBlender.WindowsForms
{
  partial class MainForm
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
      this.addButton = new System.Windows.Forms.Button();
      this.editButton = new System.Windows.Forms.Button();
      this.deleteButton = new System.Windows.Forms.Button();
      this.btnSetUrl = new System.Windows.Forms.Button();
      this.txtBoxUrl = new System.Windows.Forms.TextBox();
      this.txtFMF = new System.Windows.Forms.TextBox();
      this.btnSetFWFOlder = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // addButton
      // 
      this.addButton.Location = new System.Drawing.Point(104, 181);
      this.addButton.Name = "addButton";
      this.addButton.Size = new System.Drawing.Size(160, 23);
      this.addButton.TabIndex = 0;
      this.addButton.Text = "Add a resource";
      this.addButton.UseVisualStyleBackColor = true;
      this.addButton.Click += new System.EventHandler(this.addButton_Click);
      // 
      // editButton
      // 
      this.editButton.Location = new System.Drawing.Point(293, 181);
      this.editButton.Name = "editButton";
      this.editButton.Size = new System.Drawing.Size(160, 23);
      this.editButton.TabIndex = 1;
      this.editButton.Text = "Edit a resource";
      this.editButton.UseVisualStyleBackColor = true;
      this.editButton.Click += new System.EventHandler(this.editButton_Click);
      // 
      // deleteButton
      // 
      this.deleteButton.Location = new System.Drawing.Point(488, 180);
      this.deleteButton.Name = "deleteButton";
      this.deleteButton.Size = new System.Drawing.Size(162, 23);
      this.deleteButton.TabIndex = 2;
      this.deleteButton.Text = "Delete a resource";
      this.deleteButton.UseVisualStyleBackColor = true;
      this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
      // 
      // btnSetUrl
      // 
      this.btnSetUrl.Location = new System.Drawing.Point(464, 112);
      this.btnSetUrl.Name = "btnSetUrl";
      this.btnSetUrl.Size = new System.Drawing.Size(139, 23);
      this.btnSetUrl.TabIndex = 3;
      this.btnSetUrl.Text = "SET API URL";
      this.btnSetUrl.UseVisualStyleBackColor = true;
      this.btnSetUrl.Click += new System.EventHandler(this.btnSetUrl_Click);
      // 
      // txtBoxUrl
      // 
      this.txtBoxUrl.Location = new System.Drawing.Point(104, 115);
      this.txtBoxUrl.Name = "txtBoxUrl";
      this.txtBoxUrl.Size = new System.Drawing.Size(300, 20);
      this.txtBoxUrl.TabIndex = 4;
      // 
      // txtFMF
      // 
      this.txtFMF.Location = new System.Drawing.Point(104, 86);
      this.txtFMF.Name = "txtFMF";
      this.txtFMF.Size = new System.Drawing.Size(300, 20);
      this.txtFMF.TabIndex = 6;
      // 
      // btnSetFWFOlder
      // 
      this.btnSetFWFOlder.Location = new System.Drawing.Point(464, 83);
      this.btnSetFWFOlder.Name = "btnSetFWFOlder";
      this.btnSetFWFOlder.Size = new System.Drawing.Size(139, 23);
      this.btnSetFWFOlder.TabIndex = 5;
      this.btnSetFWFOlder.Text = "SET FRAMEWORK FOLDER";
      this.btnSetFWFOlder.UseVisualStyleBackColor = true;
      this.btnSetFWFOlder.Click += new System.EventHandler(this.btnSetFWFOlder_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.txtFMF);
      this.Controls.Add(this.btnSetFWFOlder);
      this.Controls.Add(this.txtBoxUrl);
      this.Controls.Add(this.btnSetUrl);
      this.Controls.Add(this.deleteButton);
      this.Controls.Add(this.editButton);
      this.Controls.Add(this.addButton);
      this.Name = "MainForm";
      this.Text = "Resource Editor";
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button addButton;
    private System.Windows.Forms.Button editButton;
    private System.Windows.Forms.Button deleteButton;
    private System.Windows.Forms.Button btnSetUrl;
    private System.Windows.Forms.TextBox txtBoxUrl;
    private System.Windows.Forms.TextBox txtFMF;
    private System.Windows.Forms.Button btnSetFWFOlder;
  }
}

