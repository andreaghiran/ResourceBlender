using Ionic.Zip;
using Nelibur.ObjectMapper;

using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ResourceBlender.Presentation.ApiControllers
{
  public class ResourcesController : ApiController
  {
    private readonly IFileService fileService;
    private readonly IResourceRepository resourceRepository;
    private readonly IResourcesService resourcesService;
    private readonly ILanguageService languageService;
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public ResourcesController(IFileService _fileService, IResourceRepository _resourceRepository, IResourcesService _resourcesService, ILanguageService _languageService)
    {
      fileService = _fileService;
      resourceRepository = _resourceRepository;
      resourcesService = _resourcesService;
      languageService = _languageService;
      resourcesService.BaseUri = GetUrl();
      languageService.BaseUri = GetUrl();
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

    //[HttpGet]
    //public HttpResponseMessage GetZip()
    //{
    //  var archive = fileService.GetArchive();

    //  var result = new HttpResponseMessage(HttpStatusCode.OK)
    //  {
    //    Content = new ByteArrayContent(archive.ToArray())
    //  };
    //  result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
    //  {
    //    FileName = "res.zip"
    //  };

    //  result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

    //  return result;
    //}

    [HttpPost]
    public async Task<IHttpActionResult> AddResource(ResourceTranslationViewModel resource)
    {
      Resource resourceEntity = new Resource();
      resourceEntity.ResourceString = resource.ResourceString;
      
      foreach (var translation in resource.Translations)
      {
        Translation translationEntity = new Translation()
        {
          TranslationString = translation.TranslationValue,
          Language_Id = await languageService.GetLanguageId(translation.LanguageValue),
        };

        resourceEntity.Translations.Add(translationEntity);
      }

      resourceRepository.AddResource(resourceEntity);

      return Ok(resource);
    }

    [HttpPost]
    public IHttpActionResult UpdateResource(ResourceTranslationViewModel resource)
    {

      Resource resourceEntity = new Resource();
      resourceEntity.Translations = resource.Translations.Select(x => new Translation
                                                                          {
                                                                            Id = x.TranslationId,
                                                                            TranslationString = x.TranslationValue
                                                                          }).ToList();


      resourceRepository.EditResource(resourceEntity);

      return Ok(resource);
    }

    [HttpDelete]
    public IHttpActionResult DeleteResource(int resourceId)
    {
      resourceRepository.DeleteResource(resourceId);

      return Ok();
    }

    [HttpGet]
    public async Task<IHttpActionResult> FindResourceByName(string resourceName)
    {
      var resource = await resourceRepository.GetResourceByName(resourceName);

      return Ok(resource);
    }
  }
}
