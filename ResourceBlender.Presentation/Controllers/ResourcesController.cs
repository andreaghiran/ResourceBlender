using Ionic.Zip;
using Nelibur.ObjectMapper;
using PagedList;
using ResourceBlender.Common.Exceptions;
using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace ResourceBlender.Presentation.Controllers
{
  public class ResourcesController : Controller
  {
    private readonly IResourcesService _resourcesService;
    private readonly IResourceRepository _resourceRepository;
    private readonly IFileService _fileService;
    private readonly ILanguageService _languageService;
    
    public ResourcesController(IResourcesService resourcesService, IResourceRepository resourceRepository, IFileService fileService, ILanguageService languageService)
    {
      _resourcesService = resourcesService;
      resourcesService.BaseUri = GetUrl();
      _resourceRepository = resourceRepository;
      _fileService = fileService;
      _languageService = languageService;
      _languageService.BaseUri = GetUrl();
      _fileService.BaseUri = GetUrl();
    }

    string GetUrl()
    {
      var request =System.Web.HttpContext.Current.Request;
      var appUrl = HttpRuntime.AppDomainAppVirtualPath;

      if (appUrl != "/")
        appUrl = "/" + appUrl;

      var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

      return baseUrl;
    }

    public async Task<ActionResult> Index(int? page, string searchTerm = "")
    {
      var viewModel = await _resourcesService.GetResources();

      if (viewModel == null)
      {
        return View(viewModel);
      }
      else
      {
        if (!string.IsNullOrEmpty(searchTerm))
        {
          searchTerm = searchTerm.ToLower();
          viewModel = viewModel.Where(x => x.ResourceString.ToLower().Contains(searchTerm) ||
                                                                              x.Translations.Any(t => t.TranslationValue.ToLower().Contains(searchTerm)))
                                                                              .ToList();
        }

        int pageSize = 10;
        int pageNumber = (page ?? 1);
        return View(viewModel.ToPagedList(pageNumber, pageSize));
      }
    }

    [HttpGet]
    public ActionResult ImportResources() => View();


    [HttpPost]
    public ActionResult ImportResources(ImportFileViewModel model)
    {
      //if (model.files.Any(x => x == null))
      //{
      //  ModelState.AddModelError("RequiredFile", "Both files are required.");
      //}
      //if (ModelState.IsValid)
      //{

      //  _resourcesService.AddOrUpdateRomanianResourcesOnImport(model.files.ElementAt(0));
      //  _resourcesService.AddOrUpdateEnglishResourcesOnImport(model.files.ElementAt(1));

      //  return RedirectToAction("Index");
      //}
      //else
      //{

      //  return View(model);
      //}

      BinaryReader binaryReader = new BinaryReader(model.File.InputStream);
      byte[] binaryData = binaryReader.ReadBytes(model.File.ContentLength);

      MemoryStream memstream = new MemoryStream(binaryData);
      using (ZipArchive archive = new ZipArchive(memstream))
      {
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
          //var stream = entry.Open();
          _resourcesService.AddOrUpdateResourcesOnImport(entry);

        }
      }

      return View();
    }

    public PartialViewResult ShowErrorMessage(string message)
    {
      return PartialView("_Error");
    }

   

    //public ActionResult ExportEnglishResources()
    //{
    //  return new FileGeneratingResult("Resources.resx", "application/xml", stream => _fileService.GenerateExportFile(stream, LanguageEnumeration.English));
    //}

    public async Task<ActionResult> AddResource()
    {
      ResourceTranslationViewModel viewModel = await _resourcesService.GetNewResourceViewModel();

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> AddResource(ResourceTranslationViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          await _resourcesService.AddResource(model);
          return RedirectToAction("Index");
        }
        catch (ResourceAlreadyExistsException ex)
        {
          TempData["ErrorMessage"] = ex.Message;
          return View(model);
        }
      }
      else
      {
        return View(model);
      }
    }

    public ActionResult Update(int resourceId)
    {
      ResourceTranslationViewModel resource = _resourcesService.GetResourceById(resourceId);

      if (resource != null)
      {
        return View(resource);
      }

      return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> Update(ResourceTranslationViewModel model)
    {
      if (ModelState.IsValid)
      {
        await _resourcesService.UpdateResource(model);
        return RedirectToAction("Index");
      }
      else
      {
        TempData["ValidationMessage"] = "All translations are required.";
        return RedirectToAction("Update", new { resourceId = model.ResourceId });
      }
    }

    public ActionResult Delete(int resourceId)
    {
      ResourceTranslationViewModel resource = _resourcesService.GetResourceById(resourceId);

      if (resource != null)
      {
        return View(resource);
      }
      else
      {
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    public async Task<ActionResult> Delete(ResourceTranslationViewModel viewModel)
    {
      try
      {
        await _resourcesService.DeleteResource(viewModel);
        return RedirectToAction("Index");
      }
      catch (ResourceDoesNotExistException ex)
      {
        TempData["ErrorMessage"] = ex.Message;
        return View();
      }
    }

    public ActionResult GenerateResources() => View();

    public async Task<ActionResult> ExportResources()
    {
      var archive = await _fileService.GetArchive();
      return File(archive.ToArray(), "application/zip", "res.zip");
    }
  }
}