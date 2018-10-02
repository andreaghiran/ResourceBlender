using ResourceBlender.Domain;
using ResourceBlender.Repository.Contracts;
using ResourceBlender.Repository.Implementations;
using ResourceBlender.Services.Contracts;
using ResourceBlender.Services.Implementations;
using ResourceBlender.WindowsForms.Services.Implementation;
using ResourceBlender.WindowsForms.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Injection;

namespace ResourceBlender.WindowsForms
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// 

    [STAThread]
    static void Main()
    {
      var container = BuildUnityContainer();
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      container.Resolve<AddForm>();
      Application.Run(container.Resolve<MainForm>());
    }

    public static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();
      container.RegisterType<IResourceRepository, ResourceRepository>();
      container.RegisterType<IResourcesService, ResourcesService>();
      container.RegisterType<IFileService, FileService>();
      container.RegisterType<IFileResourceRepository, FileResourceRepository>();
      container.RegisterType<ResourceBlenderEntities, ResourceBlenderEntities>();
      container.RegisterType<ITextBoxOperation, TextBoxOperation>();

      return container;
    }
  }
}
