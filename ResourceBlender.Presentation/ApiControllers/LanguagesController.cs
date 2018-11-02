using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ResourceBlender.Presentation.ApiControllers
{
  public class LanguagesController : ApiController
  {
    private readonly IFileService fileService;
    private readonly IResourceRepository resourceRepository;
    private readonly IResourcesService resourcesService;
    private readonly ILanguageRepository languageRepository;
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public LanguagesController(IFileService _fileService, IResourceRepository _resourceRepository, IResourcesService _resourcesService, ILanguageRepository _languageRepository)
    {
      fileService = _fileService;
      resourceRepository = _resourceRepository;
      resourcesService = _resourcesService;
      languageRepository = _languageRepository;
      resourcesService.BaseUri = GetUrl();
    }

    string GetUrl()
    {
      var request = HttpContext.Current.Request;
      var appUrl = HttpRuntime.AppDomainAppVirtualPath;

      if (appUrl != "/")
        appUrl = "/" + appUrl;

      var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

      return baseUrl;
    }

    [HttpGet]
    public IHttpActionResult GetLanguageByName(string name)
    {
      Language language = languageRepository.GetLanguageByName(name);

      return Ok(language);
    }

    [HttpGet]
    public IHttpActionResult GetLanguageByCode(string code)
    {
      Language language = languageRepository.GetLanguageByName(code);

      return Ok(language);
    }

    [HttpGet]
    public IHttpActionResult GetLanguages()
    {
      List<Language> languages = languageRepository.GetLanguages();

      return Ok(languages);
    }
  }
}
