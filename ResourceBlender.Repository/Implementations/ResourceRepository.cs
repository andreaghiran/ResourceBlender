using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Repository.Implementations
{
  public class ResourceRepository: IResourceRepository
  {
    private ResourceBlenderEntities _dbContext { get; set; }
    private ITranslationRepository translationRepository { get; set; }

    public ResourceRepository(ResourceBlenderEntities dbContext, ITranslationRepository _translationRepository)
    {
      _dbContext = dbContext;
      translationRepository = _translationRepository;
    }

    public List<Resource> GetResources()
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
      foreach (var translation in resource.Translations)
      {
        translationRepository.UpdateTranslation(translation);
      }
    }

    public void DeleteResource(int id)
    {
      var resource = _dbContext.Resources.Include(x => x.Translations).Where(x => x.Id == id).FirstOrDefault();
      if (resource != null)
      {
        _dbContext.Resources.Remove(resource);
        _dbContext.SaveChanges();
      }
    }

    public async Task<Resource> GetResourceByName(string name)
    {
      return await _dbContext.Resources.Where(x => x.ResourceString.Trim().ToLower().Equals(name.Trim().ToLower())).FirstOrDefaultAsync();
    }
  }
}
