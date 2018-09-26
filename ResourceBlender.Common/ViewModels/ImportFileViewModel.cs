using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ResourceBlender.Common.ViewModels
{
  public class ImportFileViewModel
  {
    public ImportFileViewModel()
    {
      files = new List<HttpPostedFileBase>();
    }

    public List<HttpPostedFileBase> files { get; set; }
  }
}
