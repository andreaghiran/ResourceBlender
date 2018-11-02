using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Common.FileGeneration
{
  public class ResourceFileEntity
  {
    public ResourceFileEntity()
    {
      Lines = new List<ResourceFileEntityElement>();
    }

    public List<ResourceFileEntityElement> Lines { get; set; }
    public string LanguageCode { get; set; }
  }

  public class ResourceFileEntityElement
  {
    public string ResourceString { get; set; }
    public string TranslationString { get; set; }
  }
}
