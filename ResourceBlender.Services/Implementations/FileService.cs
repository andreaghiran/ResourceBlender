using ResourceBlender.Common.FileGeneration;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ResourceBlender.Services.Implementations
{
  public class FileService: IFileService
  {
    private readonly IResourceRepository _resourceRepository;
    private readonly ILanguageService _languageService;

    string _baseUri;
    public string BaseUri { get { return _baseUri; } set => _baseUri = value; }

    public FileService(IResourceRepository resourceRepository, ILanguageService languageService)
    {
      _resourceRepository = resourceRepository;
      _languageService = languageService;
    }

    public async Task<MemoryStream> GetArchive()
    {
      var memoryStream = new MemoryStream();
      using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
      {
        _languageService.BaseUri = BaseUri;
        var languages = await _languageService.GetLanguages();
        ZipArchiveEntry zipArchiveEntry;

        foreach (var language in languages)
        {
          if (language.Code.Equals("en"))
          {
            zipArchiveEntry = archive.CreateEntry("Resources.resx");
          }
          else
          {
            zipArchiveEntry = archive.CreateEntry("Resources." + language.Code + ".resx");
          }
          using (var entryStream = zipArchiveEntry.Open())
          {
            GenerateResourceFile(entryStream, language.LanguageString);
          }
        }
      }
      return memoryStream;
    }

    public ResourceFileEntity GetResources(ZipArchiveEntry entry)
    {
      ResourceFileEntity resourceFileEntity = new ResourceFileEntity();

      using (ResXResourceReader resxReader = new ResXResourceReader(entry.Open()))
      {
        resourceFileEntity.LanguageCode = ResourceFileHelper.GetLanguageCode(entry.Name);

        foreach (DictionaryEntry resource in resxReader)
        {
          resourceFileEntity.Lines.Add(new ResourceFileEntityElement { ResourceString = (string)resource.Key, TranslationString = (string)resource.Value });
        }
      }

      return resourceFileEntity;
    }

    //public MemoryStream GetResourceFile(LanguageEnumeration language)
    //{
    //  var stream = new MemoryStream();
    //  ResXResourceWriter resourceFileWriter = new ResXResourceWriter(stream);
    //  resourceFileWriter = AddResourcesInChosenLanguage(resourceFileWriter, language);
    //  resourceFileWriter.Close();
    //  return stream;
    //}

    //public string GetJavascriptFile(Dictionary<string, string> resourceDictionary)
    //{
    //  JavaScriptSerializer serializer = new JavaScriptSerializer();
    //  var json =  serializer.Serialize(resourceDictionary);
    //  var javaScript = "$.blogic.resources = " + json.Replace("\",\"", "\",\n\"") + ";";
    //  return (javaScript);
    //}

    //public string GetJavaScriptFilePath(string resourcesPath)
    //{
    //  return Directory.GetParent(resourcesPath).ToString() + "\\BLogic.Web.Mvc\\Scripts\\blogic.resources.ro.js";
    //}

    void GenerateResourceFile(Stream stream, string language)
    {
      ResXResourceWriter resourceFileWriter = new ResXResourceWriter(stream);
      resourceFileWriter = AddResourcesInChosenLanguage(resourceFileWriter, language);
      resourceFileWriter.Close();
    }

    private ResXResourceWriter AddResourcesInChosenLanguage(ResXResourceWriter writer, string language)
    {
      foreach (var resource in _resourceRepository.GetResources())
      {
        var resourceString = resource.ResourceString;
        var translation = resource.Translations.FirstOrDefault(x => x.Language.LanguageString.Equals(language)).TranslationString;
        writer.AddResource(resourceString, translation);
      }

      return writer;
    }
  }
}