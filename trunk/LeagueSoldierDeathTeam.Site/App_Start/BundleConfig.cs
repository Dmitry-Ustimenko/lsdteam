using System.Web.Optimization;

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
				"~/Scripts/modernizr-{version}.js",
				"~/Scripts/jquery.blockUI.js"
				));

			bundles.Add(new ScriptBundle("~/bundles/js").Include(
				"~/Scripts/Plugins/*.js",

				"~/Scripts/Site/site.js",
				"~/Scripts/Site/site.ajax.js",
				"~/Scripts/Site/site.plugins.js",
				"~/Scripts/Site/site.layout.js",
				"~/Scripts/Site/Page/*.js"
				));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/Plugins/jquery-ui-{version}.css",
				"~/Content/Plugins/galleria.classic.css",
				"~/Content/Plugins/datepicker.css",
				"~/Content/Plugins/modal.css",
				"~/Content/Plugins/modal-responsive.css",
				"~/Content/Plugins/animate.css",
				"~/Content/Plugins/jquery.vegas.css",

				"~/Content/Plugins/HtmlEditor/sets/style.css",
				"~/Content/Plugins/HtmlEditor/skins/style.css",
				"~/Content/Plugins/HtmlEditor/xbbcode.css",

				"~/Content/Site/Site.css"
				));
		}
	}
}
