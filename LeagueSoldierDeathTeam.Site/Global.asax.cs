using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using LeagueSoldierDeathTeam.Site.App_Start;
using LeagueSoldierDeathTeam.Site.Classes;
using LeagueSoldierDeathTeam.Site.Controllers;
using LeagueSoldierDeathTeam.Site.Modules.Autofac;

namespace LeagueSoldierDeathTeam.Site
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			Logger.InitLogger();
			Logger.WriteEvent("Application started");

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			var builder = new ContainerBuilder();
			builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
			builder.RegisterModule<LibraryModule>();

			var diContainer = builder.Build();
			if (diContainer == null)
				throw new ArgumentNullException(string.Format("diContainer"));

			DependencyResolver.SetResolver(new AutofacDependencyResolver(diContainer));
		}

		protected void Application_Error()
		{
			var exception = Server.GetLastError();
			if (exception == null)
				return;

			var exceptionId = Guid.NewGuid().ToString().Replace("-", "");
			Logger.WriteEmergency(exception, "Error: " + exceptionId);

			var httpError = exception as HttpException;
			var httpCode = httpError != null ? httpError.GetHttpCode() : 500;

			var httpRequestWrapper = WebBuilder.GetHttpRequestWrapper(Request);
			if (httpRequestWrapper.IsAjaxRequest())
				Response.StatusCode = httpCode;
			else
			{
				Server.ClearError();
				Response.Clear();
				Response.Redirect(WebBuilder.BuildActionUrl<ErrorController>(o => o.Error(httpCode, exceptionId)), false);
			}
		}
	}
}
