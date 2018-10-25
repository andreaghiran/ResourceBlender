using ResourceBlender.Common.Exceptions;
using ResourceBlender.Common.ViewModels;
using ResourceBlender.Services.Contracts;
using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms
{
  public partial class EditForm : Form
  {
    private readonly IComponentOperation componentOperation;
    private readonly IResourcesService resourcesService;
    private HttpClient httpClient;
    private ErrorProvider errorProvider;

    public EditForm(IResourcesService _resourcesService, IComponentOperation _componentOperation)
    {
      InitializeComponent();
      componentOperation = _componentOperation;
      resourcesService = _resourcesService;
      resourcesService.BaseUri = Properties.Settings.Default.BaseUri;
      httpClient = new HttpClient();
    }


    private async void updateAndGenerateButton_Click(object sender, EventArgs e)
    {
      bool isFormValid = ValidateChildren();

      if (!isFormValid)
      {
        MessageBox.Show("The resource field is required.");
        return;
      }

      try
      {
        ResourceViewModel resource = new ResourceViewModel();

        resource.ResourceString = resourceTextBox.Text;
        resource.EnglishTranslation = englishTranslationTextBox.Text;
        resource.RomanianTranslation = romanianTranslationTextBox.Text;

        await resourcesService.UpdateResource(resource);
        await resourcesService.ExtractResourcesToLocalFolder(Properties.Settings.Default.ResourcesPath);

        componentOperation.ClearTextBoxes(this);
        MessageBox.Show("Updated resource.");
        this.Hide();
      }
      catch(ResourceDoesNotExistException ex)
      {
        MessageBox.Show(ex.Message);
      }
      catch(Exception)
      {
        MessageBox.Show("Something went wrong. Make sure that your Url and Framework folder path are set correctly.");
      }
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
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

    private void EditForm_Load(object sender, EventArgs e)
    {
      this.AutoValidate = AutoValidate.Disable;
    }
  }
}
