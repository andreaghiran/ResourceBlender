using ResourceBlender.Common.FileGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Services.Contracts
{
  public interface IFileService
  {
    Task<MemoryStream> GetArchive();
    ResourceFileEntity GetResources(ZipArchiveEntry entry);
    //MemoryStream GetResourceFile(LanguageEnumeration language);
    //string GetJavascriptFile(Dictionary<string, string> resourceDictionary);
    //string GetJavaScriptFilePath(string resourcesPath);

    string BaseUri { get; set; }
  }
}
