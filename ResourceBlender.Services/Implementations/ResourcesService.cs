using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using ResourceBlender.Common.Enums;
using ResourceBlender.Common.Exceptions;
using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace ResourceBlender.Services.Implementations
{
  public class ResourcesService : IResourcesService
  {
    private IResourceRepository _resourceRepository;
    private ResourceBlenderEntities context;
    private IFileResourceRepository _fileResourceRepository;
    private readonly HttpClient _httpClient;
    private readonly IFileService _fileService;
    string _baseUri;

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public string BaseUri { get { return _baseUri; } set => _baseUri = value; }


    public ResourcesService(IResourceRepository resourceRepository, ResourceBlenderEntities _context, IFileResourceRepository fileResourceRepository, IFileService fileService)
    {
      _resourceRepository = resourceRepository;
      context = _context;
      _fileResourceRepository = fileResourceRepository;
      _httpClient = new HttpClient();
      _fileService = fileService;
    }

    public List<ResourceViewModel> GetResourceViewModelList()
    {
      List<ResourceViewModel> viewModelList = _resourceRepository.GetAllResources()
                                             .Select(x =>
                                             new ResourceViewModel
                                             {
                                               Id = x.Id,
                                               ResourceString = x.ResourceString,
                                               EnglishTranslation = x.EnglishTranslation,
                                               RomanianTranslation = x.RomanianTranslation
                                             }).ToList();
      return viewModelList;
    }

    public async  Task AddResource(ResourceViewModel resourceViewModel)
    {
      var resourceExists = await CheckIfResourceWithNameExists(resourceViewModel.ResourceString);
      if(resourceExists)
      {
        throw new ResourceAlreadyExistsException("This resource already exists");
      }
      var resourceEntity = TinyMapper.Map<Resource>(resourceViewModel);
      _resourceRepository.AddResource(resourceEntity);
    }

    public ResourceViewModel GetResourceById(int id)
    {
      if (id == 0)
      {
        return null;
      }
      else
      {
        Resource resourceEntity = _resourceRepository.GetResourceById(id);
        ResourceViewModel resourceViewModel = TinyMapper.Map<ResourceViewModel>(resourceEntity);

        return resourceViewModel;
      }
    }

    public void EditResource(ResourceViewModel resourceViewModel)
    {
      Resource resourceEntity = TinyMapper.Map<Resource>(resourceViewModel);
      _resourceRepository.EditResource(resourceEntity);
    }

    public void AddOrUpdateRomanianResourcesOnImport(HttpPostedFileBase resourceFile)
    {
      List<Resource> resources = _fileResourceRepository.GetAllResources(resourceFile, LanguageEnumeration.Romanian);

      try
      {
        context.Configuration.AutoDetectChangesEnabled = false;

        int count = 0;
        foreach (var entityToInsert in resources)
        {
          ++count;
          if (context.Resources.Any(x => x.ResourceString.Equals(entityToInsert.ResourceString)))
          {
            var updateContext = new ResourceBlenderEntities();
            var entityToUpdate = context.Resources.Where(x => x.ResourceString.Equals(entityToInsert.ResourceString)).FirstOrDefault();
            if (!entityToUpdate.RomanianTranslation.Equals(entityToInsert.RomanianTranslation))
            {
              entityToUpdate.RomanianTranslation = entityToInsert.RomanianTranslation;
              context.Entry(entityToUpdate).State = EntityState.Modified;
              context.Entry(entityToUpdate).Property(x => x.EnglishTranslation).IsModified = false;
              context.Entry(entityToUpdate).Property(x => x.ResourceString).IsModified = false;
              updateContext.SaveChanges();
            }
            continue;
          }
          context = AddToContext(context, entityToInsert, count, 1000, true);
        }

        context.SaveChanges();
      }
      finally
      {
        if (context != null)
          context.Dispose();
      }
    }

    public void AddOrUpdateEnglishResourcesOnImport(HttpPostedFileBase resourceFile)
    {
      List<Resource> resources = _fileResourceRepository.GetAllResources(resourceFile, LanguageEnumeration.English);

      using (context = new ResourceBlenderEntities())
      {
        try
        {
          context.Configuration.AutoDetectChangesEnabled = false;

          foreach (var oldResource in resources)
          {
            var entityToUpdate = context.Resources.Where(x => x.ResourceString.Equals(oldResource.ResourceString)).FirstOrDefault();

            if (entityToUpdate != null)
            {
              entityToUpdate.EnglishTranslation = oldResource.EnglishTranslation;
              context.Entry(entityToUpdate).State = EntityState.Modified;
              context.Entry(entityToUpdate).Property(x => x.RomanianTranslation).IsModified = false;
              context.Entry(entityToUpdate).Property(x => x.ResourceString).IsModified = false;
              context.SaveChanges();
            }
          }
        }
        finally
        {
          context.Configuration.AutoDetectChangesEnabled = true;
        }
      }
    }

    public List<Resource> GetAllResources()
    {
      return _resourceRepository.GetAllResources().ToList();
    }

    public async Task ExtractResourcesToLocalFolder(string localResourcesPath)
    {
      var path = BaseUri + "/api/Resources/GetZip";
      HttpResponseMessage response = await _httpClient.GetAsync(path);

      var jsonMessage = await response.Content.ReadAsByteArrayAsync();
      MemoryStream memoryStream = new MemoryStream(jsonMessage);
      ZipArchive zipArchive = new ZipArchive(memoryStream);

      foreach (ZipArchiveEntry file in zipArchive.Entries)
      {
        string completeFileName = Path.Combine(localResourcesPath, file.FullName);
        file.ExtractToFile(completeFileName, true);
      }
    }

    public async Task GenerateJavascriptResources(string localResourcesPath)
    {
      var resourcesDictionary = await GetResourcesDictionary();
      var content = _fileService.GetJavascriptFile(resourcesDictionary);

      try
      {
        var path = _fileService.GetJavaScriptFilePath(localResourcesPath);
        using (var writer = System.IO.File.CreateText(path))
        {
          writer.Write(content);
          writer.Flush();
        }
      }
      catch(Exception ex)
      {

      }
    }

    async  Task<Dictionary<string, string>> GetResourcesDictionary()
    {
      var resources = await GetResourceViewModelListTask();
      var resourceDictionary = new Dictionary<string, string>();

      foreach (var resource in resources)
      {
        resourceDictionary[resource.ResourceString.ToLower()] = resource.RomanianTranslation;
      }

      return resourceDictionary;
    }

    public async Task SendAndAddResource(ResourceViewModel resource)
    {
      var path = BaseUri + "/api/Resources/AddResource";
      var jsonResource = JsonConvert.SerializeObject(resource);

      HttpResponseMessage response = await _httpClient.PostAsync(path, new StringContent(jsonResource, Encoding.UTF8, "application/json"));
    }

    public async Task SendAndUpdateResource(ResourceViewModel resource)
    {
      var path = BaseUri + "/api/Resources/UpdateResource";

      var resourceToUpdate = await FindResourceByName(resource.ResourceString);
      resource.Id = resourceToUpdate.Id;

      var jsonResource = JsonConvert.SerializeObject(resource);
      HttpResponseMessage response = await _httpClient.PostAsync(path, new StringContent(jsonResource, Encoding.UTF8, "application/json"));
    }

    public async Task SendAndDeleteResource(ResourceViewModel resource)
    {
      var path = BaseUri + "/api/Resources/DeleteResource?resourceId=";

      var resourceToDelete = await FindResourceByName(resource.ResourceString);

      var jsonResource = JsonConvert.SerializeObject(resourceToDelete.Id);
      HttpResponseMessage response = await _httpClient.DeleteAsync(path + resourceToDelete.Id);
    }

    public async Task<bool> CheckIfResourceWithNameExists(string resourceName)
    {
      var resource = await FindResourceByName(resourceName);
      bool exists = resource != null ? true : false;
      return exists;
    }

    public async Task<Resource> FindResourceByName(string resourceName)
    {
      var queryString = resourceName;
      var path = BaseUri + "/api/Resources/FindResourceByName?resourceName=" + resourceName;

      HttpResponseMessage result = await _httpClient.GetAsync(path);

      var jsonResource= await result.Content.ReadAsStringAsync();

      var resource = JsonConvert.DeserializeObject<Resource>(jsonResource);

      return resource != null ? resource : null; 
    }

    public async Task<List<ResourceViewModel>> GetResourceViewModelListTask()
    {
      var path = BaseUri + "/api/Resources/GetAllResources";

      var response = await _httpClient.GetAsync(path);
      
      var jsonResources = await response.Content.ReadAsStringAsync();

      log.Info(jsonResources);

      if (string.IsNullOrEmpty(jsonResources))
      {
        return new List<ResourceViewModel>();
      }

      var resourceList = JsonConvert.DeserializeObject<List<Resource>>(jsonResources);

      if (resourceList != null)
      {
        var viewModelList = resourceList.Select(x =>
                                             new ResourceViewModel
                                             {
                                               Id = x.Id,
                                               ResourceString = x.ResourceString,
                                               EnglishTranslation = x.EnglishTranslation,
                                               RomanianTranslation = x.RomanianTranslation
                                             }).ToList();

        return viewModelList;
      }

      return null;
    }

    ResourceBlenderEntities AddToContext(ResourceBlenderEntities context, Resource entityToInsert, int count, int commitCount, bool recreateContext)
    {
      context.Set<Resource>().Add(entityToInsert);

      if (count % commitCount == 0)
      {
        context.SaveChanges();
        if (recreateContext)
        {
          context.Dispose();
          context = new ResourceBlenderEntities();
          context.Configuration.AutoDetectChangesEnabled = false;
        }
      }

      return context;
    }

    List<Resource> GetResourcesFromFile(HttpPostedFileBase resourceFile, LanguageEnumeration language)
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

