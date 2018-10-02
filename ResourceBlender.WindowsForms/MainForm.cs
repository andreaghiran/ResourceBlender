using ResourceBlender.Services.Contracts;
using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms
{
  public partial class MainForm : Form
  {
    private readonly IResourcesService resourcesService;
    private readonly AddForm addForm;

    public MainForm(IResourcesService _resourcesService, AddForm _addForm)
    {
      InitializeComponent();
      resourcesService = _resourcesService;
      addForm = _addForm;
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      addForm.Show();
      //this.Close();
    }
  }
}
