using ResourceBlender.Common.ViewModels;
using ResourceBlender.Services.Contracts;
using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms
{
  public partial class DeleteForm : Form
  {
    private string resourceFolderPath;
    private readonly IComponentOperation componentOperation;
    private readonly IResourcesService resourcesService;
    private HttpClient httpClient;

    public DeleteForm(IResourcesService _resourcesService, IComponentOperation _componentOperation)
    {
      InitializeComponent();
      componentOperation = _componentOperation;
      resourcesService = _resourcesService;
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
      ResourceViewModel resource = new ResourceViewModel();
      resource.ResourceString = resourceTextBox.Text;

      await resourcesService.SendAndDeleteResource(resource);
      await resourcesService.ExtractResourcesToLocalFolder(resourceFolderPath);

      componentOperation.ClearTextBoxes(this);
      this.Hide();
    }
  }
}
