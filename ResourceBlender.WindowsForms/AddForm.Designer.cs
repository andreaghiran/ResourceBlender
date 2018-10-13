namespace ResourceBlender.WindowsForms
{
  partial class AddForm
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
            this.resourceStringLabel = new System.Windows.Forms.Label();
            this.romanianTranslationLabel = new System.Windows.Forms.Label();
            this.englishTranslationLabel = new System.Windows.Forms.Label();
            this.resourceStringTextBox = new System.Windows.Forms.TextBox();
            this.romanianTranslationTextBox = new System.Windows.Forms.TextBox();
            this.englishTranslationTextBox = new System.Windows.Forms.TextBox();
            this.addFormSubmitButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // resourceStringLabel
            // 
            this.resourceStringLabel.AutoSize = true;
            this.resourceStringLabel.Location = new System.Drawing.Point(188, 79);
            this.resourceStringLabel.Name = "resourceStringLabel";
            this.resourceStringLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceStringLabel.TabIndex = 0;
            this.resourceStringLabel.Text = "Resource Name";
            // 
            // romanianTranslationLabel
            // 
            this.romanianTranslationLabel.AutoSize = true;
            this.romanianTranslationLabel.Location = new System.Drawing.Point(188, 109);
            this.romanianTranslationLabel.Name = "romanianTranslationLabel";
            this.romanianTranslationLabel.Size = new System.Drawing.Size(110, 13);
            this.romanianTranslationLabel.TabIndex = 1;
            this.romanianTranslationLabel.Text = "Romanian Translation";
            // 
            // englishTranslationLabel
            // 
            this.englishTranslationLabel.AutoSize = true;
            this.englishTranslationLabel.Location = new System.Drawing.Point(188, 136);
            this.englishTranslationLabel.Name = "englishTranslationLabel";
            this.englishTranslationLabel.Size = new System.Drawing.Size(96, 13);
            this.englishTranslationLabel.TabIndex = 2;
            this.englishTranslationLabel.Text = "English Translation";
            // 
            // resourceStringTextBox
            // 
            this.resourceStringTextBox.Location = new System.Drawing.Point(309, 79);
            this.resourceStringTextBox.Name = "resourceStringTextBox";
            this.resourceStringTextBox.Size = new System.Drawing.Size(202, 20);
            this.resourceStringTextBox.TabIndex = 3;
            this.resourceStringTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // romanianTranslationTextBox
            // 
            this.romanianTranslationTextBox.Location = new System.Drawing.Point(309, 109);
            this.romanianTranslationTextBox.Name = "romanianTranslationTextBox";
            this.romanianTranslationTextBox.Size = new System.Drawing.Size(202, 20);
            this.romanianTranslationTextBox.TabIndex = 4;
            this.romanianTranslationTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // englishTranslationTextBox
            // 
            this.englishTranslationTextBox.Location = new System.Drawing.Point(309, 136);
            this.englishTranslationTextBox.Name = "englishTranslationTextBox";
            this.englishTranslationTextBox.Size = new System.Drawing.Size(202, 20);
            this.englishTranslationTextBox.TabIndex = 5;
            this.englishTranslationTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // addFormSubmitButton
            // 
            this.addFormSubmitButton.Location = new System.Drawing.Point(191, 204);
            this.addFormSubmitButton.Name = "addFormSubmitButton";
            this.addFormSubmitButton.Size = new System.Drawing.Size(196, 23);
            this.addFormSubmitButton.TabIndex = 6;
            this.addFormSubmitButton.Text = "Submit and Generate";
            this.addFormSubmitButton.UseVisualStyleBackColor = true;
            this.addFormSubmitButton.Click += new System.EventHandler(this.addFormSubmitButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(436, 204);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addFormSubmitButton);
            this.Controls.Add(this.englishTranslationTextBox);
            this.Controls.Add(this.romanianTranslationTextBox);
            this.Controls.Add(this.resourceStringTextBox);
            this.Controls.Add(this.englishTranslationLabel);
            this.Controls.Add(this.romanianTranslationLabel);
            this.Controls.Add(this.resourceStringLabel);
            this.Name = "AddForm";
            this.Text = "Add a resource";
            this.Load += new System.EventHandler(this.AddForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label resourceStringLabel;
    private System.Windows.Forms.Label romanianTranslationLabel;
    private System.Windows.Forms.Label englishTranslationLabel;
    private System.Windows.Forms.TextBox resourceStringTextBox;
    private System.Windows.Forms.TextBox romanianTranslationTextBox;
    private System.Windows.Forms.TextBox englishTranslationTextBox;
    private System.Windows.Forms.Button addFormSubmitButton;
    private System.Windows.Forms.Button cancelButton;
  }
}