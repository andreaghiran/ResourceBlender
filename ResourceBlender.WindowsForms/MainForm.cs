using ResourceBlender.Services.Contracts;
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
            resourcesService.BaseUri = Properties.Settings.Default.BaseUri;
            addForm = _addForm;
            editForm = _editForm;
            deleteForm = _deleteForm;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtBoxUrl.Text = Properties.Settings.Default.BaseUri;
            txtFMF.Text = Properties.Settings.Default.ResourcesPath;
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

        private void btnSetUrl_Click(object sender, EventArgs e)
        {
            var url = txtBoxUrl.Text;

            Properties.Settings.Default.BaseUri = url;
            Properties.Settings.Default.Save();
        }

        private void btnSetFWFOlder_Click(object sender, EventArgs e)
        {
            var resourcesPath = txtFMF.Text;

            Properties.Settings.Default.ResourcesPath = resourcesPath;
            Properties.Settings.Default.Save();
        }

        private async void generateLatestBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await resourcesService.ExtractResourcesToLocalFolder(Properties.Settings.Default.ResourcesPath);
                await resourcesService.GenerateJavascriptResources(Properties.Settings.Default.ResourcesPath);
                MessageBox.Show("Generated resources");
            }
            catch(Exception)
            {
                MessageBox.Show("Something went wrong. Check to see if your framework folder and base path are correctly set.");
            }
        }
    }
}
