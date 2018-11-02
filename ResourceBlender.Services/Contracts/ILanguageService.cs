using ResourceBlender.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Services.Contracts
{
  public interface ILanguageService
  {
    Task<int> GetLanguageId(string languageValue);
    Task<int> GetLanguageIdByCode(string languageCode);
    Task<List<string>> GetLanguageValues();
    Task<List<Language>> GetLanguages();
    string BaseUri { get; set; }
  }
}
