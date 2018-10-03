using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Repository.Implementations
{
  public class ResourceRepository: IResourceRepository
  {
    private ResourceBlenderEntities _dbContext { get; set; }

    public ResourceRepository(ResourceBlenderEntities dbContext)
    {
      _dbContext = dbContext;
    }

    public List<Resource> GetAllResources()
    {
      return _dbContext.Resources.ToList();
    }

    public void AddResource(Resource resource)
    {
      _dbContext.Resources.Add(resource);
      _dbContext.SaveChanges();
    }

    public Resource GetResourceById(int id)
    {
      return _dbContext.Resources.Where(x => x.Id == id).FirstOrDefault();
    }

    public void EditResource(Resource resource)
    {
      Resource updatedResource = _dbContext.Resources.Where(x => x.Id == resource.Id).FirstOrDefault();

      if (updatedResource != null)
      {
        updatedResource.ResourceString = resource.ResourceString != string.Empty ? resource.ResourceString : updatedResource.ResourceString;
        updatedResource.RomanianTranslation = resource.RomanianTranslation != string.Empty ? resource.RomanianTranslation : updatedResource.RomanianTranslation;
        updatedResource.EnglishTranslation = resource.EnglishTranslation != string.Empty ? resource.EnglishTranslation : updatedResource.EnglishTranslation;
        _dbContext.SaveChanges();
      }
    }

    public void DeleteResource(int id)
    {
      var resource = _dbContext.Resources.Where(x => x.Id == id).FirstOrDefault();
      if(resource != null)
      {
        _dbContext.Resources.Remove(resource);
        _dbContext.SaveChanges();
      }
    }
  }
}
