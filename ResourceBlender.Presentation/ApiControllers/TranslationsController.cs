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
    public class TranslationsController : ApiController
    {
       private readonly IFileService fileService;
       private readonly IResourceRepository resourceRepository;
       private readonly ITranslationRepository translationRepository;
       private readonly IResourcesService resourcesService;
       private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
       public TranslationsController(IFileService _fileService, IResourceRepository _resourceRepository, IResourcesService _resourcesService, ITranslationRepository _translationRepository)
       {
         fileService = _fileService;
         resourceRepository = _resourceRepository;
         resourcesService = _resourcesService;
         translationRepository = _translationRepository;
         resourcesService.BaseUri = GetUrl();
       }
       
       string GetUrl()
       {
         var request = System.Web.HttpContext.Current.Request;
         var appUrl = HttpRuntime.AppDomainAppVirtualPath;
       
         if (appUrl != "/")
           appUrl = "/" + appUrl;
       
         var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
       
         return baseUrl;
       }

       [HttpGet]
       public IHttpActionResult GetTranslations()
       {
         List<Translation> translations = translationRepository.GetTranslations();
         return Ok(translations);
       }
  }
}
