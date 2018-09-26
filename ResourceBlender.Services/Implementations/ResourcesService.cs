using Nelibur.ObjectMapper;
using ResourceBlender.Common.Enums;
using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Resources;
using System.Web;

namespace ResourceBlender.Services.Implementations
{
  public class ResourcesService : IResourcesService
  {
    private IResourceRepository _resourceRepository;
    private ResourceBlenderEntities context;
    private IFileResourceRepository _fileResourceRepository;

    public ResourcesService(IResourceRepository resourceRepository, ResourceBlenderEntities _context, IFileResourceRepository fileResourceRepository)
    {
      _resourceRepository = resourceRepository;
      context = _context;
      _fileResourceRepository = fileResourceRepository;
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

    public void AddResource(ResourceViewModel resourceViewModel)
    {
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
