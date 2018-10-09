using ResourceBlender.Common.ViewModels;
using ResourceBlender.Services.Contracts;
using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms
{
  public partial class DeleteForm : Form
  {
    private string resourceFolderPath;
    private readonly IComponentOperation componentOperation;
    private readonly IResourcesService resourcesService;
    private HttpClient httpClient;
    private ErrorProvider errorProvider;

    public DeleteForm(IResourcesService _resourcesService, IComponentOperation _componentOperation)
    {
      InitializeComponent();
      componentOperation = _componentOperation;
      resourcesService = _resourcesService;
      resourcesService.BaseUri = Properties.Settings.Default.BaseUri;
      httpClient = new HttpClient();
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      componentOperation.ClearTextBoxes(this);
      this.Hide();
    }

    private void resourceFolderButton_Click(object sender, EventArgs e)
    {
      resourceFolderPath = componentOperation.SetResourceFolderPath();
    }

    private async void deleteAndGenerateButton_Click(object sender, EventArgs e)
    {
      var defaultResourcesPath = Properties.Settings.Default.ResourcesPath;

      var isFormValid = ValidateChildren();

      if (!isFormValid)
      {
        MessageBox.Show("The resource field is required.");
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

      if (!defaultResourcesPath.Equals(string.Empty) && resourceFolderPath == null)
      {
        resourceFolderPath = defaultResourcesPath;
      }

      isFormValid = await resourcesService.CheckIfResourceWithNameExists(resourceTextBox.Text);
      if (!isFormValid)
      {
        MessageBox.Show("The resource could not be found.");
        return;
      }

      if (resourceFolderPath != null && !resourceFolderPath.Equals(String.Empty))
      {
        Properties.Settings.Default.ResourcesPath = resourceFolderPath;
        Properties.Settings.Default.Save();
      }

      ResourceViewModel resource = new ResourceViewModel();
      resource.ResourceString = resourceTextBox.Text;

      resourcesService.BaseUri = Properties.Settings.Default.BaseUri;

      await resourcesService.SendAndDeleteResource(resource);
      await resourcesService.ExtractResourcesToLocalFolder(resourceFolderPath);
      await resourcesService.GenerateJavascriptResources(resourceFolderPath);

      componentOperation.ClearTextBoxes(this);
      this.Hide();
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

    private void DeleteForm_Load(object sender, EventArgs e)
    {
      this.AutoValidate = AutoValidate.Disable;
    }
  }
}
