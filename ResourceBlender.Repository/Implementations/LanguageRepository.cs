using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Repository.Implementations
{
  public class LanguageRepository : ILanguageRepository
  {
    private ResourceBlenderEntities _dbContext { get; set; }

    public LanguageRepository(ResourceBlenderEntities dbContext)
    {
      _dbContext = dbContext;
    }

    public List<Language> GetLanguages()
    {
      return _dbContext.Languages.ToList();
    }

    public Language GetLanguageByName(string name)
    {
      return _dbContext.Languages.FirstOrDefault(x => x.LanguageString.Equals(name));
    }

    public Language GetLanguageByCode(string code)
    {
      return _dbContext.Languages.FirstOrDefault(x => x.Code.Equals(code));
    }
  }
}
