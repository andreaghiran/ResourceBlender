using ResourceBlender.Services.Contracts;
using System;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms
{
  public partial class MainForm : Form
  {
    private readonly IResourcesService resourcesService;

    public MainForm(IResourcesService _resourcesService)
    {
      InitializeComponent();
      resourcesService = _resourcesService;
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
