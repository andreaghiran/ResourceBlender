using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceBlender.WindowsForms.Services.Interfaces
{
  public interface IComponentOperation
  {
    void ClearTextBoxes(Control control);
    string SetResourceFolderPath();
    string GetValidationMessage(Form form, )
  }
}
