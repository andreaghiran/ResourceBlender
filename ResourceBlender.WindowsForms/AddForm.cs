using ResourceBlender.Services.Contracts;
using System;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms
{
  public partial class AddForm : Form
  {
    private readonly IResourcesService resourceService;
    //private readonly MainForm mainForm;

    public AddForm(IResourcesService _resourceService/*, MainForm _mainForm*/)
    {
      InitializeComponent();
      resourceService = _resourceService;
      //mainForm = _mainForm;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
