using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ResourceBlender.Presentation.App_Start
{
  public class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{action}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );

      //config.Routes.MapHttpRoute(
      //    name: "DefaultApiWithAction",
      //    routeTemplate: "api/{controller}/{action}/{id}",
      //    defaults: new { id = RouteParameter.Optional }
      //);
    }
  }
}