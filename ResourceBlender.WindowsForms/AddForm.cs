using Newtonsoft.Json;
using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using ResourceBlender.Services.Contracts;
using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms
{
  public partial class AddForm : Form
  {
    private readonly IResourcesService resourceService;
    private readonly IComponentOperation componentOperation;
    private string resourceFolderPath;
    private ErrorProvider errorProvider;

    public AddForm(IResourcesService _resourceService, IComponentOperation _componentOperation)
    {
      InitializeComponent();
      resourceService = _resourceService;
      componentOperation = _componentOperation;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      componentOperation.ClearTextBoxes(this);
      this.Hide();
    }

    private async void addFormSubmitButton_Click(object sender, EventArgs e)
    {
      var isFormValid = componentOperation.ValidateTextBoxes(this);
      isFormValid = !resourceFolderPath.Equals(string.Empty);

      if (isFormValid)
      {
        ResourceViewModel resource = new ResourceViewModel();

        resource.ResourceString = resourceStringTextBox.Text;
        resource.EnglishTranslation = englishTranslationTextBox.Text;
        resource.RomanianTranslation = romanianTranslationTextBox.Text;

        await resourceService.SendAndAddResource(resource);
        await resourceService.ExtractResourcesToLocalFolder(resourceFolderPath);
        componentOperation.ClearTextBoxes(this);
        this.Hide();
      }

      else
      {

      }
    }

    private void chooseResourceFolderButton_Click(object sender, EventArgs e)
    {
      resourceFolderPath = componentOperation.SetResourceFolderPath();
    }

    //private void textBox_Validating(object sender, CancelEventArgs e)
    //{
    //  TextBox tb = (TextBox)sender;
    //  errorProvider = new ErrorProvider();
    //  if (String.IsNullOrEmpty(tb.Text))
    //  {
    //    errorProvider.SetError(tb, "This field is required.");
    //    e.Cancel = true;
    //    return;
    //  }

    //  errorProvider.SetError(tb, String.Empty);
    //}
  }
}
