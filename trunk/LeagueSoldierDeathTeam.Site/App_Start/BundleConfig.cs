﻿using System.Web.Optimization;

namespace LeagueSoldierDeathTeam.Site.App_Start
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/jquery.validate*",
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/js").Include(
					"~/Scripts/Plugins/jssor.core.js",
					"~/Scripts/Plugins/jssor.utils.js",
					"~/Scripts/Plugins/jssor.slider.js"
				));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/Plugins/slider.css",

				"~/Content/Site/Site.css"));
		}
	}
}
