using Nelibur.ObjectMapper;
using Newtonsoft.Json;

using ResourceBlender.Common.Exceptions;
using ResourceBlender.Common.FileGeneration;
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
    private readonly ILanguageService _languageService;
    string _baseUri;

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public string BaseUri { get { return _baseUri; } set => _baseUri = value; }

    public int Resourcefileentity { get; private set; }

    public ResourcesService(IResourceRepository resourceRepository, ResourceBlenderEntities _context, 
                            IFileResourceRepository fileResourceRepository, IFileService fileService,
                            ILanguageService languageService)
    {
      _resourceRepository = resourceRepository;
      context = _context;
      _fileResourceRepository = fileResourceRepository;
      _httpClient = new HttpClient();
      _fileService = fileService;
      _languageService = languageService;
    }

    public ResourceTranslationViewModel GetResourceById(int id)
    {
      if (id == 0)
      {
        return null;
      }
      else
      {
        Resource resourceEntity = _resourceRepository.GetResourceById(id);
        ResourceTranslationViewModel viewModel = new ResourceTranslationViewModel();

        viewModel.ResourceId = id;
        viewModel.ResourceString = resourceEntity.ResourceString;

        viewModel.Translations = resourceEntity.Translations
                                                .Select(x => new ResourceTranslationViewModelElement
                                                                 {
                                                                   TranslationId = x.Id,
                                                                   TranslationValue = x.TranslationString,
                                                                   LanguageValue = x.Language.LanguageString,
                                                                 }).ToList();

        return viewModel;
      }
    }

    public async Task AddResource(ResourceTranslationViewModel resource)
    {
      var resourceToAdd = await FindResourceByName(resource.ResourceString);

      if (resourceToAdd != null)
      {
        throw new ResourceAlreadyExistsException("Resource already exists.");
      }

      var path = BaseUri + "api/Resources/AddResource";
      var jsonResource = JsonConvert.SerializeObject(resource);

      HttpResponseMessage response = await _httpClient.PostAsync(path, new StringContent(jsonResource, Encoding.UTF8, "application/json"));
    }

    public async Task UpdateResource(ResourceTranslationViewModel resource)
    {
      var path = BaseUri + "api/Resources/UpdateResource";

      var resourceToUpdate = await FindResourceByName(resource.ResourceString);

      if (resourceToUpdate == null)
      {
        throw new ResourceDoesNotExistException("Resource does not exist.");
      }

      var jsonResource = JsonConvert.SerializeObject(resource);
      HttpResponseMessage response = await _httpClient.PostAsync(path, new StringContent(jsonResource, Encoding.UTF8, "application/json"));
    }

    public async Task DeleteResource(ResourceTranslationViewModel resource)
    {
      var path = BaseUri + "api/Resources/DeleteResource?resourceId=";

      var resourceToDelete = await FindResourceByName(resource.ResourceString);
      if (resourceToDelete == null)
      {
        throw new ResourceDoesNotExistException("Resource does not exist.");
      }

      var jsonResource = JsonConvert.SerializeObject(resourceToDelete.Id);
      HttpResponseMessage response = await _httpClient.DeleteAsync(path + resourceToDelete.Id);
    }

    //  public void AddOrUpdateRomanianResourcesOnImport(HttpPostedFileBase resourceFile)
    //  {
    //    List<Resource> resources = _fileResourceRepository.GetAllResources(resourceFile, LanguageEnumeration.Romanian);

    //    try
    //    {
    //      context.Configuration.AutoDetectChangesEnabled = false;

    //      int count = 0;
    //      foreach (var entityToInsert in resources)
    //      {
    //        ++count;
    //        if (context.Resources.Any(x => x.ResourceString.Equals(entityToInsert.ResourceString)))
    //        {
    //          var updateContext = new ResourceBlenderEntities();
    //          var entityToUpdate = context.Resources.Where(x => x.ResourceString.Equals(entityToInsert.ResourceString)).FirstOrDefault();
    //          if (!entityToUpdate.RomanianTranslation.Equals(entityToInsert.RomanianTranslation))
    //          {
    //            entityToUpdate.RomanianTranslation = entityToInsert.RomanianTranslation;
    //            context.Entry(entityToUpdate).State = EntityState.Modified;
    //            context.Entry(entityToUpdate).Property(x => x.EnglishTranslation).IsModified = false;
    //            context.Entry(entityToUpdate).Property(x => x.ResourceString).IsModified = false;
    //            updateContext.SaveChanges();
    //          }
    //          continue;
    //        }
    //        context = AddToContext(context, entityToInsert, count, 1000, true);
    //      }

    //      context.SaveChanges();
    //    }
    //    finally
    //    {
    //      if (context != null)
    //        context.Dispose();
    //    }
    //  }

    //public void AddOrUpdateEnglishResourcesOnImport(HttpPostedFileBase resourceFile)
    //{
    //  List<Resource> resources = _fileResourceRepository.GetAllResources(resourceFile, LanguageEnumeration.English);

    //  using (context = new ResourceBlenderEntities())
    //  {
    //    try
    //    {
    //      context.Configuration.AutoDetectChangesEnabled = false;

    //      foreach (var oldResource in resources)
    //      {
    //        var entityToUpdate = context.Resources.Where(x => x.ResourceString.Equals(oldResource.ResourceString)).FirstOrDefault();

    //        if (entityToUpdate != null)
    //        {
    //          entityToUpdate.EnglishTranslation = oldResource.EnglishTranslation;
    //          context.Entry(entityToUpdate).State = EntityState.Modified;
    //          context.Entry(entityToUpdate).Property(x => x.RomanianTranslation).IsModified = false;
    //          context.Entry(entityToUpdate).Property(x => x.ResourceString).IsModified = false;
    //          context.SaveChanges();
    //        }
    //      }
    //    }
    //    finally
    //    {
    //      context.Configuration.AutoDetectChangesEnabled = true;
    //    }
    //  }
    //}

    //  public async Task ExtractResourcesToLocalFolder(string localResourcesPath)
    //  {
    //    var path = BaseUri + "api/Resources/GetZip";
    //    HttpResponseMessage response = await _httpClient.GetAsync(path);

    //    var jsonMessage = await response.Content.ReadAsByteArrayAsync();
    //    MemoryStream memoryStream = new MemoryStream(jsonMessage);
    //    ZipArchive zipArchive = new ZipArchive(memoryStream);

    //    foreach (ZipArchiveEntry file in zipArchive.Entries)
    //    {
    //      string completeFileName = Path.Combine(localResourcesPath, file.FullName);
    //      file.ExtractToFile(completeFileName, true);
    //    }
    //    await GenerateJavascriptResources(localResourcesPath);
    //  }

    //  private  async Task GenerateJavascriptResources(string localResourcesPath)
    //  {
    //    var resourcesDictionary = await GetResourcesDictionary();
    //    var content = _fileService.GetJavascriptFile(resourcesDictionary);

    //    try
    //    {
    //      var path = _fileService.GetJavaScriptFilePath(localResourcesPath);
    //      using (var writer = System.IO.File.CreateText(path))
    //      {
    //        writer.Write(content);
    //        writer.Flush();
    //      }
    //    }
    //    catch(Exception ex)
    //    {

    //    }
    //  }

    //  async  Task<Dictionary<string, string>> GetResourcesDictionary()
    //  {
    //    var resources = await GetResources();
    //    var resourceDictionary = new Dictionary<string, string>();

    //    foreach (var resource in resources)
    //    {
    //      resourceDictionary[resource.ResourceString.ToLower()] = resource.RomanianTranslation;
    //    }

    //    return resourceDictionary;
    //  }

    //  public async Task<bool> CheckIfResourceWithNameExists(string resourceName)
    //  {
    //    var resource = await FindResourceByName(resourceName);
    //    bool exists = resource != null ? true : false;
    //    return exists;
    //  }

    private async Task<Resource> FindResourceByName(string resourceName)
    {
      var queryString = resourceName;
      var path = BaseUri + "api/Resources/FindResourceByName?resourceName=" + resourceName;

      HttpResponseMessage result = await _httpClient.GetAsync(path);

      var jsonResource = await result.Content.ReadAsStringAsync();

      var resource = JsonConvert.DeserializeObject<Resource>(jsonResource);

      return resource != null ? resource : null;
    }

    public async Task<List<ResourceTranslationViewModel>> GetResources()
    {
      var path = BaseUri + "api/Translations/GetTranslations";

      var response = await _httpClient.GetAsync(path);

      var jsonTranslations = await response.Content.ReadAsStringAsync();

      //log.Info(jsonResources);

      if (string.IsNullOrEmpty(jsonTranslations))
      {
        return new List<ResourceTranslationViewModel>();
      }

      var translationList = JsonConvert.DeserializeObject<List<Translation>>(jsonTranslations);

      if (translationList != null)
      {
        var resourcesList = translationList.GroupBy(x => x.Resource).Select(x => new { ResourceId =  x.Key.Id, ResourceString =  x.Key.ResourceString }).Distinct().ToList();
        var languagesList = translationList.GroupBy(x => x.Language.LanguageString).Select(x => x.Key).ToList();
        languagesList.Sort();

        List<ResourceTranslationViewModel> viewModelList = new List<ResourceTranslationViewModel>();

        response = await _httpClient.GetAsync(path);
        
        foreach (var resource in resourcesList)
        {
          ResourceTranslationViewModel viewModel = new ResourceTranslationViewModel();
          viewModel.ResourceString = resource.ResourceString;
          viewModel.ResourceId = resource.ResourceId;

          List<ResourceTranslationViewModelElement> viewModelTranslations = translationList
                                                                                .Where(x => x.Resource.ResourceString.Equals(resource.ResourceString))
                                                                                .Select(x => new ResourceTranslationViewModelElement
                                                                                                 {
                                                                                                  TranslationId = x.Id,
                                                                                                  TranslationValue = x.TranslationString,
                                                                                                  LanguageValue = x.Language.LanguageString
                                                                                                  }).ToList();

          viewModel.Translations = viewModelTranslations.OrderBy(x => x.LanguageValue).ToList();
          viewModel.AvailableLanguages = languagesList;

          viewModelList.Add(viewModel);
        }

        return viewModelList;
      }

      return null;
    }

    public async Task<ResourceTranslationViewModel> GetNewResourceViewModel()
    {
      ResourceTranslationViewModel viewModel = new ResourceTranslationViewModel();

      _languageService.BaseUri = this.BaseUri;
      viewModel.AvailableLanguages = await _languageService.GetLanguageValues();

      foreach (var language in viewModel.AvailableLanguages)
      {
        viewModel.Translations.Add(new ResourceTranslationViewModelElement());
      }

      return viewModel;
    }

    public async Task AddOrUpdateResourcesOnImport(ZipArchiveEntry entry)
    {
      ResourceFileEntity resourceFileEntity = _fileService.GetResources(entry);

      //try
      //{
      //  context.Configuration.AutoDetectChangesEnabled = false;

      //  int count = 0;
      //  foreach (var entityToInsert in resources)
      //  {
      //    ++count;
      //    if (context.Resources.Any(x => x.ResourceString.Equals(entityToInsert.ResourceString)))
      //    {
      //      var updateContext = new ResourceBlenderEntities();
      //      var entityToUpdate = context.Resources.Where(x => x.ResourceString.Equals(entityToInsert.ResourceString)).FirstOrDefault();
      //      if (!entityToUpdate.RomanianTranslation.Equals(entityToInsert.RomanianTranslation))
      //      {
      //        entityToUpdate.RomanianTranslation = entityToInsert.RomanianTranslation;
      //        context.Entry(entityToUpdate).State = EntityState.Modified;
      //        context.Entry(entityToUpdate).Property(x => x.EnglishTranslation).IsModified = false;
      //        context.Entry(entityToUpdate).Property(x => x.ResourceString).IsModified = false;
      //        updateContext.SaveChanges();
      //      }
      //      continue;
      //    }
      //    context = AddToContext(context, entityToInsert, count, 1000, true);
      //  }

      //  context.SaveChanges();
      //}
      //finally
      //{
      //  if (context != null)
      //    context.Dispose();
      //}

      try
      {
        context.Configuration.AutoDetectChangesEnabled = false;
        int count = 0;

        _languageService.BaseUri = BaseUri;
        var languageId = await _languageService.GetLanguageIdByCode(resourceFileEntity.LanguageCode);

        foreach (var resourceFileEntityLine in resourceFileEntity.Lines)
        {
          ++count;

          if(context.Resources.Any(x => x.ResourceString.Equals(resourceFileEntityLine.ResourceString)))
          {
            var updateContext = new ResourceBlenderEntities();
            var entityToUpdate = context.Resources.Where(x => x.ResourceString.Equals(resourceFileEntityLine.ResourceString)).FirstOrDefault();

            if (!entityToUpdate.Translations.Any().Equals(resourceFileEntityLine.TranslationString))
            {
              entityToUpdate.Translations.Where(x => x.Language_Id == languageId).First().TranslationString = resourceFileEntityLine.TranslationString;
              context.Entry(entityToUpdate).State = EntityState.Modified;
              context.Entry(entityToUpdate).Property(x => x.ResourceString).IsModified = false;
              updateContext.SaveChanges();
            }
            continue;
          }

          context = AddToContext(context, resourceFileEntityLine, count, 1000, true);
        }
        context.SaveChanges();
      }
      finally
      {

      }
    }

    ResourceBlenderEntities AddToContext(ResourceBlenderEntities context, ResourceFileEntity entityToInsert, int count, int commitCount, bool recreateContext)
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

    //  List<Resource> GetResourcesFromFile(HttpPostedFileBase resourceFile, LanguageEnumeration language)
    //  {
    //    List<Resource> resources = new List<Resource>();

    //    using (ResXResourceReader resxReader = new ResXResourceReader(resourceFile.InputStream))
    //    {
    //      if (language == LanguageEnumeration.English)
    //      {
    //        foreach (DictionaryEntry entry in resxReader)
    //        {
    //          Resource resource = new Resource
    //          {
    //            ResourceString = (string)entry.Key,
    //            EnglishTranslation = (string)entry.Value
    //          };
    //          resources.Add(resource);
    //        }
    //      }

    //      if (language == LanguageEnumeration.Romanian)
    //      {
    //        foreach (DictionaryEntry entry in resxReader)
    //        {
    //          Resource resource = new Resource
    //          {
    //            ResourceString = (string)entry.Key,
    //            RomanianTranslation = (string)entry.Value
    //          };
    //          resources.Add(resource);
    //        }
    //      }
    //    }
    //    return resources;
    //  }
  }
}

