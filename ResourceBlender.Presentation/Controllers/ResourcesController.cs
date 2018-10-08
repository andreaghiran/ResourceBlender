using Ionic.Zip;
using Nelibur.ObjectMapper;
using PagedList;
using ResourceBlender.Common.Enums;
using ResourceBlender.Common.Exceptions;
using ResourceBlender.Common.FileGeneration;
using ResourceBlender.Common.ViewModels;
using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Services.Contracts;
using System;
using System.Activities.Statements;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
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

    public ResourcesController(IResourcesService resourcesService, IResourceRepository resourceRepository, IFileService fileService)
    {
      _resourcesService = resourcesService;
      _resourceRepository = resourceRepository;
      _fileService = fileService;
    }

    public async Task<ActionResult> Index(int? page, string searchTerm = "")
    {
      //var viewModel = _resourcesService.GetResourceViewModelList();
      var viewModel = await _resourcesService.GetResourceViewModelListTask();

      if (viewModel == null)
      {
        return View(viewModel);
      }
      else

      {
        if (!string.IsNullOrEmpty(searchTerm))
        {
          searchTerm = searchTerm.ToLower();
          viewModel = _resourcesService.GetResourceViewModelList().Where(x => x.ResourceString.ToLower().Contains(searchTerm) ||
                                                                              x.EnglishTranslation.ToLower().Contains(searchTerm) ||
                                                                              x.RomanianTranslation.ToLower().Contains(searchTerm))
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
      if (model.files.Any(x => x == null))
      {
        ModelState.AddModelError("RequiredFile", "Both files are required.");
      }
      if (ModelState.IsValid)
      {

        _resourcesService.AddOrUpdateRomanianResourcesOnImport(model.files.ElementAt(0));
        _resourcesService.AddOrUpdateEnglishResourcesOnImport(model.files.ElementAt(1));

        return RedirectToAction("Index");
      }
      else
      {

        return View(model);
      }
    }

    public PartialViewResult ShowErrorMessage(string message)
    {
      return PartialView("_Error");
    }

    public ActionResult ExportResources()
    {
      var archive = _fileService.GetArchive();
      return File(archive.ToArray(), "application/zip", "res.zip");
    }

    //public ActionResult ExportEnglishResources()
    //{
    //  return new FileGeneratingResult("Resources.resx", "application/xml", stream => _fileService.GenerateExportFile(stream, LanguageEnumeration.English));
    //}

    public ActionResult AddResource() => View();

    [HttpPost]
    public async Task<ActionResult> AddResource(ResourceViewModel model)
    {
      if (ModelState.IsValid)
      {
        var resourceAlreadyExists = await _resourcesService.CheckIfResourceWithNameExists(model.ResourceString);

        if(resourceAlreadyExists)
        {
          TempData["ErrorMessage"] = "Resource already exists.";
          return View(model);
        }

        await _resourcesService.SendAndAddResource(model);
        return RedirectToAction("Index");
      }
      else
      {
        return View(model);
      }
    }

    public async Task<ActionResult> Update(string resourceName)
    {
      Resource resource = await _resourcesService.FindResourceByName(resourceName);

      if (resource != null)
      {
        ResourceViewModel viewModel = TinyMapper.Map<ResourceViewModel>(resource);
        return View(viewModel);
      }

      return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> Update(ResourceViewModel model)
    {
      if (ModelState.IsValid)
      {
        await _resourcesService.SendAndUpdateResource(model);
        return RedirectToAction("Index");
      }
      else
      {
        return View(model);
      }
    }

    public async Task<ActionResult> Delete(string resourceName)
    {
      Resource resource = await _resourcesService.FindResourceByName(resourceName);

      if(resource != null)
      {
        ResourceViewModel viewModel = TinyMapper.Map<ResourceViewModel>(resource);
        return View(viewModel);
      }
      else
      {
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    public async Task<ActionResult> DeleteResource(ResourceViewModel viewModel)
    {
      await _resourcesService.SendAndDeleteResource(viewModel);
      return RedirectToAction("Index");
    }

    public ActionResult GenerateResources() => View();
  }
}