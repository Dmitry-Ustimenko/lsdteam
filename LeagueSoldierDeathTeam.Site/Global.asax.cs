﻿using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
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
		private IContainer _diContainer;

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			var cInfo = new CultureInfo("ru-RU");
			Thread.CurrentThread.CurrentCulture = cInfo;
			Thread.CurrentThread.CurrentUICulture = cInfo;
		}

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

			_diContainer = builder.Build();
			if (_diContainer == null)
				throw new ArgumentNullException(string.Format("diContainer"));

			DependencyResolver.SetResolver(new AutofacDependencyResolver(_diContainer));
		}

		protected void Application_Disposed()
		{
			if (_diContainer != null)
				_diContainer.Dispose();
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
			{
				Response.StatusCode = httpCode;
			}
			else
			{
				Server.ClearError();
				Response.Clear();
				Response.Redirect(WebBuilder.BuildActionUrl<ErrorController>(o => o.Error(httpCode, exceptionId)), false);
			}
		}
	}
}
