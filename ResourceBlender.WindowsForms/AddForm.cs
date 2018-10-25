using ResourceBlender.Common.Exceptions;
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
      
      var isFormValid = ValidateChildren();

      if (!isFormValid)
      {
        MessageBox.Show("All fields are required.");
        return;
      }

      try
      {
        ResourceViewModel resource = new ResourceViewModel();

        resource.ResourceString = resourceStringTextBox.Text;
        resource.EnglishTranslation = englishTranslationTextBox.Text;
        resource.RomanianTranslation = romanianTranslationTextBox.Text;

        await resourceService.AddResource(resource);
        await resourceService.ExtractResourcesToLocalFolder(Properties.Settings.Default.ResourcesPath);

        componentOperation.ClearTextBoxes(this);
        MessageBox.Show("Added resource.");
        this.Hide();
      }
      catch(ResourceAlreadyExistsException ex)
      {
        MessageBox.Show(ex.Message);
      }
      catch(Exception)
      {
        MessageBox.Show("Something went wrong. Make sure that your Url and Framework folder path are set correctly.");
      }
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
