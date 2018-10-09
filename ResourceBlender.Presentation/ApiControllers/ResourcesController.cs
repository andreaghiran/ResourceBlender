using Ionic.Zip;
using Nelibur.ObjectMapper;
using ResourceBlender.Common.Enums;
using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
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

    public ResourcesController(IFileService _fileService, IResourceRepository _resourceRepository, IResourcesService _resourcesService)
    {
      fileService = _fileService;
      resourceRepository = _resourceRepository;
      resourcesService = _resourcesService;
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
    public IHttpActionResult GetAllResources()
    {
      var resources = resourceRepository.GetAllResources();
      return Ok(resources);
    }

    [HttpGet]
    public HttpResponseMessage GetZip()
    {
      var archive = fileService.GetArchive();

      var result = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new ByteArrayContent(archive.ToArray())
      };
      result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
      {
        FileName = "res.zip"
      };

      result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

      return result;
    }

    [HttpPost]
    public IHttpActionResult AddResource(ResourceViewModel resource)
    {
      resourceRepository.AddResource(TinyMapper.Map<Resource>(resource));

      return Ok(resource);
    }

    [HttpPost]
    public IHttpActionResult UpdateResource(ResourceViewModel resource)
    {
      resourceRepository.EditResource(TinyMapper.Map<Resource>(resource));

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
