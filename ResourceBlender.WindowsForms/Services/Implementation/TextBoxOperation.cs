using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms.Services.Implementation
{
  public class TextBoxOperation : ITextBoxOperation
  {
    public void ClearTextBoxes(Control control)
    {
      foreach (var c in control.Controls)
      {
        if (c is TextBox) ((TextBox)c).Text = String.Empty;
      }
    }
  }
}
