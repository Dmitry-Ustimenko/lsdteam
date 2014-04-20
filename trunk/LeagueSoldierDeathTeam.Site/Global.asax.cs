using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using LeagueSoldierDeathTeam.Site.App_Start;
using LeagueSoldierDeathTeam.Site.Modules.Autofac;

namespace LeagueSoldierDeathTeam.Site
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
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
	}
}
