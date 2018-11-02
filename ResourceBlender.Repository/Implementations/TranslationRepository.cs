using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Repository.Implementations
{
  public class TranslationRepository : ITranslationRepository
  {
    private ResourceBlenderEntities _dbContext { get; set; }

    public TranslationRepository(ResourceBlenderEntities dbContext)
    {
      _dbContext = dbContext;
    }

    public List<Translation> GetTranslations()
    {
      return _dbContext.Translations.ToList();
    }

    public Translation GetTranslationById(int id)
    {
      return _dbContext.Translations.FirstOrDefault(x => x.Id == id);
    }

    public void UpdateTranslation(Translation updatedTranslation)
    {
      var translationToUpdate = GetTranslationById(updatedTranslation.Id);

      if(!translationToUpdate.TranslationString.Equals(updatedTranslation.TranslationString))
      {
        translationToUpdate.TranslationString = updatedTranslation.TranslationString;
        _dbContext.SaveChanges();
      }
    }
  }
}
