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


        private async void deleteAndGenerateButton_Click(object sender, EventArgs e)
        {
            var isFormValid = ValidateChildren();

            if (!isFormValid)
            {
                MessageBox.Show("The resource field is required.");
                return;
            }

            isFormValid = await resourcesService.CheckIfResourceWithNameExists(resourceTextBox.Text);
            if (!isFormValid)
            {
                MessageBox.Show("The resource could not be found.");
                return;
            }

            try
            {
                ResourceViewModel resource = new ResourceViewModel();
                resource.ResourceString = resourceTextBox.Text;

                resourcesService.BaseUri = Properties.Settings.Default.BaseUri;

                await resourcesService.SendAndDeleteResource(resource);
                await resourcesService.ExtractResourcesToLocalFolder(Properties.Settings.Default.ResourcesPath);
                await resourcesService.GenerateJavascriptResources(Properties.Settings.Default.ResourcesPath);

                componentOperation.ClearTextBoxes(this);
                MessageBox.Show("Deleted resource.");
                this.Hide();
            }
            catch(Exception)
            {
                MessageBox.Show("Something went wrong. Check to see if your framework folder and base path are correctly set.");
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

        private void DeleteForm_Load(object sender, EventArgs e)
        {
            this.AutoValidate = AutoValidate.Disable;
        }
    }
}
