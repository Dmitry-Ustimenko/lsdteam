﻿using System.Web.Optimization;

namespace LeagueSoldierDeathTeam.Site.App_Start
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-{version}.js",
				"~/Scripts/jquery-ui-{version}.js",
				"~/Scripts/jquery.validate*",
				"~/Scripts/jquery.blockUI.js"
				));

			bundles.Add(new ScriptBundle("~/bundles/js").Include(
				"~/Scripts/Plugins/galleria-1.3.5.js",

				"~/Scripts/Site/site.js",
				"~/Scripts/Site/site.ajax.js",
				"~/Scripts/Site/site.plugins.js",
				"~/Scripts/Site/site.layout.js",
				"~/Scripts/Site/site.home.js"
				));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/Plugins/galleria.classic.css",

				"~/Content/Site/Site.css"
				));
		}
	}
}