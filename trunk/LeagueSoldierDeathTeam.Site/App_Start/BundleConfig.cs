using System.Web.Optimization;

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
				));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/Site/Site.css"));
		}
	}
}
