﻿using System;
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

			Server.ClearError();
			Response.Clear();

			var exceptionId = Guid.NewGuid().ToString().Replace("-", "");
			Logger.WriteEmergency(exception, "Error: " + exceptionId);

			var httpError = exception as HttpException;
			if (httpError != null && httpError.GetHttpCode() == 404)
			{
				exceptionId = "PageNotFound";
				Response.StatusCode = httpError.GetHttpCode();
			}
			else
				Response.StatusCode = 500;

			Response.Status = "error";
			Response.Redirect(WebBuilder.BuildActionUrl<ErrorController>(o => o.Error(exceptionId)), false);
		}
	}
}
