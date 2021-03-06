﻿using System.Web.Mvc;
using System.Web.Routing;

namespace LeagueSoldierDeathTeam.Site.App_Start
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("scripts/{*pathinfo}");
			routes.IgnoreRoute("content/{*pathinfo}");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

			routes.LowercaseUrls = true;
			routes.MapMvcAttributeRoutes();

			routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}
	}
}
