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
      // 
      // deleteButton
      // 
      this.deleteButton.Location = new System.Drawing.Point(488, 180);
      this.deleteButton.Name = "deleteButton";
      this.deleteButton.Size = new System.Drawing.Size(162, 23);
      this.deleteButton.TabIndex = 2;
      this.deleteButton.Text = "Delete a resource";
      this.deleteButton.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.deleteButton);
      this.Controls.Add(this.editButton);
      this.Controls.Add(this.addButton);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button addButton;
    private System.Windows.Forms.Button editButton;
    private System.Windows.Forms.Button deleteButton;
  }
}

