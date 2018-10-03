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
    private readonly EditForm editForm;
    private readonly DeleteForm deleteForm;

    public MainForm(IResourcesService _resourcesService, AddForm _addForm, EditForm _editForm, DeleteForm _deleteForm)
    {
      InitializeComponent();
      resourcesService = _resourcesService;
      addForm = _addForm;
      editForm = _editForm;
      deleteForm = _deleteForm;
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      addForm.Show();
    }

    private void editButton_Click(object sender, EventArgs e)
    {
      editForm.Show();
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
      deleteForm.Show();
    }
  }
}
