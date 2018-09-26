using ResourceBlender.Common.Enums;
using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ResourceBlender.Repository.Implementations
{
  public class FileResourceRepository : IFileResourceRepository
  {
    public List<Resource> GetAllResources(HttpPostedFileBase resourceFile, LanguageEnumeration language)
    {
      List<Resource> resources = new List<Resource>();

      using (ResXResourceReader resxReader = new ResXResourceReader(resourceFile.InputStream))
      {
        if (language == LanguageEnumeration.English)
        {
          foreach (DictionaryEntry entry in resxReader)
          {
            Resource resource = new Resource
            {
              ResourceString = (string)entry.Key,
              EnglishTranslation = (string)entry.Value
            };
            resources.Add(resource);
          }
        }

        if (language == LanguageEnumeration.Romanian)
        {
          foreach (DictionaryEntry entry in resxReader)
          {
            Resource resource = new Resource
            {
              ResourceString = (string)entry.Key,
              RomanianTranslation = (string)entry.Value
            };
            resources.Add(resource);
          }
        }
      }
      return resources;
    }
  }
}
