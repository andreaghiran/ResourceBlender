using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ResourceBlender.Services.Contracts
{
  public interface IResourcesService
  {
    Task<List<ResourceTranslationViewModel>> GetResources();
    ResourceTranslationViewModel GetResourceById(int id);
    Task AddResource(ResourceTranslationViewModel resource);
    Task UpdateResource(ResourceTranslationViewModel resource);
    Task DeleteResource(ResourceTranslationViewModel resource);

    Task<ResourceTranslationViewModel> GetNewResourceViewModel();
    //void AddOrUpdateRomanianResourcesOnImport(HttpPostedFileBase resourceFile);
    //void AddOrUpdateEnglishResourcesOnImport(HttpPostedFileBase resourceFile);

    //Task ExtractResourcesToLocalFolder(string path);

    void AddOrUpdateResourcesOnImport(ZipArchiveEntry entry);
    string BaseUri { get; set; }
  }
}
