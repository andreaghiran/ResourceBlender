using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms.Services.Implementation
{
  public class ComponentOperation : IComponentOperation
  {
    public void ClearTextBoxes(Control control)
    {
      foreach (var c in control.Controls)
      {
        if (c is TextBox) ((TextBox)c).Text = String.Empty;
      }
    }

    public string SetResourceFolderPath()
    {
      FolderBrowserDialog resourcesFolderDialog = new FolderBrowserDialog();
      resourcesFolderDialog.ShowNewFolderButton = true;
      DialogResult result = resourcesFolderDialog.ShowDialog();
      if (result == DialogResult.OK)
      {
        return resourcesFolderDialog.SelectedPath;
      }
      else
      {
        return string.Empty;
      }
    }

    public bool ValidateTextBoxes(Control control)
    {
      foreach (var c in control.Controls)
      {
        if (c is TextBox && ((TextBox)c).Text.Equals(string.Empty))
        {
          return false;
        }
      }
      return true;
    }
  }
}
