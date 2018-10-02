using Newtonsoft.Json;
using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using ResourceBlender.Services.Contracts;
using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms
{
  public partial class AddForm : Form
  {
    private readonly IResourcesService resourceService;
    private readonly ITextBoxOperation textBoxOperation;
    private string resourceFolderPath;
    //private ErrorProvider errorProvider;

    public AddForm(IResourcesService _resourceService, ITextBoxOperation _textBoxOperation)
    {
      InitializeComponent();
      resourceService = _resourceService;
      textBoxOperation = _textBoxOperation;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      //this.AutoValidate = AutoValidate.Disable;
      textBoxOperation.ClearTextBoxes(this);
      this.Hide();
    }

    private async void addFormSubmitButton_Click(object sender, EventArgs e)
    {
      ResourceViewModel resource = new ResourceViewModel();

      resource.ResourceString = resourceStringTextBox.Text;
      resource.EnglishTranslation = englishTranslationTextBox.Text;
      resource.RomanianTranslation = romanianTranslationTextBox.Text;

      //resourceStringTextBox.Validating += new CancelEventHandler(textBox_Validating);
      //resourceService.AddResource(resource);
      await resourceService.ZipResources(resourceFolderPath);
      textBoxOperation.ClearTextBoxes(this);
      this.Hide();
    }

    private void chooseResourceFolderButton_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog resourcesFolderDialog = new FolderBrowserDialog();
      resourcesFolderDialog.ShowNewFolderButton = true;
      // Show the FolderBrowserDialog.  
      DialogResult result = resourcesFolderDialog.ShowDialog();
      if (result == DialogResult.OK)
      {
        resourceFolderPath = resourcesFolderDialog.SelectedPath;
      }
    }

    //private void textBox_Validating(object sender, CancelEventArgs e)
    //{
    //  TextBox tb = (TextBox)sender;
    //  errorProvider = new ErrorProvider();
    //  if (String.IsNullOrEmpty(tb.Text))
    //  {
    //    errorProvider.SetError(tb, "*");
    //    e.Cancel = true;
    //    return;
    //  }

    //  errorProvider.SetError(tb, String.Empty);
    //}
  }
}
