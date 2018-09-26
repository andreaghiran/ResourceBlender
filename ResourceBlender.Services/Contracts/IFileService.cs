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
    void GenerateExportFile(Stream stream, LanguageEnumeration language);
  }
}
