using ResourceBlender.Common.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Services.Contracts
{
  public interface IFileService
  {
    MemoryStream GetArchive();
    MemoryStream GetResourceFile(LanguageEnumeration language);
    string GetJavascriptFile(Dictionary<string, string> resourceDictionary);
    string GetJavaScriptFilePath(string resourcesPath);
  }
}
