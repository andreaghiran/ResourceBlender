using ResourceBlender.Common.Enums;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Resources;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ResourceBlender.Services.Implementations
{
  public class FileService: IFileService
  {
    private readonly IResourceRepository _resourceRepository;

    public FileService(IResourceRepository resourceRepository)
    {
      _resourceRepository = resourceRepository;
    }

    public MemoryStream GetArchive()
    {
      var memoryStream = new MemoryStream();
      using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
      {
        var zipArchiveEntry = archive.CreateEntry("Resources.ro.resx");
        using (var entryStream = zipArchiveEntry.Open())
        {
          GenerateResourceFile(entryStream, LanguageEnumeration.Romanian);
        }

        zipArchiveEntry = archive.CreateEntry("Resources.resx");
        using (var entryStream = zipArchiveEntry.Open())
        {
          GenerateResourceFile(entryStream, LanguageEnumeration.English);
        }
      }
      return memoryStream;
    }

    public MemoryStream GetResourceFile(LanguageEnumeration language)
    {
      var stream = new MemoryStream();
      ResXResourceWriter resourceFileWriter = new ResXResourceWriter(stream);
      resourceFileWriter = AddResourcesInChosenLanguage(resourceFileWriter, language);
      resourceFileWriter.Close();
      return stream;
    }

    public string GetJavascriptFile(Dictionary<string, string> resourceDictionary)
    {
      JavaScriptSerializer serializer = new JavaScriptSerializer();
      var json =  serializer.Serialize(resourceDictionary);
      var javaScript = "$.blogic.resources = " + json.Replace("\",\"", "\",\n\"") + ";";
      return (javaScript);
    }

    public string GetJavaScriptFilePath(string resourcesPath)
    {
      return Directory.GetParent(resourcesPath).ToString() + "\\BLogic.Web.Mvc\\Scripts\\blogic.resources.ro.js";
    }

    void GenerateResourceFile(Stream stream, LanguageEnumeration language)
    {
      ResXResourceWriter resourceFileWriter = new ResXResourceWriter(stream);
      resourceFileWriter = AddResourcesInChosenLanguage(resourceFileWriter, language);
      resourceFileWriter.Close();
    }

    private ResXResourceWriter AddResourcesInChosenLanguage(ResXResourceWriter writer, LanguageEnumeration language)
    {
      if (language == LanguageEnumeration.Romanian)
      {
        foreach (var resource in _resourceRepository.GetAllResources())
        {
          writer.AddResource(resource.ResourceString, resource.RomanianTranslation);
        }
      }
      if (language == LanguageEnumeration.English)
      {
        foreach (var resource in _resourceRepository.GetAllResources())
        {
          writer.AddResource(resource.ResourceString, resource.EnglishTranslation);
        }
      }
      return writer;
    }
  }
}