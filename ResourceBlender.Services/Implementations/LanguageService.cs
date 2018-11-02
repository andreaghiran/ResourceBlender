using Newtonsoft.Json;
using ResourceBlender.Domain;
using ResourceBlender.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Services.Implementations
{
  public class LanguageService : ILanguageService
  {
    private readonly HttpClient _httpClient;
    string _baseUri;
    public string BaseUri { get { return _baseUri; } set => _baseUri = value; }

    public LanguageService()
    {
      _httpClient = new HttpClient();
    }

    public async Task<int> GetLanguageId(string languageValue)
    {
      var path = BaseUri + "api/Languages/GetLanguageByName?name=" + languageValue;
      HttpResponseMessage result = await _httpClient.GetAsync(path);
      string jsonLanguage = await result.Content.ReadAsStringAsync();

      Language language = JsonConvert.DeserializeObject<Language>(jsonLanguage);

      return language != null ? language.Id : 0;
    }

    public async Task<int> GetLanguageIdByCode(string languageCode)
    {
      var path = BaseUri + "api/Languages/GetLanguageByCode?code=" + languageCode;
      HttpResponseMessage result = await _httpClient.GetAsync(path);
      string jsonLanguage = await result.Content.ReadAsStringAsync();

      Language language = JsonConvert.DeserializeObject<Language>(jsonLanguage);

      return language != null ? language.Id : 0;
    }

    public async Task<List<string>> GetLanguageValues()
    {
      List<string> languageValues = new List<string>();

      var path = BaseUri + "api/Languages/GetLanguages";
      HttpResponseMessage result = await _httpClient.GetAsync(path);
      string jsonLanguages = await result.Content.ReadAsStringAsync();

      List<Language> languages = JsonConvert.DeserializeObject<List<Language>>(jsonLanguages);

      if(languages.Count > 0)
      {
        foreach (var language in languages)
        {
          languageValues.Add(language.LanguageString);
        }

        return languageValues;
      }

      return languageValues;
    }

    public async Task<List<Language>> GetLanguages()
    {
      var path = BaseUri + "api/Languages/GetLanguages";
      HttpResponseMessage result = await _httpClient.GetAsync(path);
      string jsonLanguages = await result.Content.ReadAsStringAsync();

      List<Language> languages = JsonConvert.DeserializeObject<List<Language>>(jsonLanguages);

      return languages;
    }
  }
}
