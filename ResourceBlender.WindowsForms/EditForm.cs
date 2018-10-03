using ResourceBlender.Common.ViewModels;
using ResourceBlender.Services.Contracts;
using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms
{
  public partial class EditForm : Form
  {
    private string resourceFolderPath;
    private readonly IComponentOperation componentOperation;
    private readonly IResourcesService resourcesService;
    private HttpClient httpClient;

    public EditForm(IResourcesService _resourcesService, IComponentOperation _componentOperation)
    {
      InitializeComponent();
      componentOperation = _componentOperation;
      resourcesService = _resourcesService;
      httpClient = new HttpClient();
    }

    private void chooseResourceFolderButton_Click(object sender, EventArgs e)
    {
      resourceFolderPath = componentOperation.SetResourceFolderPath();
    }

    private async void updateAndGenerateButton_Click(object sender, EventArgs e)
    {
      ResourceViewModel resource = new ResourceViewModel();

      resource.ResourceString = resourceTextBox.Text;
      resource.EnglishTranslation = englishTranslationTextBox.Text;
      resource.RomanianTranslation = romanianTranslationTextBox.Text;

      await resourcesService.SendAndUpdateResource(resource);
      await resourcesService.ExtractResourcesToLocalFolder(resourceFolderPath);

      componentOperation.ClearTextBoxes(this);
      this.Hide();
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      componentOperation.ClearTextBoxes(this);
      this.Hide();
    }
  }
}
