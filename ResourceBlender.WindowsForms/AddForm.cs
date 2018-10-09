using ResourceBlender.Common.ViewModels;
using ResourceBlender.Services.Contracts;
using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.ComponentModel;
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
      resourceService.BaseUri = Properties.Settings.Default.BaseUri;
      componentOperation = _componentOperation;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      componentOperation.ClearTextBoxes(this);
      this.Hide();
    }

    private async void addFormSubmitButton_Click(object sender, EventArgs e)
    {
      var defaultResourcesPath = Properties.Settings.Default.ResourcesPath;

      var isFormValid = ValidateChildren();

      if (!isFormValid)
      {
        MessageBox.Show("All fields are required.");
        return;
      }


      if (defaultResourcesPath.Equals(string.Empty))
      {
        isFormValid = !(resourceFolderPath == null);
        if (!isFormValid)
        {
          MessageBox.Show("You must choose a folder.");
          return;
        }
      }

      if(!defaultResourcesPath.Equals(string.Empty) && resourceFolderPath == null)
      {
        resourceFolderPath = defaultResourcesPath;
      }

      isFormValid =!(await resourceService.CheckIfResourceWithNameExists(resourceStringTextBox.Text));
      if (!isFormValid)
      {
        MessageBox.Show("A resource with the same name already exists.");
        return;
      }

      if (resourceFolderPath != null && !resourceFolderPath.Equals(String.Empty))
      {
        Properties.Settings.Default.ResourcesPath = resourceFolderPath;
        Properties.Settings.Default.Save();
      }

      ResourceViewModel resource = new ResourceViewModel();

      resource.ResourceString = resourceStringTextBox.Text;
      resource.EnglishTranslation = englishTranslationTextBox.Text;
      resource.RomanianTranslation = romanianTranslationTextBox.Text;

      await resourceService.SendAndAddResource(resource);
      await resourceService.ExtractResourcesToLocalFolder(resourceFolderPath);
      await resourceService.GenerateJavascriptResources(resourceFolderPath);

      componentOperation.ClearTextBoxes(this);
      this.Hide();
    }

    private void chooseResourceFolderButton_Click(object sender, EventArgs e)
    {
      resourceFolderPath = componentOperation.SetResourceFolderPath();
    }

    private void textBox_Validating(object sender, CancelEventArgs e)
    {
      TextBox tb = (TextBox)sender;
      errorProvider = new ErrorProvider();
      if (String.IsNullOrEmpty(tb.Text))
      {
        errorProvider.SetError(tb, "This field is required.");
        e.Cancel = true;
      }
      else
      {
        e.Cancel = false;
      }

      errorProvider.SetError(tb, String.Empty);
    }

    private void AddForm_Load(object sender, EventArgs e)
    {
      this.AutoValidate = AutoValidate.Disable;
    }
  }
}
