using Ionic.Zip;
using ResourceBlender.Common.Enums;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace ResourceBlender.Presentation.ApiControllers
{
  public class ResourcesController : ApiController
  {
    private readonly IResourceRepository resourceRepository;
    private readonly IFileService fileService;

    public ResourcesController(IResourceRepository _resourceRepository, IFileService _fileService)
    {
      resourceRepository = _resourceRepository;
      fileService = _fileService;
    }

    [HttpGet]
    public IHttpActionResult GetResources()
    {
      return Ok(resourceRepository.GetAllResources());
    }

    [HttpGet]
    public HttpResponseMessage UpdateRomanianResources()
    {
      var stream = fileService.GetResourceFile(LanguageEnumeration.Romanian);

      var result = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new ByteArrayContent(stream.ToArray())
      };
      result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
      {
        FileName = "Resources.ro.resx"
      };

      result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

      return result;
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
  }
}
