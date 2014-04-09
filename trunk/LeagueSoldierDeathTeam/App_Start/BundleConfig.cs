using System.Web.Optimization;

namespace LeagueSoldierDeathTeam.App_Start
{
	public static class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/Scripts/bundle").Include(
				"~/Scripts/jquery-{version}.js",
				"~/Scripts/jquery-ui-{version}.js",
				"~/Scripts/jquery.validate*"
				));

			bundles.Add(new StyleBundle("~/Content/bundle").Include(
				)
			);
		}
	}
}