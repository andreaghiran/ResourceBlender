using ResourceBlender.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Repository.Contracts
{
  public interface ILanguageRepository
  {
    List<Language> GetLanguages();
    Language GetLanguageByName(string name);
    Language GetLanguageByCode(string code);
  }
}
