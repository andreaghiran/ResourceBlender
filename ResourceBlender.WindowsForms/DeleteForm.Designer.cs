namespace ResourceBlender.WindowsForms
{
  partial class DeleteForm
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
            this.deleteAndGenerateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.resourceLabel = new System.Windows.Forms.Label();
            this.resourceTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // deleteAndGenerateButton
            // 
            this.deleteAndGenerateButton.Location = new System.Drawing.Point(185, 190);
            this.deleteAndGenerateButton.Name = "deleteAndGenerateButton";
            this.deleteAndGenerateButton.Size = new System.Drawing.Size(194, 23);
            this.deleteAndGenerateButton.TabIndex = 0;
            this.deleteAndGenerateButton.Text = "Delete and Generate";
            this.deleteAndGenerateButton.UseVisualStyleBackColor = true;
            this.deleteAndGenerateButton.Click += new System.EventHandler(this.deleteAndGenerateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.CausesValidation = false;
            this.cancelButton.Location = new System.Drawing.Point(518, 190);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // resourceLabel
            // 
            this.resourceLabel.AutoSize = true;
            this.resourceLabel.Location = new System.Drawing.Point(182, 137);
            this.resourceLabel.Name = "resourceLabel";
            this.resourceLabel.Size = new System.Drawing.Size(190, 13);
            this.resourceLabel.TabIndex = 3;
            this.resourceLabel.Text = "Enter the resource you want to remove";
            // 
            // resourceTextBox
            // 
            this.resourceTextBox.Location = new System.Drawing.Point(379, 134);
            this.resourceTextBox.Name = "resourceTextBox";
            this.resourceTextBox.Size = new System.Drawing.Size(214, 20);
            this.resourceTextBox.TabIndex = 4;
            this.resourceTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // DeleteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.resourceTextBox);
            this.Controls.Add(this.resourceLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.deleteAndGenerateButton);
            this.Name = "DeleteForm";
            this.Text = "DeleteForm";
            this.Load += new System.EventHandler(this.DeleteForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button deleteAndGenerateButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Label resourceLabel;
    private System.Windows.Forms.TextBox resourceTextBox;
  }
}