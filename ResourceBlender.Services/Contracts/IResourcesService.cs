using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ResourceBlender.Services.Contracts
{
  public interface IResourcesService
  {
    Task<List<ResourceViewModel>> GetResources();
    ResourceViewModel GetResourceById(int id);
    Task AddResource(ResourceViewModel resource);
    Task UpdateResource(ResourceViewModel resource);
    Task DeleteResource(ResourceViewModel resource);

    void AddOrUpdateRomanianResourcesOnImport(HttpPostedFileBase resourceFile);
    void AddOrUpdateEnglishResourcesOnImport(HttpPostedFileBase resourceFile);

    Task ExtractResourcesToLocalFolder(string path);

    string BaseUri { get; set; }
  }
}
