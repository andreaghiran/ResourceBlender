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
    List<ResourceViewModel> GetResourceViewModelList();
    void AddResource(ResourceViewModel resource);
    ResourceViewModel GetResourceById(int id);
    void EditResource(ResourceViewModel resourceViewModel);
    void AddOrUpdateRomanianResourcesOnImport(HttpPostedFileBase resourceFile);
    void AddOrUpdateEnglishResourcesOnImport(HttpPostedFileBase resourceFile);
    List<Resource> GetAllResources();
    Task ExtractResourcesToLocalFolder(string path);
    Task SendAndAddResource(ResourceViewModel resource);
    Task SendAndUpdateResource(ResourceViewModel resource);
    Task SendAndDeleteResource(ResourceViewModel resource);
  }
}
