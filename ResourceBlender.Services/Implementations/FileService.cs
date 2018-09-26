using ResourceBlender.Common.Enums;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System.IO;
using System.Resources;

namespace ResourceBlender.Services.Implementations
{
  public class FileService: IFileService
  {
    private readonly IResourceRepository _resourceRepository;

    public FileService(IResourceRepository resourceRepository)
    {
      _resourceRepository = resourceRepository;
    }

    public void GenerateExportFile(Stream stream, LanguageEnumeration language)
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