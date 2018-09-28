using ResourceBlender.WindowsForms.FormFactory.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace ResourceBlender.WindowsForms
{
  public partial class AddForm : Form
  {
    public AddForm()
    {
      InitializeComponent();
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
